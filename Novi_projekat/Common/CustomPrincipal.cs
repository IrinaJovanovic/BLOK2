using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity identity;
        private Dictionary<string, string[]> roles = new Dictionary<string, string[]>();//uloge sa permisijama se spajaju

        public CustomPrincipal(WindowsIdentity winIdentity)//ovde je problem negde
        {
            /// define list of roles based on custom roles 			 
            string[] rolesTypes = Enum.GetNames(typeof(Role));
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                string groupName = Formatter.ParseName(name.ToString());

                
            foreach (string g in Enum.GetNames(typeof(Role)))
                {
                    if (g.ToString().Equals(groupName))
                    {
                        if (!roles.ContainsKey(groupName))
                        {
                            
                            roles.Add(groupName,RolesConfig.GetPermissions(g.ToString()));
                            break;
                        }
                    }
                }
            }

        }


        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            bool isAuthorized = false;
            foreach (string[] r in roles.Values)
            {
                if (r.Contains(role))
                {
                    isAuthorized = true;
                    break;
                }
            }
            return isAuthorized;
        }

        public void Dispose()
        {
            if (identity != null)
            {
                identity.Dispose();
                identity = null;
            }
        }
    }
}
