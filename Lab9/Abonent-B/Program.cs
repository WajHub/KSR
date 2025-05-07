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
            $"[Abonent-B] - odebrano wiadomość: {ctx.Message.Tekst1} {ctx.Message.number}", ConsoleColor.DarkBlue);
        if (ctx.Message.number % 3 == 0)
        {
            ctx.RespondAsync<Komunikaty.IOdpB>(new OdpB()
            {
                kto = "A - B"
            });
        }

    });
}