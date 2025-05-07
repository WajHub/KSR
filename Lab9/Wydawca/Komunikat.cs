using Komunikaty;

public class Publ : IPubl
{
    public string Tekst1 { get; set; }
    public int number { get; set; }
}

public class Polecenie : IPolecenie
{
    public string instrukcja { get; set; }
}
