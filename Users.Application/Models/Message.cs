namespace Users.Application.Models
{
    public class Message
    {
        public MessageType Type { get; set; }
        public object Data { get; set; }
    }


    public enum MessageType
    {
        ADD_USER,
        DELETE_USER
    }
}
