using MassTransit;
using Komunikaty;
using utils;

string[] messages = {"Test1(1)", "Test2(2)","Message1(3)", "message example(4)", "TEST-5(5)", "Message - 6(6)", "777(7)", "Mess-8(8)", "Test-9(9)", "Last(10)" };

var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
            {
                h.Username("guest");
                h.Password("guest"); 
            }
        );
});

await bus.StartAsync();

ConsoleCol.WriteLine("Nadawca wystartowal", ConsoleColor.Cyan);
var index = 0;
var r = new Random();
foreach (var mess in messages)
{
    var rInt = r.Next(500, 2000);
    ConsoleCol.WriteLine($"[Nawdawca] - Wysłano wiadomość: {mess}", ConsoleColor.Cyan);
    ConsoleCol.WriteLine($"- HEADER=[Job-count:{index}; Job-ms:{rInt}]", ConsoleColor.Cyan);
    await bus.Publish<Komunikat>(
        new Komunikat() { tekst = mess },
        ctx =>
        {
            ctx.Headers.Set("Job-Count", index); 
            ctx.Headers.Set("Job-ms", rInt);
        }
        );
    index++;
    // await Task.Delay(rInt);
}

Console.ReadKey();
await bus.StopAsync();
