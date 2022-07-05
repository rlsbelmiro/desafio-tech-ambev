using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Commands.Users;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Core.Queries.Users;
using DevAmbev.Infra.Data;
using DevAmbev.Infra.Repositories;
using DevAmbev.Infra.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

//commands
builder.Services.AddTransient<ICommand<UserRequest, UserResponse>, CreateUserCommand>();
builder.Services.AddTransient<ICommand<UserUpdateRequest, UserResponse>, UpdateUserCommand>();
builder.Services.AddTransient<ICommand<int, BaseResponse>, DeleteUserCommand>();

//queries
builder.Services.AddTransient<IQuery<UserListRequest, UserListResponse>, ListUserQuery>();
builder.Services.AddTransient<IQuery<int, UserResponse>, FindByIdUserQuery>();
builder.Services.AddTransient<IQuery<LoginUserRequest, LoginUserResponse>, LoginUserQuery>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
