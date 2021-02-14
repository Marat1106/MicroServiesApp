using Ordering.Core.Entities;
using Ordering.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string userName);
        Task<bool> DeleteOrder(string userName);
        Task<IEnumerable<Order>> UpderOrders(string ordername);

    }
}
