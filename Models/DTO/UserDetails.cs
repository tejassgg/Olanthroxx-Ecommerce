using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UserLoginObject
    {
        public int AccountID { get; set; }
        public System.Guid UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHistory { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastPasswordChange { get; set; }
        public string LoginType { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
        public TempCartDetails tempCartDetails { get; set; }
    }

    public class UserDetails
    {
        public int UserDetailsID { get; set; }
        public int AccountID_FK { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MidName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Mobile Number")]
        public string MobileNo { get; set; }

        [DisplayName("Email ID")]
        public string EmailID { get; set; }

        [DisplayName("Aadhar Number")]
        public string AadharNumber { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        [DisplayName("PIN Code")]
        public int PinCode { get; set; }

        [DisplayName("Full Address")]
        public string FullAddress { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public UserLoginObject loginObject { get; set; }
    }

    public class ChangePassword
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int AttemptNo { get; set; }
        public int MaxNoOfAttempts { get; set; }
        public System.Guid Key { get; set; }
        public System.DateTime ExpiryTime { get; set; }
        public string Link { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string EmailID {get; set;}
        public string Message { get; set; }
    }
}
