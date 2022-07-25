namespace SQSPoc
{
    public interface IProducer
    {
        Task PostMessageAsync<T>(T message);
    }
}