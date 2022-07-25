using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;
using System.Text.Json;
using Amazon;

namespace SQSPoc;

public class Consumer : BackgroundService
{
    private readonly AmazonSQSClient _amazonSQSClient;
    public Consumer()
    {
        _amazonSQSClient = AmazonCredential.CreateClient();
    }

    public async Task GetMessagesAsync(CancellationToken cancellationToken = default)
    {
        var queueUrl = "https://sqs.sa-east-1.amazonaws.com//MassTransit";

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = await _amazonSQSClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,

                }, cancellationToken);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new AmazonSQSException($"Failed to GetMessagesAsync for queue. Response: {response.HttpStatusCode}");
                }

                var messageList = new List<WeatherForecast>();
                foreach (var item in response.Messages)
                {
                    var weather = JsonSerializer.Deserialize<WeatherForecast>(item.Body);

                    if (weather is not null)
                    {
                        messageList.Add(weather);
                        Console.WriteLine(weather.Summary);
                    }

                    await _amazonSQSClient.DeleteMessageAsync(new DeleteMessageRequest(queueUrl, item.ReceiptHandle));
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"Failed to GetMessagesAsync for queue  because the task was canceled");

            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to GetMessagesAsync for queue");
                throw;
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await GetMessagesAsync(stoppingToken);
    }
}

