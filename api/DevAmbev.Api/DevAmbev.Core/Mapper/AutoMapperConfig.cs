using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Domain.Entities;

namespace DevAmbev.Core.Mappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();
            CreateMap<LoginUserResponse, User>().ReverseMap();

            CreateMap<ProductRequest, Product>().ReverseMap();
            CreateMap<ProductResponse, Product>().ReverseMap();

            CreateMap<CustomerRequest, Customer>().ReverseMap();
            CreateMap<CustomerResponse, Customer>().ReverseMap();
        }
    }
}
