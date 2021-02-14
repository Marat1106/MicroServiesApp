using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection:IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected)
            {
                TryConnect();
            }

        }
        public bool IsConnected
        {
            get 
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }


        public bool TryConnect()
        {
            try
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }
            catch (Exception e){
                Console.WriteLine("Error"+e.Message);
                return false;
            }
            if (IsConnected)
            {
                Console.WriteLine("Connection acquired!");
                return true;
            }
            else
            {
                Console.WriteLine("Error");
                return false;
            }
        }
        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                _connection.Dispose();
                _disposed = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No connections");
            }
            return _connection.CreateModel(); 
        }
    }
}
