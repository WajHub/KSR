using MassTransit;
using utils;


var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.ReceiveEndpoint("recvqueue-odbiorca-A",
        ep =>
        {
            ep.Handler<Komunikaty.IKomunikat>(Handle);
        });
});

await bus.StartAsync();

ConsoleCol.WriteLine("Odbiorca-A wystartowal", ConsoleColor.Blue);
Console.ReadKey();

await bus.StopAsync();

return;

static Task Handle(ConsumeContext<Komunikaty.IKomunikat> ctx)
{
    return Task.Run(() =>
    {
        ConsoleCol.WriteLine(
            $"[Odbiorca-A] - odebrano wiadomość: {ctx.Message.Tekst1} ", ConsoleColor.Blue);
        foreach (var hdr in ctx.Headers.GetAll())
        {
            ConsoleCol.WriteLine($"- HEADER=[{hdr.Key}: {hdr.Value}]", ConsoleColor.Blue);
        } 
        ConsoleCol.WriteLine(
            $"\n", ConsoleColor.Blue);
    });
}