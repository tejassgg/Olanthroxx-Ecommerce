using System;
using System.Text;

namespace WebAPI.Helpers
{
    public class EncryptDecrypt
    {
        readonly Constants constants = new Constants();

        public string Key = "%2rfst@90asdsn!pdohy9";

        public string Encrypt(string str, string type = "")
        {
            if (string.IsNullOrEmpty(type))
            {
                str += Key;
                var strBytes = Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(strBytes);
            }
            else if (type == constants.EncryptTypePass)
            {
                var strBytes = Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(strBytes);
            }
            return str;
        }

        public string Decrypt(string str, string type = "")
        {
            if (string.IsNullOrEmpty(type))
            {
                var encodedBytes = Convert.FromBase64String(str);
                var data = Encoding.UTF8.GetString(encodedBytes);
                str = data.Substring(0, data.Length - Key.Length);
                return str;
            }
            else if (type == constants.EncryptTypePass)
            {
                var encodedBytes = Convert.FromBase64String(str);
                var data = Encoding.UTF8.GetString(encodedBytes);
                return str;
            }
            return str;

        }
    }
}
