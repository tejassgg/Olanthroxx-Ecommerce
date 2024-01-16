using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO
{
    public class PaymentDetail
    {
        public int PaymentID { get; set; }
        public System.Guid OrderID { get; set; }
        public string UserName { get; set; }
        
        public int? ShippingAddressID { get; set; }
        public int? BillingAddressID { get; set; }

        [DisplayName("Amount (in Rs.)")]
        public int TotalPayableAmount { get; set; }
        public string TransactionNo { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public CartDetails CartDetails { get; set; }
        public string HdnCartDetails { get; set; }
        public PaymentModeDetails PaymentModeDetails { get; set; }
        public List<Address> LstAddress { get; set; }

        public string PaymentOf { get; set; }
        public MovieBooking movieBooking { get; set; }
        public string HdnMovieBooking { get; set; }
    }

    public class PaymentModeDetails
    {
        [DisplayName("Payment Mode")]
        public string PaymentMode { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentDetails { get; set; }

        [DisplayName("UPI ID or TPA Details")]
        public string UPIID { get; set; }

        [DisplayName("Bank Name")]
        public string Bankname { get; set; }

        [DisplayName("Account Number")]
        public string AccountNo { get; set; }

        [DisplayName("IFSC Code")]
        public string IFSC_Code { get; set; }
        
        [DisplayName("Cheque No.")]
        public string ChequeNo { get; set; }

        [DisplayName("Date of Cheque")]
        public DateTime ChequeDate { get; set; }
    }
}
