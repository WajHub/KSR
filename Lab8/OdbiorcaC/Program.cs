using MassTransit;
using utils;


var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.ReceiveEndpoint("recvqueue-Odbiorca-C",
        ep =>
        {
            ep.Handler<Komunikaty.IKomunikat2>(Handle);
        });
});

await bus.StartAsync();

ConsoleCol.WriteLine("Odbiorca-C wystartowal", ConsoleColor.White);
Console.ReadKey();

await bus.StopAsync();

return;

static Task Handle(ConsumeContext<Komunikaty.IKomunikat2> ctx)
{
    return Task.Run(() =>
    {
        ConsoleCol.WriteLine(
            $"[Odbiorca-C] - odebrano wiadomość: {ctx.Message.Tekst2} ", ConsoleColor.White);
        foreach (var hdr in ctx.Headers.GetAll())
        {
            ConsoleCol.WriteLine($"- HEADER=[{hdr.Key}: {hdr.Value}]", ConsoleColor.White);
        } 
        ConsoleCol.WriteLine(
            "\n", ConsoleColor.White);
    });
}