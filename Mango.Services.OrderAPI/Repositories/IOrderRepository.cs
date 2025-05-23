﻿using Mango.Services.OrderAPI.Models;

namespace Mango.Services.OrderAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> Store(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
