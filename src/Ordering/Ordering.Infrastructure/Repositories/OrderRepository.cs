using Ordering.Core.Entities;
using Ordering.Core.Repository;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> DeleteOrder(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> UpderOrders(string ordername)
        {
            throw new NotImplementedException();
        }
    }
}
