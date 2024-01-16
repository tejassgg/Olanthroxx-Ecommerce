using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Models.DTO;

namespace WebAPI.Helpers
{
    public class CommonMethods
    {
        public string GenerateSalt()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public string GenerateTransactionNo(string paymentMode)
        {
            Random rnd = new Random();
            int num = rnd.Next();
            string txnNo = paymentMode.Substring(0, 1).ToUpper() + DateTime.Now.ToShortDateString().Replace("-", "") + num;

            return txnNo.Substring(0, 15);
        }
    }
}
