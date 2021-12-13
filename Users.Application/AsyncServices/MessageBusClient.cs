using EasyNetQ;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using Users.Application.Models;

namespace Users.Application.AsyncServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IBus _connection;

        public MessageBusClient()
        {
            _connection = RabbitHutch.CreateBus(ConfigurationManager.ConnectionStrings["RabbitMQ"].ConnectionString);
            _connection.Advanced.Disconnected += Disconnect_Handler;
        }

        public void Publish(Message message)
        {
            _connection.PubSub.Publish(JsonConvert.SerializeObject(message));
        }

        private void Disconnect_Handler(object sender, DisconnectedEventArgs e)
        {
            Debug.WriteLine("--> Bus disconnected");
        }
    }
}
