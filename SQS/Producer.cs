using Amazon.SQS.Model;
using System.Text.Json;
using Amazon;
using Amazon.SQS;

namespace SQSPoc;

public class Producer : IProducer
{
    private readonly AmazonSQSClient _amazonSQSClient;
    public Producer()
    {
        _amazonSQSClient = AmazonCredential.CreateClient();
    }

    public async Task PostMessageAsync<T>(T message)
    {
        var queueUrl = "https://sqs.sa-east-1.amazonaws.com//MassTransit";

        try
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(message),
            };

            await _amazonSQSClient.SendMessageAsync(sendMessageRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to PostMessagesAsync to queue. Exception: {ex.Message}");
            throw;
        }
    }
}

