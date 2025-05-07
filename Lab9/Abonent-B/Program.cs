using Komunikaty;
using MassTransit;
using utils;


var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.ReceiveEndpoint("recvqueue-abonent-B",
        ep =>
        {
            ep.Handler<Komunikaty.IPubl>(Handle);
        });
    sbc.ReceiveEndpoint("recvqueue-abonent-B_error",
        ep =>
        {
            ep.Handler<Fault<Komunikaty.IOdp>>(HandleFault);
        }
    );
});

await bus.StartAsync();

ConsoleCol.WriteLine("Abonent-B wystartowal", ConsoleColor.DarkBlue);
Console.WriteLine("Press enter to finish...");
Console.ReadKey();

await bus.StopAsync();

return;

static Task Handle(ConsumeContext<Komunikaty.IPubl> ctx)
{
    return Task.Run(() =>
    {
        ConsoleCol.WriteLine(
            $"[A-B] - odebrano wiadomość: {ctx.Message.Tekst1} {ctx.Message.number}", ConsoleColor.DarkBlue);
        if (ctx.Message.number % 3 == 0)
        {
            ConsoleCol.WriteLine($"[A-A] - Wysyłam odpowiedz do W", ConsoleColor.DarkBlue);
            ctx.RespondAsync<Komunikaty.IOdpB>(new OdpB()
            {
                kto = "A - B"
            });
        }

    });
}

static Task HandleFault(ConsumeContext<Fault<Komunikaty.IOdp>> ctx)
{
    return Task.Run(() =>
    {
        foreach (var e in ctx.Message.Exceptions) 
            ConsoleCol.WriteLine($"[A-B] - błąd od: {ctx.Message} ", ConsoleColor.Red);
    });
}