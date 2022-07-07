using AutoMapper;
using DevAmbev.Api;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Commands.Customers;
using DevAmbev.Core.Commands.Orders;
using DevAmbev.Core.Commands.Products;
using DevAmbev.Core.Commands.Users;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Orders;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Core.Queries.Customers;
using DevAmbev.Core.Queries.Orders;
using DevAmbev.Core.Queries.Products;
using DevAmbev.Core.Queries.Users;
using DevAmbev.Infra.Data;
using DevAmbev.Infra.Repositories;
using DevAmbev.Infra.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//commands

#region Users
builder.Services.AddTransient<ICommand<UserRequest, UserResponse>, CreateUserCommand>();
builder.Services.AddTransient<ICommand<UserUpdateRequest, UserResponse>, UpdateUserCommand>();

builder.Services.AddTransient<IQuery<UserListRequest, UserListResponse>, ListUserQuery>();
builder.Services.AddTransient<IQuery<int, UserResponse>, FindByIdUserQuery>();
builder.Services.AddTransient<IQuery<LoginUserRequest, LoginUserResponse>, LoginUserQuery>();
builder.Services.AddTransient<ICommand<int, UserResponse>, DeleteUserCommand>();
#endregion

#region Products
builder.Services.AddTransient<ICommand<ProductRequest, ProductResponse>, CreateProductCommand>();
builder.Services.AddTransient<ICommand<ProductUpdateRequest, ProductResponse>, UpdateProductCommand>();
builder.Services.AddTransient<ICommand<int, ProductResponse>, DeleteProductCommand>();

builder.Services.AddTransient<IQuery<ProductListRequest, ProductListResponse>, ListProductQuery>();
builder.Services.AddTransient<IQuery<int, ProductResponse>, FindByIdProductQuery>();
#endregion

#region Customers
builder.Services.AddTransient<ICommand<CustomerRequest, CustomerResponse>, CreateCustomerCommand>();
builder.Services.AddTransient<ICommand<CustomerUpdateRequest, CustomerResponse>, UpdateCustomerCommand>();
builder.Services.AddTransient<ICommand<int, BaseResponse>, DeleteCustomerCommand>();

builder.Services.AddTransient<IQuery<CustomerListRequest, CustomerListResponse>, ListCustomerQuery>();
builder.Services.AddTransient<IQuery<int, CustomerResponse>, FindByIdCustomerQuery>();
#endregion

#region Orders
builder.Services.AddTransient<ICommand<OrderRequest, OrderResponse>, CreateOrderCommand>();
builder.Services.AddTransient<IQuery<OrderListRequest, OrderListResponse>, ListOrderQuery>();
builder.Services.AddTransient<IQuery<int, OrderResponse>, FindByIdOrderQuery>();
#endregion


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "devambev.api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.
                   \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.
                    \r\n\r\nExample: 'Bearer 12345abcdef'",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddCors();

builder.Logging.AddLog4Net();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(opt =>
{
    opt.AllowAnyOrigin();
    opt.AllowAnyMethod();
    opt.AllowAnyHeader();
}); ;
app.Run();
