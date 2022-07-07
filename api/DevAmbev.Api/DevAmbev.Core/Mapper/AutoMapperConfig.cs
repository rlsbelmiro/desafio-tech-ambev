using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Orders;
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

            CreateMap<OrderRequest, Order>().ReverseMap();
            CreateMap<OrderItemRequest, OrderItem>().ReverseMap();
            CreateMap<OrderResponse, Order>();
            CreateMap<Order, OrderResponse>()
                .ForMember(dst => dst.CustomerName,
                            map => map.MapFrom(src => src.Customer != null ? src.Customer.Name : "")
                );
            CreateMap<OrderItemResponse, OrderItem>();
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dst => dst.ProductName,
                            map => map.MapFrom(src => src.Product != null ? src.Product.Name : "")
                )
                .ForMember(dst => dst.Price,
                            map => map.MapFrom(src => src.UnityPrice)
                );
        }
    }
}
