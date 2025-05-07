using MassTransit;
using utils;
using Komunikaty;


class Wydawca 
{
    string[] messages = {"Test", "TestowaWiadomosc","Message", "messageExample", "TEST", "Message", "777", "Mess", "Test...", "Last" };
    public static bool exit = false;
    public static bool in_progress = true;

    static async Task Main(string[] args)
    {
        
        var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
            sbc.Host(new Uri("rabbitmq://localhost/"), h => 
                {
                    h.Username("guest");
                    h.Password("guest"); 
                }
            );
            sbc.ReceiveEndpoint("recvqueue-wydawca",
                ep =>
                {
                    ep.Handler<Komunikaty.IPolecenie>(Handle);
                });
        });
        await bus.StartAsync();

        ConsoleCol.WriteLine("Wydawca wystartowal", ConsoleColor.Cyan);


        Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Press enter to finish...");
            Console.ReadKey();
            exit = true;
        });
        var index = 0;
        while (!exit)
        {
            await Task.Delay(1000);
            if (in_progress)
            {
                IPubl komunikat = new Publ()
                {
                    Tekst1 = "Test",
                    number = index
                };
                await bus.Publish<IPubl>(komunikat);
            
                ConsoleCol.WriteLine($"[Nawdawca] - Wysłano wiadomość: Test ({index})", ConsoleColor.Cyan);  
                index++;
            }
        }
        await bus.StopAsync();

        return ;

        static Task Handle(ConsumeContext<Komunikaty.IPolecenie> ctx)
        {
            return Task.Run(() =>
            {
                ConsoleCol.WriteLine(
                    $"[Wydawca] - odebrano polecenie: {ctx.Message.instrukcja} ", ConsoleColor.Cyan);
                if (ctx.Message.instrukcja == "s")
                {
                    in_progress = true;
                }
                else if (ctx.Message.instrukcja == "t")
                {
                    in_progress = false;
                }
            });
        }
        
    }
}
