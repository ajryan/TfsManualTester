using System.Web.Script.Serialization;

namespace TfsManualTester.Web.Authorization
{
    public class TfsUserData
    {
        public string TfsUrl { get; set; }
        public string Password { get; set; }

        public static TfsUserData DeSerialize(string tfsUserDataJson)
        {
            return new JavaScriptSerializer().Deserialize<TfsUserData>(tfsUserDataJson);
        }

        public string Serialize()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}