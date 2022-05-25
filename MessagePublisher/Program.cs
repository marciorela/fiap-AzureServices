using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace MessagePublisher
{
    public class Program
    {
        private const string storageConnectionString = "Endpoint=sb://sbnamespacenet21.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ohIwN7VYsWznhwe/zKnwSgmSJ6cSyhtaDOvBRHOnukU=";
        private const string queueName = "messagequeuemarciorela";
        private const int numOfMessages = 30;
        static ServiceBusClient client;
        static ServiceBusSender sender;

        public static async Task Main(string[] args)
        {
            client = new ServiceBusClient(storageConnectionString);
            sender = client.CreateSender(queueName);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            for (int i = 1; i <= numOfMessages; i++)
            {
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }
            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}