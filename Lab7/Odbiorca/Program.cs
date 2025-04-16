using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using utils;


var factory = new ConnectionFactory()
{
    UserName = "guest",
    Password = "guest",
    HostName = "localhost",
};
await Task.Delay(1000);

try
{
    await using (var connection = await factory.CreateConnectionAsync())
    await using (var channel = await connection.CreateChannelAsync())
    {
        await channel.QueueDeclareAsync("message-queue", false, false, false, null);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: true);
        var consumer = new AsyncEventingBasicConsumer(channel);
        await channel.BasicConsumeAsync("message-queue", false, consumer);
        
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            int jobCounter = (int)ea.BasicProperties.Headers["Job-counter"];
            int jobMs = (int)ea.BasicProperties.Headers["Job-ms"];
            ConsoleCol.WriteLine(
                $"[Odbiorca] Received:{message};Headers[{jobMs}, {jobCounter}]", 
                ConsoleColor.Yellow
            );
            await Task.Delay(jobMs);
            ConsoleCol.WriteLine(
                $"[Odbiorca] Done:{message};Headers[{jobMs}, {jobCounter}]", 
                ConsoleColor.Yellow
            );
            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        Console.WriteLine("Press [ENTER] to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

