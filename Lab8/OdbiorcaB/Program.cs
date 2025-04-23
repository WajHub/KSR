using Komunikaty;
using MassTransit;
using utils;

var handler = new Handler();

var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.ReceiveEndpoint("recvqueue-Odbiorca-B",
        ep =>
        {
            ep.Instance(handler);
        });
});

await bus.StartAsync();

ConsoleCol.WriteLine("Odbiorca-B wystartowal", ConsoleColor.Magenta);
Console.ReadKey();

await bus.StopAsync();

class Handler: IConsumer<IKomunikat3>
{
    private int _counterRecivedMessages = 0;
    
    public Task Consume(ConsumeContext<Komunikaty.IKomunikat3> ctx)
    {
        return Task.Run(() =>
        {
            ConsoleCol.WriteLine(
                $"[Odbiorca-B] - odebrano wiadomość: Tekst1={ctx.Message.Tekst1} Tekst2={ctx.Message.Tekst2}", ConsoleColor.Magenta);
            foreach (var hdr in ctx.Headers.GetAll())
            {
                ConsoleCol.WriteLine($"- HEADER=[{hdr.Key}: {hdr.Value}]", ConsoleColor.Magenta);
            } 
            ConsoleCol.WriteLine($"Liczba odebranych wiadomości: {++_counterRecivedMessages} \n", ConsoleColor.Magenta);
        });
    }
}