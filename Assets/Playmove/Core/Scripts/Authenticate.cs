using Playmove.SopService;

namespace Playmove.Core
{
    public class Authenticate
    {
        public bool IsValid()
        {
            return Authentication.TrueValidation;
        }

        public int SopAuthentication(string ret)
        {
            return Authentication.Authenticate(ret);
        }
    }
}