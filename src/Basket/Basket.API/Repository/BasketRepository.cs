using Basket.API.Data.Interface;
using Basket.API.Entities;
using Basket.API.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketCardContext _context;
        public BasketRepository(IBasketCardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BasketCard> GetBasket(string userName)
        {
            var basket = await _context
                                .Redis
                                .StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<BasketCard>(basket);
        }

        public async Task<BasketCard> UpdateBasket(BasketCard basket)
        {
            var updated = await _context
                              .Redis
                              .StringSetAsync(basket.Username, JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }
            return await GetBasket(basket.Username);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _context
                            .Redis
                            .KeyDeleteAsync(userName);
        }
    }



}
