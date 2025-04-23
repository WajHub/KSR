using Komunikaty;

namespace Wydawca;

public class Komunikat : IKomunikat
{
    public string Tekst1 { get; set; }
}

public class Komunikat2 : IKomunikat2
{
    public string Tekst2 { get; set; }
}

public class Komunikat3 : IKomunikat3
{
    public string Tekst1 { get; set; }
    public string Tekst2 { get; set; }
}