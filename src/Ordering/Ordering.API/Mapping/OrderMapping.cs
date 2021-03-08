using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.API.DTOs;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<BasketCheckoutEvent, Order>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
