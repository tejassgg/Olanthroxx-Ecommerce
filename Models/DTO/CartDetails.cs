using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class CartDetails
    {
        public Guid OrderID { get; set; }
        [DisplayName("Total Amount")]
        public decimal TotalAmount { get; set; }
        [DisplayName("Placed On")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Last Updated")]
        public DateTime? ModifiedDate { get; set; }
        public int TotalQuantity { get; set; }
        public List<OrderDetails> lstOrderDetails { get; set; }
        public string OrderStatus { get; set; }
        public string LoggedInUser { get; set; }
    }

    public class TempCartDetails
    {
        public int CartID { get; set; }

        public int AccountID_FK { get; set; }

        public string TempCartDetailsString { get; set; }

        public string SessionID { get; set; }

        public bool IsUsed { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public Nullable<System.DateTime> UsedDate { get; set; }
    }
}
