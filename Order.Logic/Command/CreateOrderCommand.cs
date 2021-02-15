using MediatR;
using Order.Data.Connection;
using Order.Logic.Model;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Order.Logic.Command
{
    public class CreateOrderCommand : IRequest<ResponseModel<string>>
    {
        public OrderModel OrderModel { get; }
        public CreateOrderCommand(OrderModel orderModel)
        {
            OrderModel = orderModel;
        }
    }

    public class CreateMailCommandHandler : IRequestHandler<CreateOrderCommand, ResponseModel<string>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<CreateMailCommandHandler> _logger;
        public CreateMailCommandHandler(ILogger<CreateMailCommandHandler> logger, IDbConnectionFactory dbConnectionFactory)
        {
            _logger = logger;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ResponseModel<string>> Handle(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken)
        {
            string sql = @"INSERT INTO Orders (BrandId, Price, StoreName, CustomerName, CreatedOn, Status) Values (@BrandId, @Price, @StoreName, @CustomerName, @CreatedOn, @Status)";

            using var connection = _dbConnectionFactory.CreateConnection();

            var insertCount = 0;
            foreach (var item in createOrderCommand.OrderModel.Orders.Where(x => x.BrandId > 0))
            {
                insertCount += await connection.ExecuteAsync(sql, new
                {
                    item.BrandId,
                    item.Price,
                    item.StoreName,
                    item.CustomerName,
                    item.CreatedOn,
                    item.Status,
                }, commandType: CommandType.Text);
            }

            var invalidBrandIdCount = createOrderCommand.OrderModel.Orders.Where(x => x.BrandId <= 0).Count();

            if (invalidBrandIdCount > 0)
                _logger.LogWarning($"BrandId less than 1 item count: {invalidBrandIdCount}");


            return ResponseModel.Ok($"Total insert count: {insertCount}");
        }
    }
}
