using MassTransit;
using utils;

var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.ReceiveEndpoint("recvqueue",
        ep =>
        {
            ep.Handler<Komunikaty.Komunikat>(Handle);
        });
});

await bus.StartAsync();

ConsoleCol.WriteLine("Odbiorca wystartowal", ConsoleColor.Blue);
Console.ReadKey();

await bus.StopAsync(); 


return;

static Task Handle(ConsumeContext<Komunikaty.Komunikat> ctx)
{
    return Task.Run(() =>
    {
        foreach (var hdr in ctx.Headers.GetAll())
        {
            ConsoleCol.WriteLine($"- HEADER=[{hdr.Key}: {hdr.Value}]", ConsoleColor.Blue);
        } 
        ConsoleCol.WriteLine($"[Odbiorca 1] - odebrano wiadomość: {ctx.Message.tekst}", ConsoleColor.Blue);
    });
}