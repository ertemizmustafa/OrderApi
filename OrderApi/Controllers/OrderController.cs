using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.Logic.Command;
using Order.Logic.Model;
using Order.Logic.Query;
using OrderApi.Base;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;

        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("FilterOrder")]
        public async Task<IActionResult> FilterOrder([FromBody] OrderFilterModel orderFilterModel)
        {
            _logger.LogInformation($"Filter order with : {JsonConvert.SerializeObject(orderFilterModel)}");
            return FromResult(await _mediator.Send(new GetOrdersQuery(orderFilterModel)));
        }


        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            _logger.LogInformation($"Create order with : {JsonConvert.SerializeObject(orderModel)}");
            return FromResult(await _mediator.Send(new CreateOrderCommand(orderModel)));
        }
    }
}
