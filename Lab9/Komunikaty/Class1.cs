namespace Komunikaty {
    
    public interface IPubl { 
        public string Tekst1 { get; set; }
        public int number { get; set; }
    }

    public interface IPolecenie
    {
        public string instrukcja { get; set; }
    }
    
}