using RabbitMQ.Client;
using utils;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    UserName = "guest",
    Password = "guest",
    HostName = "localhost",
};

string[] messages = {"Test1(1)", "Test2(2)","Message1(3)", "message example(4)", "TEST-5(5)", "Message - 6(6)", "777(7)", "Mess-8(8)", "Test-9(9)", "Last(10)" };
var r = new Random();

try
{
    await using (var connection = await factory.CreateConnectionAsync())
    await using (var channel = await connection.CreateChannelAsync())
    {
        await channel.QueueDeclareAsync("message-queue", false, false, false, null);
        var replyQueueName = (await channel.QueueDeclareAsync()).QueueName;
        var consumerReply = new AsyncEventingBasicConsumer(channel);
        await channel.BasicConsumeAsync(replyQueueName, true, consumerReply);
        
        consumerReply.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            ConsoleCol.Write(
                $"[Nadawca] Received Reply:", 
                ConsoleColor.Blue
            );
            ConsoleCol.Write(
                $"{message}", 
                ConsoleColor.Green
            );
        };
        
        var index = 0;
        foreach (var mess in messages)
        {
            var rInt = r.Next(1500, 3000);
            var body = Encoding.UTF8.GetBytes(mess);
            
            var props = new BasicProperties();
            props.Headers =  new Dictionary<string, object>();
            props.Headers.Add("Job-counter", index + 1);
            props.Headers.Add("Job-ms", rInt);
            props.ReplyTo = replyQueueName;
            var corrId = Guid.NewGuid().ToString();
            props.CorrelationId = corrId;
            
            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "message-queue",
                mandatory: false,
                basicProperties: props,
                body: body
            );
            ConsoleCol.WriteLine($"[Nadawca] Sent: {mess}", ConsoleColor.Blue);
            
            // await Task.Delay(rInt);
            index++;
        }
        

        Console.WriteLine("Press [ENTER] to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
