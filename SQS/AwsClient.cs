using Amazon.SQS;
using Amazon;
using Amazon.Runtime;

namespace SQSPoc;
public static class AmazonCredential
{
    public static AmazonSQSClient CreateClient()
    {
        var sqsConfig = new AmazonSQSConfig
        {
            RegionEndpoint = RegionEndpoint.SAEast1
        };

        var awsCredentials = new BasicAWSCredentials("", "");
        return new AmazonSQSClient(awsCredentials, sqsConfig);
    }
}
