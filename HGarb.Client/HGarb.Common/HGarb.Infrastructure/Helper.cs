using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace HGarb.Infrastructure
{
    public class Helper
    {
        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }

            return string.Empty;
        }

        public static string GetDBValue(object value)
        {
            if (value != null && value != DBNull.Value)
            {
                return Convert.ToString(value);
            }

            return string.Empty;
        }

        public static string GetString(object value)
        {
            try
            {
                if (value != null) return Convert.ToString(value);
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int ToInt(object value)
        {
            if (value != null && value != DBNull.Value)
            {
                int retVal = 0;
                int.TryParse(Convert.ToString(value), out retVal);
                return retVal;
            }

            return 0;
        }

        public static bool ToBool(object value)
        {
            if (value != null && value != DBNull.Value)
            {
                bool retVal = false;
                bool.TryParse(Convert.ToString(value), out retVal);
                return retVal;
            }

            return false;
        }

        public static string Serialize(object value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T Deserialize<T>(string value)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
