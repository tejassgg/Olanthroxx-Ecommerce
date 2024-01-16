using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class Address
    {
        public int AddressID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserID { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int Pincode { get; set; }
        public string FullAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Landmark { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
