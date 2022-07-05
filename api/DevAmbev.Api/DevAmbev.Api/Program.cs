using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Commands.Users;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Mappers;
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

//queries


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
