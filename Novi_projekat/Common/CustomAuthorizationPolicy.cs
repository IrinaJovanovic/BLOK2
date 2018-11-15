using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Common
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private string id;
        private object locker = new object();

        public CustomAuthorizationPolicy()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public ClaimSet Issuer
        {
            get
            {
                return ClaimSet.System;
            }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object list;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }

            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0]);
            return true;
        }


        public virtual IPrincipal GetPrincipal(IIdentity identity)
        {
            lock (locker)
            {
                IPrincipal principal = null;
                WindowsIdentity winIdentity = identity as WindowsIdentity;

                //string idenName = identity.Name;
                //string[] idenNameSplit = idenName.Split(',');
                //string[] cn = idenNameSplit[0].Split('=');
                //string username = cn[1];
                //string[] ou = idenNameSplit[1].Split('=');
                //string group = ou[1];

                if (identity != null)//ovde je bilo !=winIdentity
                {
                    principal = new CustomPrincipal(winIdentity);
                }
                return principal;
            }
        }

    }
}
