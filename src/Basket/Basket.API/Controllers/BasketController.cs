using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producers;
using System;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, ILogger<BasketController> logger, EventBusRabbitMQProducer eventBus, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCard), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCard>> GetBasket(string username)
        {
            var basket = await _repository.GetBasket(username);
            return Ok(basket ?? new BasketCard(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCard), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCard>> UpdateBasket([FromBody] BasketCard basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            return Ok(await _repository.DeleteBasket(username));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasket(basketCheckout.Username);
            if (basket == null)
            {
                _logger.LogError("Basket could not be found for user: " + basketCheckout.Username);
                return BadRequest();
            }

            var removed = await _repository.DeleteBasket(basketCheckout.Username);
            if (!removed)
            {
                _logger.LogError("Basket could not be removed for user: " + basketCheckout.Username);
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();

            try
            {
                _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("Error: Message cannot be published for user: " + basketCheckout.Username + ", " + e.Message);
                throw;
            }

            return Accepted();
        }
    }
}
