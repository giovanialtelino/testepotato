namespace SqsMassTransit;
public interface IMassTransitProducer
{
    Task SendWeather(WeatherForecast weather);
}
