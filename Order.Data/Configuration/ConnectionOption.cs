using Microsoft.Data.Sqlite;
using System;
using System.Data.Common;

namespace Order.Data.Configuration
{
    public class ConnectionOption : IDisposable
    {
        private DbConnection _connection;

        public string ConnectionString { get; set; }

        public DbConnection CreateOrderDatabase()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("Provide connectionstring first. Cannot raise db");
            }
            _connection = new SqliteConnection(ConnectionString);

            _connection.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS Orders ([Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [BrandId] INT NOT NULL, [Price]  DECIMAL(5,2) NOT NULL, [StoreName] NVARCHAR(128) NULL, [CustomerName] NVARCHAR(128)  NULL, [CreatedOn] DATE, [Status] INT NOT NULL)";
            var command = _connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();

            return _connection;
        }

        public void Dispose() => _connection.Dispose();
    }

}
