﻿using System.Threading.Tasks;
using Fake.Domain.Repositories;

namespace Domain.Aggregates.OrderAggregate;

public interface IOrderRepository:IRepository<Order>
{
    Task<Order> AddAsync(Order order);

    void Update(Order order);

    Task<Order> GetAsync(int orderId);
}