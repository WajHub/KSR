using MassTransit;
using utils;
using Komunikaty;


class Wydawca 
{
    public static bool exit = false;
    public static bool in_progress = true;
    public static IBusControl bus;
    public static int responseTryCount = 0;
    static async Task Main(string[] args)
    {
        
        bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
            sbc.Host(new Uri("rabbitmq://localhost/"), h => 
                {
                    h.Username("guest");
                    h.Password("guest"); 
                }
            );
            sbc.ReceiveEndpoint("recvqueue-wydawca",
                ep =>
                {
                    ep.Handler<Komunikaty.IPolecenie>(HandleInstruction);
                    ep.Handler<Komunikaty.IOdp>(HandleResponse);
                    ep.UseMessageRetry(r => { r.Immediate(5); });
                });
            sbc.ReceiveEndpoint("recvqueue-wydawca_error",
                ep =>
                {
                    ep.Handler<Fault<Komunikaty.IOdp>>(HandleFault);
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
        
        var index = 1;
        while (!exit)
        {
            await Task.Delay(5000);
            if (in_progress)
            {
                IPubl komunikat = new Publ()
                {
                    Tekst1 = "Test",
                    number = index
                };
                await bus.Publish<IPubl>(komunikat);
            
                ConsoleCol.WriteLine($"[W] - Wysłano wiadomość: Test ({index})", ConsoleColor.Cyan);  
                index++;
            }
        }
        await bus.StopAsync();

        return ;

        static Task HandleInstruction(ConsumeContext<Komunikaty.IPolecenie> ctx)
        {
            return Task.Run(() =>
            {
                ConsoleCol.WriteLine(
                    $"[W] - odebrano polecenie: {ctx.Message.instrukcja} ", ConsoleColor.Cyan);
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
        
        static Task HandleResponse(ConsumeContext<Komunikaty.IOdp> ctx)
        {
            return Task.Run(() =>
            {
                responseTryCount++;
                ConsoleCol.WriteLine(
                    $"[W] - próba nr {responseTryCount} dla odpowiedzi od: {ctx.Message.kto}", 
                    ConsoleColor.Yellow);

                Random r = new Random();
                int rInt = r.Next(0, 100);
                if (rInt < 30) throw new Exception();
                responseTryCount = 0;
                ConsoleCol.WriteLine(
                    $"[W] - odebrano odpowiedz od: {ctx.Message.kto} ", ConsoleColor.Cyan);
            });
        }
        
        static Task HandleFault(ConsumeContext<Fault<Komunikaty.IOdp>> ctx)
        {
            return Task.Run(() =>
            {
                foreach (var e in ctx.Message.Exceptions)
                {
                    ConsoleCol.WriteLine(
                        $"[W] - odebrano błąd ({e.Message}) od: {ctx.Message.Message.kto} ", ConsoleColor.Red);
                }
            });
        }

    }
}
