using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Authorize
{
    public class UserPermissionDictionary
    {
        private static Dictionary<string, List<UserPermissionItem>> dictionary = new Dictionary<string, List<UserPermissionItem>>();
        public static void Add(string name, List<UserPermissionItem> permissionItems)
        {
            if (null == dictionary.GetValueOrDefault(name))
            {
                dictionary.Add(name, permissionItems);
            }
        }

        public static List<UserPermissionItem> Get(string name)
        {
            return dictionary.GetValueOrDefault(name, null);
        } 
    }
}
