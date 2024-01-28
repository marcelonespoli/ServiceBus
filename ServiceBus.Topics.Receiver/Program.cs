using Azure.Messaging.ServiceBus;

const string ServiceConnectionString = "Endpoint=sb://az-course-mns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zLUh1iLTzzyfRnIQ/tGLOWWzcPnAIjzpc+ASbLp7gas=";
//const string QueueName = "high-priority";
const string TopicName = "order-topic";
const string Subscription1Name = "Sub1";

ServiceBusClient client;
ServiceBusProcessor processor = default!;

async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    string body = processMessageEventArgs.Message.Body.ToString();
    Console.WriteLine($"{body} - Subscription: {Subscription1Name}");
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processMessageEventArgs)
{
    Console.WriteLine(processMessageEventArgs.Exception.ToString());
    return Task.CompletedTask;
}

client = new ServiceBusClient(ServiceConnectionString);
//processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());
processor = client.CreateProcessor(TopicName, Subscription1Name, new ServiceBusProcessorOptions());

try
{
    // Everytime this event is fired I want to call the corresponding message handler
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end teh processing");
    Console.ReadKey();
    Console.WriteLine("Stopped receiving message");
}
catch (Exception ex)
{
    Console.WriteLine("Exception: " + ex.Message);
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}
