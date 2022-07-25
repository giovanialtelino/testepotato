using MassTransit;
using MassTransit.AmazonSqsTransport;

namespace SqsMassTransit;
public static class MassTransitConsumer
{
    public static void AddMassTransitConsumer(this IServiceCollection services)
    {
        try
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(Program).Assembly);

                x.UsingAmazonSqs((context, config) =>
                {
                    config.Host("sa-east-1", h =>
                    {
                        h.AccessKey("");
                        h.SecretKey("");
                    });

                    config.ReceiveEndpoint("MassTransit", e =>
                    {
                        e.Consumer(() => new Printer());
                    });

                    config.ConfigureEndpoints(context);
                });
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        } 
    }
}

public class Printer : IConsumer<WeatherForecast>
{
    public async Task Consume(ConsumeContext<WeatherForecast> context)
    {
        Console.WriteLine(context.Message.Summary);
        Console.WriteLine(context.Message.TemperatureF);
        Console.WriteLine(context.Message.Date);

    }
}

