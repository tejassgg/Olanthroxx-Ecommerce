using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class ProductDetails
    {
        public int ProductID { get; set; }
        public string PName { get; set; }
        public string PTitle { get; set; }
        public string PDescription { get; set; }
        public string Category { get; set; }
        public int CategoryID_FK { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Last Updated Date")]
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string SellerName { get; set; }
        public string ImgPath { get; set; }
        public double OverallRating { get; set; }
        public List<ProductReview> lstProductReviews { get; set; }
        public List<ProductDetails> LstOtherProducts { get; set; }
    }

    public class ProductReview
    {
        public string PName { get; set; }
        public int ProductID { get; set; }
        public System.Guid OrderID { get; set; }
        public int Rating { get; set; }
        [DisplayName("Write You Review")]
        public string Review { get; set; }
        public string ImgPath { get; set; }
        public string UserID { get; set; }
        public string ReviewerName { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
