using MassTransit;
using MassTransit.Serialization;
using utils;
using Komunikaty;

var bus = Bus.Factory.CreateUsingRabbitMq(sbc => {
    sbc.Host(new Uri("rabbitmq://localhost/"), h => 
        {
            h.Username("guest");
            h.Password("guest"); 
        }
    );
    sbc.UseEncryptedSerializer(new AesCryptoStreamProvider(
        new SymmetricKeyProvider("19351119351119351119351119351119"), "1935111935111935"));
});
await bus.StartAsync();

ConsoleCol.WriteLine("Kontroler wystartowal", ConsoleColor.DarkYellow);

bool exit = false;
Console.WriteLine("Press 'q' to exit program \nPress 't' or 's' to send instruction\n");

while (!exit)
{
    var key = Console.ReadKey().Key;
    if (key == ConsoleKey.Q) exit = true;
    else
    {
        string instrukcja = (key == ConsoleKey.S ? "s" : "t");
        ConsoleCol.WriteLine($"Kontroler wysyła instrukcje {instrukcja}", ConsoleColor.DarkYellow);
        var tsk = bus.GetSendEndpoint(new Uri("rabbitmq://localhost/recvqueue-instructions-wydawca"));
        tsk.Wait(); var sendEp = tsk.Result;
        await sendEp.Send<Komunikaty.IPolecenie>(new Polecenie()
        {
            instrukcja = instrukcja
        },ctx =>
        {
            ctx.Headers.Set(EncryptedMessageSerializer.EncryptionKeyHeader, Guid.NewGuid().ToString());
        });

    }
}


await bus.StopAsync();