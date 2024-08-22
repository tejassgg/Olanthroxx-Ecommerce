using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class OrderDetails
    {
        public System.Guid OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public string UserID { get; set; }
        [DisplayName("Order Date")]
        public System.DateTime OrderDate { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Order Status")]
        public string OrderStatus { get; set; }
        [DisplayName("Last Updated Date")]
        public System.DateTime? ModifiedDate { get; set; }
        public string ImgPath { get; set; }
        public bool IsReviewed { get; set; }
        public int? ShippingAddressID { get; set; }
        public int? BilllingAddressID { get; set; }
        public string SoldBy { get; set; }
    }

    public class LstOrderDetailsForSeller 
    {
        public UserDetails UserDetails { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public int TotalSales {get; set; }
        public int TotalSalesQuantity {get; set; }  
        public int TotalSalesAmount { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveUsers { get; set; }
    }

}
