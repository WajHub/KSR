using RabbitMQ.Client;
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
        var index = 0;
        foreach (var message in messages)
        {
            var routingKey = index % 2 == 0 ? "abc.def" : "abc.xyz";
            var body = Encoding.UTF8.GetBytes(message+$" {index} - {routingKey}");
            await channel.BasicPublishAsync(
                exchange: "topic", 
                routingKey:routingKey, 
                body: body);
            index++;
            ConsoleCol.WriteLine(
                $"[Wydawca] Sent: message {index} - {routingKey}", 
                ConsoleColor.Blue
            );
        }
        
        Console.WriteLine("Press [ENTER] to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
