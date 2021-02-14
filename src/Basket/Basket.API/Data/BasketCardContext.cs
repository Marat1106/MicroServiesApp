using Basket.API.Data.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public class BasketCardContext:IBasketCardContext
    {
        private readonly ConnectionMultiplexer _connection;

        public BasketCardContext(ConnectionMultiplexer connection)
        {
            _connection = connection;
            Redis = _connection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
