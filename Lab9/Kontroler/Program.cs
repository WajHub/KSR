using MassTransit;
using utils;
using Komunikaty;

var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
});
await bus.StartAsync();

ConsoleCol.WriteLine("Kontroler wystartowal", ConsoleColor.DarkYellow);

bool exit = false;
Console.WriteLine("Press 'q' to exit program \nPress 't' or 's' to send instruction\n");

while (!exit)
{
    var key = Console.ReadKey().Key;
    if (key == ConsoleKey.Q) exit = true;
    else if (key == ConsoleKey.S)
    {
        ConsoleCol.WriteLine("Kontroler wysyła instrukcje 's'", ConsoleColor.DarkYellow);
        var tsk = bus.GetSendEndpoint(new Uri("rabbitmq://localhost/recvqueue-wydawca"));
        tsk.Wait(); var sendEp = tsk.Result;
        await sendEp.Send<Komunikaty.IPolecenie>(new Polecenie()
        {
            instrukcja = "s"
        });
    }
    else if (key == ConsoleKey.T)
    {
        ConsoleCol.WriteLine("Kontroler wysyła instrukcje 't'", ConsoleColor.DarkYellow);
        var tsk = bus.GetSendEndpoint(new Uri("rabbitmq://localhost/recvqueue-wydawca"));
        tsk.Wait(); var sendEp = tsk.Result;
        await sendEp.Send<Komunikaty.IPolecenie>(new Polecenie()
        {
            instrukcja = "t"
        });
    }
}


await bus.StopAsync();