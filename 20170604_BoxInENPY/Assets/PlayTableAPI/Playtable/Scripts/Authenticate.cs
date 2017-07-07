using Playtable;

namespace Playmove
{
    public class Authenticate
    {

        public bool IsValid()
        {
            return Authentication.TrueValidation;
        }

        public string WebServicePath
        {
            get
            {
                return Authentication.REQUEST_PATH;
            }
        }

        public int SopAuthentication(string ret)
        {
            return Authentication.Authenticate(ret);
        }

    }
}