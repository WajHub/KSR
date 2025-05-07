namespace Komunikaty {
    
    public interface IPubl { 
        public string Tekst1 { get; set; }
        public int number { get; set; }
    }

    public interface IPolecenie
    {
        public string instrukcja { get; set; }
    }

    public interface IOdp
    {
        public string kto { get; set; }
    }

    public interface IOdpA : IOdp
    {
        public string kto { get; set; }
    }
    
    public class OdpA : IOdpA
    {
        public string kto { get; set; }
    }
    
    public interface IOdpB: IOdp
    {
        public string kto { get; set; }
    }
    
    public class OdpB : IOdpB
    {
        public string kto { get; set; }
    }
    
}