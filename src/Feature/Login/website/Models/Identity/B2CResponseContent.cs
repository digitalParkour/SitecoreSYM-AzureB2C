using System.Net;
using System.Reflection;

namespace SYMB2C.Feature.Login.Models.Identity
{
    public class B2CResponseContent
    {
        public string version { get; set; }

        public int status { get; set; }

        public string userMessage { get; set; }

        public B2CResponseContent(string message, HttpStatusCode status)
        {
            this.userMessage = message;
            this.status = (int)status;
            this.version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}