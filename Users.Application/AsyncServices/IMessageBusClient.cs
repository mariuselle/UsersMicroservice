using Users.Application.Models;

namespace Users.Application.AsyncServices
{
    public interface IMessageBusClient
    {
        void Publish(Message message);
    }
}