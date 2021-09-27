using Newtonsoft.Json;

namespace SYMB2C.Feature.Login.Models.Identity
{
    public class EmailInputClaimsModel
    {
        public string email { get; set; }

        public string code { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static EmailInputClaimsModel Parse(string JSON)
        {
            return JsonConvert.DeserializeObject(JSON, typeof(EmailInputClaimsModel)) as EmailInputClaimsModel;
        }
    }
}
