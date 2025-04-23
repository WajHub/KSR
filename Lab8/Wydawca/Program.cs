using MassTransit;
using Komunikaty;
using utils;
using Wydawca;

string[] messages = {"Test", "TestowaWiadomosc","Message", "messageExample", "TEST", "Message", "777", "Mess", "Test...", "Last" };

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
var index = 1;
var r = new Random();
foreach (var mess in messages)
{
    var rInt = r.Next(500, 2000);
    ConsoleCol.WriteLine($"[Nawdawca] - Wysłano wiadomość: {mess}", ConsoleColor.Cyan);
    ConsoleCol.WriteLine($"- HEADER=[Job-count:{index}; Job-ms:{rInt}]", ConsoleColor.Cyan);
    var typWiadomosci = (index % 3==0) ? 3 : index % 3;
    ConsoleCol.WriteLine($"- Typ wiadomości {typWiadomosci} \n", ConsoleColor.Cyan);
    if (typWiadomosci== 1)
    {
        IKomunikat komunikat = new Komunikat() { Tekst1 = mess };
        await bus.Publish<IKomunikat>(
            komunikat,
            ctx =>
            {
                ctx.Headers.Set("Job-Count", index); 
                ctx.Headers.Set("Job-ms", rInt);
            }
        );
    }
    if (typWiadomosci == 2)
    {
        IKomunikat2 komunikat = new Komunikat2() { Tekst2 = mess };
        await bus.Publish<IKomunikat2>(
            komunikat,
            ctx =>
            {
                ctx.Headers.Set("Job-Count", index); 
                ctx.Headers.Set("Job-ms", rInt);
            }
        );
    }
    if (typWiadomosci == 3)
    {
        IKomunikat3 komunikat = new Komunikat3() { Tekst1 = mess+"_1", Tekst2 = mess+"_2"};
        await bus.Publish<IKomunikat3>(
            komunikat,
            ctx =>
            {
                ctx.Headers.Set("Job-Count", index); 
                ctx.Headers.Set("Job-ms", rInt);
            }
        );
    }
    
    index++;
    await Task.Delay(rInt);
}

Console.ReadKey();
await bus.StopAsync();
