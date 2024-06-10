using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class PaymentController : Controller
    {
        HttpClient hc = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"].ToString())
        };

        public List<CommonDropDown> GetPaymentModes()
        {
            var listCat = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetPaymentModes");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                listCat = readResult.Result;
            }
            return listCat;
        }

        [HttpPost]
        public ActionResult Payment(string PaymentOf, List<OrderDetails> listOrderDetails = null, MovieBooking movieBooking = null)
        {
            var TotalAmount = decimal.Zero;
            var OrderID = System.Guid.NewGuid();
            var paymentDetails = new PaymentDetail();

            if(PaymentOf == "ECommerce" && listOrderDetails != null && listOrderDetails.Count() > 0)
            {
                paymentDetails.OrderID = OrderID;
                paymentDetails.CartDetails = new CartDetails();
                paymentDetails.CartDetails.lstOrderDetails = new List<OrderDetails>();

                paymentDetails.CartDetails.lstOrderDetails = listOrderDetails;
                foreach (var item in listOrderDetails)
                {
                    item.OrderID = OrderID;
                    TotalAmount += item.Amount;
                }
                paymentDetails.CartDetails.TotalAmount = TotalAmount;
                paymentDetails.CartDetails.OrderID = OrderID;
                paymentDetails.HdnCartDetails = JsonConvert.SerializeObject(paymentDetails.CartDetails);
                paymentDetails.LstAddress = GetAddressByUserID();
            }
            else if(PaymentOf == "MovieTickets" && movieBooking != null)
            {
                movieBooking.BookingID = OrderID;
                movieBooking.CreatedDate = DateTime.Now;
                movieBooking.UserName = User.Identity.Name;
                movieBooking = GetCalculateMovieTicketCost(movieBooking);
                TotalAmount = movieBooking.Amount;
                paymentDetails.movieBooking = new MovieBooking();
                paymentDetails.movieBooking = movieBooking;

                paymentDetails.HdnMovieBooking = JsonConvert.SerializeObject(paymentDetails.movieBooking);
            }

            paymentDetails.PaymentOf = PaymentOf;
            paymentDetails.PaymentModeDetails = new PaymentModeDetails();
            paymentDetails.TotalPayableAmount = Convert.ToInt32(TotalAmount);
            
            paymentDetails.OrderID = OrderID;
            paymentDetails.UserName = User.Identity.Name;
            paymentDetails.CreatedDate = DateTime.Now;

            ViewBag.lstPaymentMode = GetPaymentModes();

            return View(paymentDetails);
        }

        [HttpPost]
        public JsonResult ProceedToPayment(PaymentDetail paymentDetail)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();

            if (paymentDetail.PaymentOf == "ECommerce")
            {
                paymentDetail.CartDetails = new CartDetails();
                paymentDetail.CartDetails = JsonConvert.DeserializeObject<CartDetails>(paymentDetail.HdnCartDetails, settings);
            }
            else if (paymentDetail.PaymentOf == "MovieTickets")
            {
                paymentDetail.movieBooking = new MovieBooking();
                paymentDetail.movieBooking = JsonConvert.DeserializeObject<MovieBooking>(paymentDetail.HdnMovieBooking, settings);
            }

            var responseTask = hc.PostAsJsonAsync<PaymentDetail>("API/SaveOrderDetails", paymentDetail);
            responseTask.Wait();
            var result = responseTask.Result;

            return Json(new { IsSuccess = result.IsSuccessStatusCode, Message = result.IsSuccessStatusCode ? "Your Order Has Been Placed Successfully..!!" : result.ReasonPhrase }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult PaymentSuccessfull (PaymentDetail paymentDetail)
        {
            return View(paymentDetail);
        }

        public List<Address> GetAddressByUserID()
        {
            string userName = User.Identity.Name;
            List<Address> lstAddress = new List<Address>();

            var responseTask = hc.GetAsync("API/GetAddressByUserID/" + userName);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<Address>>();
                readResult.Wait();
                lstAddress = readResult.Result;
            }

            return lstAddress;
        }

        public JsonResult GetJsonAddressByUserID()
        {
            string userName = User.Identity.Name;
            List<Address> lstAddress = new List<Address>();

            var responseTask = hc.GetAsync("API/GetAddressByUserID/" + userName);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<Address>>();
                readResult.Wait();
                lstAddress = readResult.Result;
            }

            return Json(lstAddress, JsonRequestBehavior.AllowGet);
        }

        public MovieBooking GetCalculateMovieTicketCost(MovieBooking movie)
        {
            var responseTask = hc.PostAsJsonAsync<MovieBooking>("API/BookTicket/GetCalculateMovieTicketCost/", movie);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<MovieBooking>();
                readResult.Wait();
                movie = readResult.Result;
            }

            return movie;
        }

    }
}