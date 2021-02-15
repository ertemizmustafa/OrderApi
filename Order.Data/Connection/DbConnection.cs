using Microsoft.Extensions.Options;
using Order.Data.Configuration;
using System;
using System.Data;

namespace Order.Data.Connection
{
    public class DbConnection<TConnection> : IDbConnectionFactory where TConnection : IDbConnection, new()
    {
        private readonly IOptions<ConnectionOption> _connectionOptions;
        public DbConnection(IOptions<ConnectionOption> connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new TConnection
            {
                ConnectionString = _connectionOptions.Value.ConnectionString
            };

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while connecting to the database. See innterExcepiton for details.", ex);
            }

            return connection;
        }
    }
}

