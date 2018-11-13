using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity identity;
        private Dictionary<string, string[]> roles = new Dictionary<string, string[]>();//uloge sa permisijama se spajaju

        public CustomPrincipal(string group)
        {
            /// define list of roles based on custom roles 			 
            string[] rolesTypes = Enum.GetNames(typeof(Role));

            foreach (string role in rolesTypes)
            {
                if (role.Equals(group))
                {
                    if (!roles.ContainsKey(role))
                    {
                        roles.Add(group, RolesConfig.GetPermissions(group));
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
