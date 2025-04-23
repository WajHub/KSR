namespace Komunikaty { 
    public interface IKomunikat { 
        public string Tekst1 { get; set; }
    }

    public interface IKomunikat2 
    {
        public string Tekst2 { get; set; }
    }

    public interface IKomunikat3 : IKomunikat, IKomunikat2;
}