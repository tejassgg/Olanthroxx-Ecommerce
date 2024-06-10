using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class EmailDetails
    {
        public string EmailID { get; set; }
        public string LoginType { get; set; }
        public string UserName { get; set; }
        public string EmailType { get; set; }
        public string OTP { get; set; }
        public string OrderID { get; set; }
        public int NoOfItems { get; set; }
    }
}
