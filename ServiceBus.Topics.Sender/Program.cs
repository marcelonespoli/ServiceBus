
using Azure.Messaging.ServiceBus;

const string ServiceConnectionString = "Endpoint=sb://az-course-mns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zLUh1iLTzzyfRnIQ/tGLOWWzcPnAIjzpc+ASbLp7gas=";
//const string QueueName = "high-priority";
const string TopicName = "order-topic";
const int MaxNumberOfMessages = 3;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(ServiceConnectionString);
//sender = client.CreateSender(QueueName);
sender = client.CreateSender(TopicName);

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
for (int i = 1; i <= MaxNumberOfMessages; i++)
{
    if (batch.TryAddMessage(new ServiceBusMessage($"This a message - {i}")))
        Console.WriteLine($"Message - {i} was added to the batch");
}

try
{
    await sender.SendMessagesAsync(batch);
    Console.WriteLine("Batch of Messages Sent");
}
catch (Exception ex)
{
    Console.WriteLine("Messages not sent, Error: " + ex.Message);
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}


