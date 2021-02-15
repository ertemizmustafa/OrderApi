using MediatR;
using Order.Data.Connection;
using Order.Logic.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Order.Logic.Common;

namespace Order.Logic.Query
{
    public class GetOrdersQuery : IRequest<ResponseModel<IEnumerable<object>>>
    {
        public OrderFilterModel OrderFilterModel { get; }

        public GetOrdersQuery(OrderFilterModel orderFilterModel)
        {
            OrderFilterModel = orderFilterModel;
        }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ResponseModel<IEnumerable<object>>>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ResponseModel<IEnumerable<object>>> Handle(GetOrdersQuery getOrdersQuery, CancellationToken cancellationToken)
        {
            var builder = new SqlBuilder();
            
            var sql = builder.AddTemplate(@"select * from Orders /**where**/ /**orderby**/ LIMIT @Next OFFSET @Offset",
                 new
                 {
                     Offset = getOrdersQuery.OrderFilterModel.PageNumber - 1,
                     Next = getOrdersQuery.OrderFilterModel.PageSize
                 });

            if (!string.IsNullOrEmpty(getOrdersQuery.OrderFilterModel.SearchText))
            {
                builder.Where("StoreName || CustomerName like @SearchText", new { SearchText = "%" + getOrdersQuery.OrderFilterModel.SearchText + "%" });
            }

            if (getOrdersQuery.OrderFilterModel.StartDate.HasValue && getOrdersQuery.OrderFilterModel.EndDate.HasValue)
            {
                builder.Where("CreatedOn >= @StartDate AND CreatedOn <= @EndDate", new { StartDate = getOrdersQuery.OrderFilterModel.StartDate.Value, EndDate = getOrdersQuery.OrderFilterModel.EndDate.Value });
            }

            if (getOrdersQuery.OrderFilterModel.Statuses.ContainItem())
            {
                builder.Where("Status in (@Statuses)", new { getOrdersQuery.OrderFilterModel.Statuses });
            }

            if (!string.IsNullOrEmpty(getOrdersQuery.OrderFilterModel.SortBy))
            {
                var sortColumns = OrderInfoModel.GetMatchOrderByFields(getOrdersQuery.OrderFilterModel.SortBy);
                if (sortColumns.ContainItem())
                    builder.OrderBy($"{string.Join(",", sortColumns)}");
            }

            using var connection = _dbConnectionFactory.CreateConnection();
            var result = await connection.QueryAsync<object>(sql.RawSql, sql.Parameters);
            return ResponseModel.Ok(result);
        }

    }


}
