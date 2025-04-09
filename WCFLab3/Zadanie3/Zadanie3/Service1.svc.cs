using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Xml;

namespace Zadanie3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.


    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet(UriTemplate = "index.html")]
        [XmlSerializerFormat]
        XmlDocument Index();

        [OperationContract]
        [WebGet(UriTemplate = "scripts.js")]
        Stream Script();

        [OperationContract]
        [WebInvoke(UriTemplate = "Dodaj/{a}/{b}")]
        int Dodaj(string a, string b);
    }

    public class Service1 : IService
    {
        public int Dodaj(string a, string b)
        {
            return int.Parse(a) + int.Parse(b);
        }

        public XmlDocument Index()
        {
            var xml = new XmlDocument();
            xml.Load("D:\\Semestr6\\KSR\\WCFLab3\\Zadanie3\\Zadanie3\\index.xhtml");

            return xml;
        }

        public Stream Script()
        {

            return File.OpenRead("D:\\Semestr6\\KSR\\WCFLab3\\Zadanie3\\Zadanie3\\scripts.js");
        }
    }
}
