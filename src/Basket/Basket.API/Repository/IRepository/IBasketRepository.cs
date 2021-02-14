using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repository.IRepository
{
    public interface IBasketRepository
    {
        Task<BasketCard> GetBasket(string userName);
        Task<BasketCard> UpdateBasket(BasketCard basket);
        Task<bool> DeleteBasket(string userName);
    }
}
