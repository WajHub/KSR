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
        });
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
        ConsoleCol.WriteLine(
            $"[A-A] - odebrano wiadomość: {ctx.Message.Tekst1} {ctx.Message.number}", ConsoleColor.Blue);
        if (ctx.Message.number % 2 == 0)
        {
            ctx.RespondAsync<Komunikaty.IOdpA>(new OdpA()
            {
                kto = "A - A"
            });
        }

    });
}