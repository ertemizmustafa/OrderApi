using System.Data;

namespace Order.Data.Connection
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
