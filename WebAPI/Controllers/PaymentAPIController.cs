using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    public class PaymentAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();
        private readonly CommonMethods _CommonMethods = new CommonMethods();
        Constants _Constants = new Constants();

        [HttpPost]
        [Route("API/SaveOrderDetails")]
        public IHttpActionResult SaveOrderDetails(PaymentDetail paymentDetail)
        {
            if(entities.tblOrderDetails.FirstOrDefault(a=>a.OrderID == paymentDetail.OrderID) != null)
            {
                return Ok("Order is Alreay Places. Please Wait for Order Confirmation Email.");
            }
            if (paymentDetail.PaymentOf == "ECommerce" && paymentDetail.CartDetails != null)
            {
                var OTP = GenerateOTP();
                foreach (var order in paymentDetail.CartDetails.lstOrderDetails)
                {
                    tblOrderDetail obj = new tblOrderDetail();
                    obj.OrderID = paymentDetail.OrderID;
                    obj.ProductID = order.ProductID;
                    obj.Quantity = order.Quantity;
                    obj.Amount = order.Amount;
                    obj.UserID = paymentDetail.UserName;
                    obj.CreatedDate = DateTime.Now;
                    obj.OrderStatus = 0;  //Order Pending
                    obj.ShippingAddressID_FK = paymentDetail.ShippingAddressID;
                    obj.BillingAddressID_FK = paymentDetail.BillingAddressID;
                    obj.OTPToBeShown = false;
                    obj.OTP = OTP;
                    entities.tblOrderDetails.Add(obj);

                    var objProduct = entities.tblProducts.Where(a => a.ProductID == order.ProductID).FirstOrDefault();
                    objProduct.Quantity -= order.Quantity;
                    objProduct.ModifiedDate = DateTime.Now;
                }
            }
            else if(paymentDetail.PaymentOf == "MovieTickets" && paymentDetail.movieBooking != null)
            {
                int TicketPrice = 0;
                var ScreenConfig = entities.tblMovieScreenTimingConfigs.Where(a => a.ScreenTimingConfigID == paymentDetail.movieBooking.ScreenTimingConfigID_FK).FirstOrDefault();
                if (ScreenConfig != null)
                {
                    var ScreenDet = entities.tblScreenDetails.Where(a => a.ScreenID == ScreenConfig.ScreenID_FK).ToList();
                    switch (paymentDetail.movieBooking.SeatCategory)
                    {
                        case 101:
                            TicketPrice = ScreenDet.Select(a => a.PriceSilver).FirstOrDefault();
                            break;
                        case 102:
                            TicketPrice = ScreenDet.Select(a => a.PriceGold).FirstOrDefault();
                            break;
                        case 103:
                            TicketPrice = ScreenDet.Select(a => a.PricePlatinum).FirstOrDefault();
                            break;
                        case 104:
                            TicketPrice = ScreenDet.Select(a => a.PriceRecliner).FirstOrDefault();
                            break;
                    }
                    paymentDetail.movieBooking.MovieName = entities.tblMovieDetails.Where(a => a.MovieID == ScreenConfig.MovieID_FK).Select(a => a.MovieName).FirstOrDefault();
                }

                paymentDetail.movieBooking.Amount = paymentDetail.movieBooking.NoOfSeats * (TicketPrice);

                var dbObj = new tblMovieBookingHistory
                {
                    BookingID = paymentDetail.movieBooking.BookingID,
                    ScreenTimingConfigID_FK = paymentDetail.movieBooking.ScreenTimingConfigID_FK,
                    MovieName = paymentDetail.movieBooking.MovieName,
                    NoOfSeats = paymentDetail.movieBooking.NoOfSeats,
                    SeatNo = paymentDetail.movieBooking.SeatNo,
                    Amount = paymentDetail.movieBooking.Amount,
                    UserName = paymentDetail.movieBooking.UserName,
                    SeatCategory = paymentDetail.movieBooking.SeatCategory,
                    IsUsed = false,
                    CreatedDate = DateTime.Now
                };
                entities.tblMovieBookingHistories.Add(dbObj);
            }

            try
            {
                int success = entities.SaveChanges();
                if (success > 0)
                {
                    paymentDetail.PaymentModeDetails.PaymentDetails = JsonConvert.SerializeObject(paymentDetail);
                    paymentDetail.CreatedDate = DateTime.Now;
                    paymentDetail.PaymentModeDetails.PaymentStatus = 2006;
                    paymentDetail.TransactionNo = _CommonMethods.GenerateTransactionNo(paymentDetail.PaymentModeDetails.PaymentMode);
                    SavePaymentDetails(paymentDetail);

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(_Constants.ErrorMsg);
                throw;
            }

            return BadRequest(_Constants.ErrorMsg);

        }

        public void SavePaymentDetails(PaymentDetail objPayment)
        {
            if (objPayment != null)
            {
                tblPayment objtblPay = new tblPayment();
                objtblPay.OrderID = objPayment.OrderID;
                objtblPay.PaymentMode = objPayment.PaymentModeDetails.PaymentMode;
                objtblPay.PaymentDetails = objPayment.PaymentModeDetails.PaymentDetails;
                objtblPay.UserName = objPayment.UserName;
                objtblPay.CreatedDate = DateTime.Now;

                switch (objtblPay.PaymentMode)
                {
                    case "Online":
                        break;
                    case "Cheque":
                        objtblPay.ChequeNo = objPayment.PaymentModeDetails.ChequeNo;
                        objtblPay.ChequeDate = objPayment.PaymentModeDetails.ChequeDate;
                        objtblPay.AccountNo = objPayment.PaymentModeDetails.AccountNo;
                        objtblPay.IFSC_Code = objPayment.PaymentModeDetails.IFSC_Code;
                        break;
                    case "UPI":
                        objtblPay.UPI_ID = objPayment.PaymentModeDetails.UPIID;
                        break;
                }

                objtblPay.PaymentStatus = objPayment.PaymentModeDetails.PaymentStatus;
                
                objtblPay.TransactionNo = objPayment.TransactionNo;
                objtblPay.TotalPayableAmount = objPayment.TotalPayableAmount;
                entities.tblPayments.Add(objtblPay);
                entities.SaveChanges();
            }
        }

        [HttpGet]
        [Route("API/GetAddressByUserID/{userName}")]
        public List<Address> GetAddressByUserID(string userName)
        {
            List<Address> lstAddress = new List<Address>();
            lstAddress = (from obj in entities.tblAddresses
                          join state in entities.tblMasStates on obj.StateID equals state.StateID
                          join city in entities.tblMasCities on obj.CityID equals city.CityID
                          where obj.UserID == userName
                          select new Address
                          {
                              AddressID = obj.AddressID,
                              Name = obj.Name,
                              Type = obj.Type,
                              UserID = userName,
                              StateID = obj.StateID,
                              CityID = obj.CityID,
                              City = city.City,
                              State = state.State,
                              Pincode = obj.Pincode,
                              Address1 = obj.Address1,
                              Address2 = obj.Address2,
                              Landmark = obj.Landmark,
                              FullAddress = obj.FullAddress,
                              CreatedDate = DateTime.Now,
                          }).ToList();

            return lstAddress;
        }

        public int GenerateOTP()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return Convert.ToInt32(r);
        }


    }
}