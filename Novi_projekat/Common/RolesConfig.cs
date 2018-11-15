using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Role
    {
        Admin =0,
        Writer=1,
        Reader=2,
    }
    public enum Permissions
    {
        Create=0,
        Arhiving=1,
        Deleting=2,
        Modify=3,
        Writting=4,
        ReadingCityAvgConsumption=5,
        ReadingRegionAvgConsumption=6,
        ReadingMaxAvgConsumption=7,
    }

    class RolesConfig
    {
        static string[] AdminPermissions = new string[] {  Permissions.Create.ToString(), Permissions.Deleting.ToString(),Permissions.Arhiving.ToString() };
        static string[] WriterPermissions = new string[] {Permissions.Writting.ToString(), Permissions.Modify.ToString() };
        static string[] ReaderPermissions = new string[] {Permissions.ReadingCityAvgConsumption.ToString(), Permissions.ReadingRegionAvgConsumption.ToString(),Permissions.ReadingMaxAvgConsumption.ToString() };
        static string[] listPermissions = new string[] { };
        static string[] Empty = new string[] { Permissions.Create.ToString(), Permissions.Deleting.ToString(),
            Permissions.Arhiving.ToString(), Permissions.Writting.ToString(),
            Permissions.Modify.ToString(), Permissions.ReadingCityAvgConsumption.ToString(),
            Permissions.ReadingRegionAvgConsumption.ToString(), Permissions.ReadingMaxAvgConsumption.ToString() };
        public static string[] GetPermissions(string role)
        {
            switch (role)
            {
                case "Admin": return AdminPermissions;
                case "Reader": return ReaderPermissions;
                case "Writer": return WriterPermissions;
                default: return Empty;
            }
        }
    }
}
