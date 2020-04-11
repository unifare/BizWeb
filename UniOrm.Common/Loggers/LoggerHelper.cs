/*
 * ************
 * file:	    Logger.cs
 * creator:	    Cai Huan(huan.cai@philips.com)
 * date:	    2014-03
 * description:	Some help function to write log easily
 * ************
 */

using System;
using System.Text;

namespace UniOrm
{
    public static class LoggerHelper
    {
        public static string ByteArrayToHexString(byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder(data.Length*3);
            foreach (var b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }

        public static string GetExceptionString(Exception e)
        {
            if (null == e)
            {
                return "";
            }

            var sb = new StringBuilder();
            sb.Append("\r\n[Message]: \r\n\t").Append(e.Message).Append("\r\n");
            sb.Append("===================================\r\n");
            sb.Append("[ExceptionType]: \r\n\t").Append(e.GetType().ToString()).Append("\r\n\r\n");
            sb.Append("[Source]:  \r\n\t").Append(e.Source).Append("\r\n\r\n");
            sb.Append("[TargetSite]: \r\n\t").Append(e.TargetSite).Append("\r\n\r\n");
            sb.Append("[StackTrace]: \r\n").Append(e.StackTrace).Append("\r\n\r\n");
            if (null != e.InnerException)
            {
                sb.Append("[InnerException]: {\r\n").Append("\r\n");
                sb.Append(GetExceptionString(e.InnerException)).Append("\r\n}\r\n\r\n");
            }
            return sb.ToString();
        }

        public static string ToJsonFormat(object obj, params string[] properties)
        {
            if (null == obj)
            {
                return "null";
            }
            if (null == properties || 0 == properties.Length)
            {
                return "{ }";
            }

            var sb = new StringBuilder();
            sb.Append("{ ");

            var type = obj.GetType();
            foreach (var property in properties)
            {
                if (null == property)
                {
                    continue;
                }
                var pi = type.GetProperty(property);
                if (null == pi ||
                    (!pi.PropertyType.IsPrimitive && !pi.PropertyType.IsEnum &&
                     (pi.PropertyType != typeof(string) && pi.PropertyType != typeof(DateTime))))
                {
                    continue;
                }
                var value = pi.GetValue(obj, null).ToString();
                if (string.IsNullOrEmpty(value))
                {
                    value = "''";
                }
                sb.AppendFormat("{0} = {1}, ", property, value);
            }
            if (sb.Length > 2)
            {
                sb.Remove(sb.Length - 2, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}