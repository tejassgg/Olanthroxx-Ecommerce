using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    public class AccountAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();
        readonly Constants constants = new Constants();
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
        private readonly CommonMethods _CommonMethods = new CommonMethods();

        [HttpPost]
        [Route("API/Account/ValidateLogin")]
        public IHttpActionResult ValidateLogin(UserLoginObject userDetails)
        {
            var User = new UserLoginObject();

            try
            {
                User = entities.tblUsers.Where(a => a.UserName == userDetails.UserName).
                        Select(x => new UserLoginObject
                        {
                            AccountID = x.AccountID,
                            UserName = x.UserName,
                            LoginType = x.LoginType,
                            Password = x.Password,
                            PasswordSalt = x.PasswordSalt,
                            IsActive = x.IsActive == true ? true : false
                        }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return BadRequest("DB Error:- " + ex.Message + " " + ex.StackTrace);
            }

            if (User != null)
            {
                User.tempCartDetails = new TempCartDetails();
                User.tempCartDetails = entities.tblTempCartDetails.Where(a => a.AccountID_FK == User.AccountID && a.IsUsed == false).
                    Select(x => new TempCartDetails
                    {
                        TempCartDetailsString = x.TempCartDetails,
                        SessionID = x.SessionID,
                        IsUsed = x.IsUsed,
                        CreatedDate = x.CreatedDate,
                        UsedDate = x.UsedDate
                    }).FirstOrDefault();

                if (!User.IsActive)
                {
                    return BadRequest("Please activate user using the verification link sent on to your registered Email ID.");
                }
                else
                {
                    if (encryptDecrypt.Encrypt(userDetails.Password + User.PasswordSalt.Replace(" ", ""), constants.EncryptTypePass) == User.Password)
                    {
                        return Ok(new UserLoginObject { UserName = User.UserName, LoginType = User.LoginType, tempCartDetails = User.tempCartDetails ?? null });
                    }
                    return BadRequest("Invalid Credentials.");
                }
            }
            return BadRequest("User doesn't exists in our records.");
        }

        [HttpGet]
        [Route("API/Account/GetRoles/{username}")]
        public string[] GetRoles(string username)
        {
            var roles = entities.tblUsers.Where(a => a.UserName == username).Select(a => a.LoginType).ToArray();
            return roles;
        }

        [HttpPost]
        [Route("API/Account/RegisterUser")]
        public IHttpActionResult RegisterUser(UserDetails userDetails)
        {
            var User = entities.tblUsers.Where(a => a.UserName == userDetails.loginObject.UserName).
                Select(x => new UserLoginObject
                {
                    AccountID = x.AccountID,
                    UserName = x.UserName,
                    LoginType = x.LoginType,
                    Password = x.Password,
                    PasswordSalt = x.PasswordSalt
                }).FirstOrDefault();

            if (User == null)
            {
                var Salt = _CommonMethods.GenerateSalt();
                var pass = encryptDecrypt.Encrypt(userDetails.loginObject.Password + Salt.Replace(" ", ""), constants.EncryptTypePass);
                tblUser NewUser = new tblUser
                {
                    UserName = userDetails.loginObject.UserName,
                    UserID = Guid.NewGuid(),
                    LoginType = userDetails.loginObject.LoginType,
                    CreatedDate = DateTime.Now,
                    LastPasswordChange = DateTime.Now,
                    PasswordSalt = Salt,
                    Password = pass,
                    PasswordHistory = "",
                };

                entities.tblUsers.Add(NewUser);

                var StateID = Convert.ToInt16(userDetails.State);
                var CityID = Convert.ToInt16(userDetails.City);
                var State = entities.tblMasStates.Where(a => a.StateID == StateID).Select(a => a.State).FirstOrDefault();
                var City = entities.tblMasCities.Where(a => a.CityID == CityID).Select(a => a.City).FirstOrDefault();

                tblUserDetail newUserDetails = new tblUserDetail
                {
                    AccountID_FK = NewUser.AccountID,
                    FirstName = userDetails.FirstName,
                    MidName = userDetails.MidName,
                    LastName = userDetails.LastName,
                    MobileNo = userDetails.MobileNo,
                    AadharNumber = userDetails.AadharNumber,
                    EmailID = userDetails.EmailID,
                    State = State,
                    City = City,
                    PinCode = userDetails.PinCode,
                    FullAddress = userDetails.FullAddress,
                    CreatedDate = DateTime.Now
                };

                entities.tblUserDetails.Add(newUserDetails);

                tblAddress objAddress = new tblAddress
                {
                    Name = userDetails.FirstName + " " + userDetails.LastName,
                    Type = constants.AddressTypeBilling,
                    UserID = userDetails.loginObject.UserName,
                    StateID = StateID,
                    CityID = CityID,
                    Pincode = userDetails.PinCode,
                    FullAddress = userDetails.FullAddress,
                    CreatedDate = DateTime.Now,
                };

                entities.tblAddresses.Add(objAddress);

                int success = entities.SaveChanges();

                if (success > 0)
                {
                    User = new UserLoginObject();
                    User.UserName = userDetails.loginObject.UserName;
                    User.Password = userDetails.loginObject.Password;

                    EmailDetails objEmail = new EmailDetails();
                    objEmail.UserName = userDetails.EmailID;
                    objEmail.LoginType = userDetails.loginObject.LoginType;
                    objEmail.EmailType = constants.UserRegistration;

                    SendEmail(objEmail);
                    User.Message = "Thanks for Registering on Olanthroxx. An email has been sent, which contains a link to verfiy you account.";
                    return Ok(User);
                }
            }
            return BadRequest("User Registration failed due to some technical error.");
        }

        public void SendEmail(EmailDetails emailObj)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                switch (emailObj.EmailType)
                {
                    case "UserRegistration":
                        mailMessage = new MailMessage(constants.FromEmailID, emailObj.EmailID)
                        {
                            Subject = constants.EmailSubjectforRegistration,
                            Body = constants.EmailBodyforRegistration.Replace("txtUserName", emailObj.UserName).
                                    Replace("txtActionURL", constants.LoginURL).
                                    Replace("txtContactUsURL", constants.ContactUsURL).
                                    Replace("txtSupportEmailID", constants.FromEmailID).
                                    Replace("txtLoginType", emailObj.LoginType).
                                    Replace("txtVerificationLinkURL", constants.VerificationURL + emailObj.UserName),
                            IsBodyHtml = true
                        };
                        break;
                    case "OutForDelivery":
                        var msgBody = constants.EmailBodyforOutForDelivery.Replace("txtUserName", emailObj.UserName).
                                    Replace("txtQuantity", emailObj.NoOfItems.ToString()).
                                    Replace("txtOrderID", emailObj.OrderID).
                                    Replace("txtOrderDetailsLink", constants.BuyerOrdeDetailsURL + emailObj.OrderID).
                                    Replace("txtTrackPackageLink", constants.TrackPackageURL + emailObj.OrderID);

                        var OTP = emailObj.OTP;
                        for (int i = 0; i < OTP.Length; i++)
                        {
                            msgBody = msgBody.Replace("txtOTP" + (i+1).ToString(), OTP[i].ToString());
                        }

                        mailMessage = new MailMessage(constants.FromEmailID, emailObj.EmailID)
                        {
                            Subject = constants.EmailSubjectforOutForDelivery,
                            Body = msgBody,
                            IsBodyHtml = true
                        };
                        break;
                }
                

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient smtpClient = new SmtpClient(constants.SMTPHost)
                {
                    Port = constants.SMTPPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(constants.FromEmailID, constants.EmailPassword)
                };

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SendForgotPasswordMail(ForgotPassword objForgot)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(constants.FromEmailID, objForgot.EmailID)
                {
                    Subject = constants.EmailSubjectforForgotPassword,
                    Body = constants.EmailBodyforForgotPassword.Replace("txtUserName", objForgot.Username).
                    Replace("txtActionURL", constants.LoginURL).
                    Replace("txtContactUsURL", constants.ContactUsURL).
                    Replace("txtSupportEmailID", constants.FromEmailID).
                    Replace("txtVerificationLinkURL", objForgot.Link).
                    Replace("txtLinkExpiryTime", constants.ForgotPasswordLinkExpiryHours.ToString()),
                    IsBodyHtml = true
                };

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                SmtpClient smtpClient = new SmtpClient(constants.SMTPHost)
                {
                    Port = constants.SMTPPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(constants.FromEmailID, constants.EmailPassword)
                };

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("API/Account/VerifyUser/{userName}")]
        public IHttpActionResult VerifyUser(string userName)
        {
            var User = entities.tblUsers.Where(a => a.UserName == userName).FirstOrDefault();
            if (User == null)
            {
                return BadRequest("User doesn't exist in our records..!!");
            }
            else
            {
                if (User.IsActive == true)
                {
                    return Ok("User Already Verified..!!");
                }

                User.IsActive = true;
                int success = entities.SaveChanges();
                if (success > 0)
                {
                    return Ok("Verification Successfull..!!");
                }
            }
            return BadRequest("Oops. Some Error Occured while Verifying the User.");
        }

        [HttpPost]
        [Route("API/Account/ForgotPassword")]
        public IHttpActionResult ForgotPassword(ForgotPassword objForgot)
        {
            var User = entities.tblUsers.Where(a => a.UserName == objForgot.Username).FirstOrDefault();

            var UserDetails = entities.tblUserDetails.Where(a => a.AccountID_FK == User.AccountID).FirstOrDefault();

            if (UserDetails != null)
            {
                var newKey = Guid.NewGuid();
                tblForgotPasswordHistory forgotDetails = new tblForgotPasswordHistory
                {
                    Username = objForgot.Username,
                    Key = newKey,
                    AttemptNo = 0,
                    MaxNoOfAttempts = 5,
                    ExpiryTime = DateTime.Now.AddHours(constants.ForgotPasswordLinkExpiryHours),
                    Link = constants.ForgotPassowrdLink.Replace("txtUserName", objForgot.Username).Replace("txtKey", newKey.ToString()),
                    CreatedDate = DateTime.Now,
                    IsValid = true,
                };

                entities.tblForgotPasswordHistories.Add(forgotDetails);

                int success = entities.SaveChanges();
                if (success > 0)
                {
                    objForgot.Link = forgotDetails.Link;
                    SendForgotPasswordMail(objForgot);
                    return Ok("A Mail with password reset link has been shared to your registered email ID");
                }
            }
            return BadRequest("Error in Process. Please try again after some time.");
        }

        [HttpPost]
        [Route("API/Account/GetForgotPasswordDetails")]
        public IHttpActionResult GetForgotPasswordDetails(ForgotPassword objForgot)
        {
            var User = entities.tblUsers.Where(a => a.UserName == objForgot.Username).FirstOrDefault();

            if (User != null)
            {
                var UserDetails = entities.tblUserDetails.Where(a => a.AccountID_FK == User.AccountID).FirstOrDefault();

                var forgotDB = entities.tblForgotPasswordHistories.Where(a => a.Username == objForgot.Username && a.IsValid == true && a.Key == objForgot.Key).FirstOrDefault();
                if (forgotDB != null)
                {
                    if (forgotDB.PasswordChangedDate != null)
                    {
                        objForgot.Message = "Password Already Changed..!!";
                        return Ok(objForgot);
                    }
                    else if (forgotDB.ExpiryTime < DateTime.Now)
                    {
                        objForgot.Message = "Link has Expired..!!";
                        return Ok(objForgot);
                    }
                    else
                    {
                        objForgot.AttemptNo = forgotDB.AttemptNo;
                        objForgot.MaxNoOfAttempts = forgotDB.MaxNoOfAttempts;
                        return Ok(objForgot);
                    }
                }
                else
                {
                    objForgot = new ForgotPassword();
                    objForgot.Message = "Authentication Failed..!!";
                    return Ok(objForgot);
                }
            }
            return Ok("User not found..!!");

        }

        [HttpPost]
        [Route("API/Account/VerifyForgotPassword")]
        public IHttpActionResult VerifyForgotPassword(ForgotPassword objForgot)
        {
            var User = entities.tblUsers.Where(a => a.UserName == objForgot.Username).FirstOrDefault();

            var UserDetails = entities.tblUserDetails.Where(a => a.AccountID_FK == User.AccountID).FirstOrDefault();

            var forgotDB = entities.tblForgotPasswordHistories.Where(a => a.Username == objForgot.Username && a.IsValid == true && a.Key == objForgot.Key).FirstOrDefault();
            if (forgotDB != null)
            {
                if (encryptDecrypt.Encrypt(objForgot.Password + User.PasswordSalt.Replace(" ", ""), constants.EncryptTypePass) == User.Password)
                {
                    objForgot.Link = forgotDB.Link;
                    objForgot.Message = "Old and New password are same..!!";
                    return Ok(objForgot);
                }

                forgotDB.AttemptNo = objForgot.AttemptNo;
                forgotDB.PasswordChangedDate = DateTime.Now;
                forgotDB.IsValid = false;

                User.LastPasswordChange = DateTime.Now;
                User.PasswordHistory += ", " + User.Password;
                User.Password = encryptDecrypt.Encrypt(objForgot.Password + User.PasswordSalt.Replace(" ", ""), constants.EncryptTypePass);

                int success = entities.SaveChanges();

                if (success > 0)
                {
                    return Ok(objForgot);
                }
            }
            else
            {
                return BadRequest("Authentication Failed..!!");
            }
            return BadRequest("Not Allowed.");
        }

        [HttpPost]
        [Route("API/AddAddress")]
        public IHttpActionResult AddAddress(Address address)
        {
            var CityID = entities.tblMasCities.Where(a => a.City == address.City).Select(a => a.CityID).FirstOrDefault();

            tblAddress obj = new tblAddress
            {
                Name = address.Name,
                Address1 = address.Address1,
                Address2 = address.Address2,
                FullAddress = address.Address1 + " " + address.Address2,
                Landmark = address.Landmark,
                StateID = address.StateID,
                CityID = CityID,
                Pincode = address.Pincode,
                UserID = address.UserID,
                Type = address.Type,
                CreatedDate = DateTime.Now,
            };

            entities.tblAddresses.Add(obj);
            int success = entities.SaveChanges();
            
            if (success > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("API/Account/GetUserDetailListForAdmin")]
        public IHttpActionResult GetUserDetailListForAdmin() 
        {
            UserManagement objUser = new UserManagement();
            objUser.userDetails = new List<UserDetails>();

            objUser.userDetails = (from obj in  entities.tblUsers 
                     join obj1 in entities.tblUserDetails on obj.AccountID equals obj1.AccountID_FK
                     select new UserDetails
                     {
                         UserName = obj.UserName,
                         IsActive = obj.IsActive == true ? true : false,
                         FirstName = obj1.FirstName,
                         MidName = obj1.MidName,
                         LastName = obj1.LastName,
                         CreatedDate = obj.CreatedDate,
                         City = obj1.City,
                         MobileNo = obj1.MobileNo,
                         UserType = obj.LoginType
                     }).OrderBy(a=>a.CreatedDate).ToList();

            if (objUser.userDetails.Count > 0)
            {
                return  Ok(objUser);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("API/Account/UpdateActiveStatus/{userName}")]
        public IHttpActionResult UpdateActiveStatus(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = entities.tblUsers.FirstOrDefault(a => a.UserName == userName);
                if (user != null)
                {
                    user.IsActive = !user.IsActive;
                    int success = entities.SaveChanges();
                    if (success > 0) {
                        return Ok();
                    }
                }
                else
                    return BadRequest();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("API/Account/GetAdminDashboardDetails/{currentUser}")]
        public IHttpActionResult GetAdminDashboardDetails(string currentUser)
        {
            try
            {
                AdminDashboard adminData = new AdminDashboard();
                adminData.userList = new List<UserDetails>();
                adminData.productList = new List<ProductDetails>();

                adminData.userList = (from obj in entities.tblUsers
                                      join obj1 in entities.tblUserDetails on obj.AccountID equals obj1.AccountID_FK
                                      where obj.UserName != currentUser
                                      select new UserDetails
                                      {
                                          UserName = obj.UserName,
                                          IsActive = obj.IsActive == true ? true : false,
                                          FirstName = obj1.FirstName,
                                          MidName = obj1.MidName,
                                          LastName = obj1.LastName,
                                          CreatedDate = obj.CreatedDate,
                                          City = obj1.City,
                                          MobileNo = obj1.MobileNo,
                                          UserType = obj.LoginType
                                      }).OrderBy(a => a.UserType).ToList();

                adminData.productList = (from obj in entities.tblProducts
                                         select new ProductDetails
                                         {
                                             Quantity = obj.Quantity,
                                             ProductID = obj.ProductID,
                                             CategoryID_FK = obj.CategoryID_FK,
                                             PName = obj.PName,
                                             Price = obj.Price,
                                             Category = entities.tblCategories.Where(a => a.CategoryID == obj.CategoryID_FK).Select(a => a.CategoryType).FirstOrDefault()
                                         }).ToList();


                List<OrderDetails> orderDetails = new List<OrderDetails>();
                orderDetails = (from obj in entities.tblOrderDetails
                                join obj2 in entities.tblProducts on obj.ProductID equals obj2.ProductID
                                select new OrderDetails
                                {
                                    OrderID = obj.OrderID,
                                    ProductID = obj2.ProductID,
                                    Amount = obj.Amount,
                                    Quantity = obj.Quantity,
                                    OrderDate = obj.CreatedDate,
                                    ProductName = obj2.PName,
                                    OrderStatus = entities.tblMasCommonTypes.Where(a => a.MasterType == "OrderStatus" && a.Code == obj.OrderStatus).FirstOrDefault().Description,
                                    ModifiedDate = obj.ModifiedDate,
                                    ShippingAddressID = obj.ShippingAddressID_FK,
                                    BilllingAddressID = obj.BillingAddressID_FK,
                                }).OrderByDescending(a => a.OrderStatus).ToList();

                adminData.TotalSales = orderDetails.Count();
                adminData.TotalSalesQuantity = orderDetails.Sum(a => a.Quantity);
                adminData.TotalSalesAmount = orderDetails.Sum(a => a.Amount);

                adminData.TotalProducts = adminData.productList.Count();
                adminData.ActiveUsersCount = entities.tblUsers.Where(a => a.IsActive == true).Count();

                orderDetails = orderDetails.GroupBy(a => a.OrderID).Select(obj => new OrderDetails
                {
                    OrderID = obj.Max(a => a.OrderID),
                    Amount = obj.Sum(a => a.Amount),
                    Quantity = obj.Sum(a => a.Quantity),
                    OrderStatus = obj.Max(a => a.OrderStatus),
                    OrderDate = obj.Max(a => a.OrderDate),
                }).ToList();

                adminData.monthwiseSales = new List<MonthWiseSales>();

                adminData.monthwiseSales = orderDetails.GroupBy(a => a.OrderDate.Month).Select(obj => new MonthWiseSales
                {
                    Month = obj.Max(a => a.OrderDate).Month, //.ToString("MMM")
                    MonthQuantity = obj.Sum(a => a.Quantity),
                    MonthAmount = obj.Sum(a => a.Amount),
                    MonthOrders = obj.Count()
                }).ToList();
                adminData.monthwiseSales = adminData.monthwiseSales.OrderByDescending(a => a.Month).ToList();

                adminData.Month = JsonConvert.SerializeObject(adminData.monthwiseSales.Select(a => a.Month).ToList());
                adminData.MonthAmount = JsonConvert.SerializeObject(adminData.monthwiseSales.Select(a => a.MonthAmount).ToList());
                adminData.MonthQuantity = JsonConvert.SerializeObject(adminData.monthwiseSales.Select(a => a.MonthQuantity).ToList());
                adminData.MonthOrders = JsonConvert.SerializeObject(adminData.monthwiseSales.Select(a => a.MonthOrders).ToList());


                return Ok(adminData);
            }
            catch (Exception ex)
            {
                return BadRequest("Some Error Occured While Fetching the Details..!");
                throw;
            }
        }
    }
}