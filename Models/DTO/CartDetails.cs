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
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Last Updated Date")]
        public DateTime? ModifiedDate { get; set; }
        public List<OrderDetails> lstOrderDetails { get; set; }
    }
}
