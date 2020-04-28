/*
 * ************************************
 * file:	    Uni.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/28 10:15:07
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace UniOrm.Common
{
    public class Uni : DynamicObject
    {  
        // Inner Dictionary
        public   IDictionary<string, object> Dictionary = new Dictionary<string, object>();

        /// <summary>
        /// If you try to get a value of Property not defined in class, this method is called
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();

            return Dictionary.TryGetValue(name, out result);
        }

        /// <summary>
        /// If you try to set a value of a property taht is not defined in the class, this method is called
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Dictionary[binder.Name.ToLower()] = value;
            return true;
        }

        /// <summary>
        /// Try to set a method not defined in the class, this method is called
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = Dictionary[binder.Name.ToLower()];
            result = method(args);
            return result != null;
        }
    }
}
