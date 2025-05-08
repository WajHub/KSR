using MassTransit;
using utils;
using Komunikaty;
using MassTransit.Serialization;

class Wydawca 
{
    private static bool _exit = false;
    private static bool _in_progress = true;
    private static IBusControl _bus_abonent;
    private static IBusControl _bus_controller;
    private static int _responseTryCount = 0;
    private static int _index = 1;

    private static Dictionary<string, int> _statistisc_attempts = new Dictionary<string, int>
    {
        { "type-a", 0 },
        { "type-b", 0 },
        { "controller", 0 }
    };
    private static Dictionary<string, int> _statistisc_success = new Dictionary<string, int>
    {
        { "type-a", 0 },
        { "type-b", 0 },
        { "controller", 0 }
    };
    static async Task Main(string[] args)
    {
        _bus_abonent = Bus.Factory.CreateUsingRabbitMq(sbc => {
            sbc.Host(new Uri("rabbitmq://localhost/"), h => 
                {
                    h.Username("guest");
                    h.Password("guest"); 
                }
            );
            sbc.ReceiveEndpoint("recvqueue-wydawca",
                ep =>
                {
                    ep.UseMessageRetry(r => { r.Immediate(3); });
                    ep.Handler<Komunikaty.IOdp>(HandleResponse);
                });
        });
        
        
        _bus_controller = Bus.Factory.CreateUsingRabbitMq(sbc => {
            sbc.Host(new Uri("rabbitmq://localhost/"), h => 
                {
                    h.Username("guest");
                    h.Password("guest"); 
                }
            );
            sbc.UseEncryptedSerializer(new AesCryptoStreamProvider(
                new SymmetricKeyProvider("19351119351119351119351119351119"), "1935111935111935"));
            sbc.ReceiveEndpoint("recvqueue-instructions-wydawca",
                ep =>
                {
                    ep.Handler<Komunikaty.IPolecenie>(HandleInstruction);
                });
        });
        
        await _bus_abonent.StartAsync();
        await _bus_controller.StartAsync();
        ConsoleCol.WriteLine("Wydawca wystartowal", ConsoleColor.Cyan);


        Task.Factory.StartNew(() =>
        {
            Console.WriteLine("Press enter to finish...");
            while (!_exit)
            {
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.S)
                {
                
                    ConsoleCol.WriteLine("\nStatistics:", ConsoleColor.Cyan);
                    ConsoleCol.WriteLine($"Attempts to A: {_statistisc_attempts["type-a"]}", ConsoleColor.Cyan);
                    ConsoleCol.WriteLine($"Attempts to B: {_statistisc_attempts["type-b"]}", ConsoleColor.Cyan);
                    ConsoleCol.WriteLine($"Success to A: {_statistisc_success["type-a"]}", ConsoleColor.Cyan);
                    ConsoleCol.WriteLine($"Success to B: {_statistisc_success["type-b"]}", ConsoleColor.Cyan);
                    ConsoleCol.WriteLine($"Sent messages: {_index}\n", ConsoleColor.Cyan);
                }
                else _exit = true;
            }
        });
        

        while (!_exit)
        {
            await Task.Delay(1000);
            if (_in_progress)
            {
                IPubl komunikat = new Publ()
                {
                    Tekst1 = "Test",
                    number = _index
                };
                await _bus_abonent.Publish<IPubl>(komunikat);

                ConsoleCol.WriteLine($"[W] - Wysłano wiadomość: Test ({_index})", ConsoleColor.Cyan);
                _index++;
            }
        }
        await _bus_abonent.StopAsync();
        await _bus_controller.StopAsync();
        
        return ;

        static Task HandleInstruction(ConsumeContext<Komunikaty.IPolecenie> ctx)
        {
            _statistisc_attempts["controller"]++;
            return Task.Run(() =>
            {
                ConsoleCol.WriteLine($"[W] - odebrano polecenie: {ctx.Message.instrukcja} ", ConsoleColor.Cyan);
                if (ctx.Message.instrukcja == "s") _in_progress = true;
                else if (ctx.Message.instrukcja == "t") _in_progress = false;
                _statistisc_success["controller"]++;
            });
        }
        
        static Task HandleResponse(ConsumeContext<Komunikaty.IOdp> ctx)
        {
            Random r = new Random();
            return Task.Run(() =>
            {
                ConsoleCol.WriteLine($"[W] - odebrano odpowiedz od: {ctx.Message.kto} ", ConsoleColor.Cyan);

                if (ctx.Message.kto == "A - A") _statistisc_attempts["type-a"]++;
                else if (ctx.Message.kto == "A - B") _statistisc_attempts["type-b"]++;
                
                int rInt = r.Next(0, 100);
                if (rInt < 60) throw new Exception();
                
                if (ctx.Message.kto == "A - A") _statistisc_success["type-a"]++;
                else if (ctx.Message.kto == "A - B") _statistisc_success["type-b"]++;
            });
        }
        
    }
}
