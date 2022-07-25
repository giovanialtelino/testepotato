namespace SqsMassTransit;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using System.Text.Json;

public class MassTransitProducer : IMassTransitProducer
{
    private readonly IBus _producer;
    public MassTransitProducer(IBus producer)
    {
        _producer = producer;

        EndpointConvention.Map<WeatherForecast>(
            new Uri("amazonsqs://sqs.sa-east-1.amazonaws.com//MassTransit"));
    }

    public async Task SendWeather(WeatherForecast weather)
    {
        try
        {
            var weatherJson = JsonSerializer.Serialize(weather);

            await _producer.Send<WeatherForecast>(weather);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
