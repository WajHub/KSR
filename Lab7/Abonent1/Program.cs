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

string[] messages = {"Test1(1)", "Test2(2)","Message1(3)", "message example(4)", "TEST-5(5)", "Message - 6(6)", "777(7)", "Mess-8(8)", "Test-9(9)", "Last(10)" };

try
{
    await using (var connection = await factory.CreateConnectionAsync())
    await using (var channel = await connection.CreateChannelAsync())
    {
        await channel.ExchangeDeclareAsync(exchange: "topic", type: ExchangeType.Topic);
        var queueName = (await channel.QueueDeclareAsync()).QueueName;
        await channel.QueueBindAsync(queueName, "topic", "abc.*");
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        await channel.BasicConsumeAsync(queueName, true, consumer);
        
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            ConsoleCol.WriteLine(
                $"[Abonent1] Received: {message}", 
                ConsoleColor.Yellow
            );
        };
        Console.WriteLine("Press [ENTER] to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}