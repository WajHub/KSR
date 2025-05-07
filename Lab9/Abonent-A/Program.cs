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
    sbc.ReceiveEndpoint("recvqueue-abonent-A",
        ep =>
        {
            ep.Handler<Komunikaty.IPubl>(Handle);
        }
    );
    sbc.ReceiveEndpoint("recvqueue-abonent-A_error",
        ep =>
        {
            ep.Handler<Fault<Komunikaty.IOdp>>(HandleFault);
        }
    );
});

await bus.StartAsync();

ConsoleCol.WriteLine("Abonent-A wystartowal", ConsoleColor.Blue);
Console.WriteLine("Press enter to finish...");
Console.ReadKey();

await bus.StopAsync();

return;

static Task Handle(ConsumeContext<Komunikaty.IPubl> ctx)
{
    return Task.Run(() =>
    {
        ConsoleCol.WriteLine($"[A-A] - odebrano wiadomość: {ctx.Message.Tekst1} {ctx.Message.number}", ConsoleColor.Blue);
        if (ctx.Message.number % 2 == 0)
        {
            ConsoleCol.WriteLine($"[A-A] - Wysyłam odpowiedz do W", ConsoleColor.Blue);
            ctx.RespondAsync<Komunikaty.IOdpA>(new OdpA()
            {
                kto = "A - A"
            });
        }

    });
}

static Task HandleFault(ConsumeContext<Fault<Komunikaty.IOdp>> ctx)
{
    return Task.Run(() =>
    {
        foreach (var e in ctx.Message.Exceptions) 
            ConsoleCol.WriteLine($"[A-A] - błąd od: {ctx.Message} ", ConsoleColor.Red);
    });
}