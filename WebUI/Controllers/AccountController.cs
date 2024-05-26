using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using WebAPI.Helpers;

namespace WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        readonly Constants constants = new Constants();
        HttpClient hc = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"].ToString())
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginObject userDetails)
        {
            var responseTask = hc.PostAsJsonAsync<UserLoginObject>("API/Account/ValidateLogin", userDetails);
            responseTask.Wait();
            var result = responseTask.Result;

            var readResult = result.Content.ReadAsStringAsync();
            readResult.Wait();
            var stringData = readResult.Result;

            userDetails = JsonConvert.DeserializeObject<UserLoginObject>(stringData);

            if (!string.IsNullOrEmpty(userDetails.Message))
            {
                ModelState.AddModelError("", userDetails.Message);
            }
            else
            {
                if (result.IsSuccessStatusCode)
                {
                    FormsAuthentication.SetAuthCookie(userDetails.UserName, false);  //rememberMe functionality
                    
                    if (userDetails.LoginType == "Admin" || userDetails.LoginType == "BotLogin" || userDetails.LoginType == "Seller")
                    {
                        return RedirectToAction("Index", "Products", new { userType = userDetails.LoginType });
                    }
                    if (userDetails.tempCartDetails != null)
                        ViewBag.tempCartDetails = userDetails.tempCartDetails.TempCartDetailsString;

                    return RedirectToAction("UserIndex", "Products", new { userType = userDetails.LoginType });
                }
            }
            return View();
        }

        public ActionResult Register()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View("Index", "Products");
            }
            ViewBag.lstLoginType = GetLoginTypeList();
            ViewBag.lstStateList = GetStateList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserDetails userDetails)
        {
            var responseTask = hc.PostAsJsonAsync<UserDetails>("API/Account/RegisterUser", userDetails);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsStringAsync();
                readResult.Wait();
                var stringData = readResult.Result;

                var user = JsonConvert.DeserializeObject<UserLoginObject>(stringData);

                if (!string.IsNullOrEmpty(user.Message))
                {
                    ViewBag.Message = user.Message;
                    if (result.IsSuccessStatusCode)
                    {
                        return View("CommonValidationPrinter");
                    }
                    else
                    {
                        ModelState.AddModelError("", user.Message);
                        return View();
                    }
                }
            }
            ModelState.AddModelError("", "Error in Registrating User..!!");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserProfile()
        {
            LstOrderDetails lstOrderDetails = new LstOrderDetails();

            var responseTask = hc.GetAsync("API/GetUserDetailsByUserID/" + User.Identity.Name);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<LstOrderDetails>();
                readResult.Wait();
                lstOrderDetails = readResult.Result;
            }
            return View(lstOrderDetails);
        }

        public List<CommonDropDown> GetLoginTypeList()
        {
            var loginList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetLoginTypeList");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                loginList = readResult.Result;
            }
            return loginList;
        }

        public List<CommonDropDown> GetStateList()
        {
            var stateList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetStateList");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                stateList = readResult.Result;
            }
            return stateList;
        }

        [HttpGet]
        public JsonResult GetCityList(int stateID)
        {
            var stateList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetCityList/" + stateID);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                stateList = readResult.Result;
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SellerProfile()
        {
            LstOrderDetailsForSeller lstOrderDetailsSeller = new LstOrderDetailsForSeller();

            var responseTask = hc.GetAsync("API/GetOrderDetailsForSeller/" + User.Identity.Name);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<LstOrderDetailsForSeller>();
                readResult.Wait();
                lstOrderDetailsSeller = readResult.Result;
            }
            return View(lstOrderDetailsSeller);
        }

        public ActionResult VerifyUser(string userName)
        {
            var responseTask = hc.GetAsync("API/Account/VerifyUser/" + userName);
            responseTask.Wait();
            var result = responseTask.Result;

            var readResult = result.Content.ReadAsStringAsync();
            readResult.Wait();
            var stringData = readResult.Result;

            if (!string.IsNullOrEmpty(stringData))
            {
                ViewBag.Message = stringData.Replace('"', ' ').Replace("/", "");
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Action = "Login";
                    ViewBag.Controller = "Account";
                    ViewBag.ButtonName = "Login";
                }
                else
                {
                    ViewBag.Action = "ContactUs";
                    ViewBag.Controller = "Account";
                    ViewBag.ButtonName = "Contact Us";
                }
            }
            return View("CommonValidationPrinter");
        }

        public ActionResult CommonValidationPrinter()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return RedirectToAction("ContactUs", "Services");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword objForgot)
        {
            var responseTask = hc.PostAsJsonAsync<ForgotPassword>("API/Account/ForgotPassword", objForgot);
            responseTask.Wait();
            var result = responseTask.Result;

            var readResult = result.Content.ReadAsStringAsync();
            readResult.Wait();
            var stringData = readResult.Result;

            var msg = JsonConvert.DeserializeObject<string>(stringData);

            if (!string.IsNullOrEmpty(msg))
            {
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = msg;
                    ViewBag.Link = constants.ContactUsURL;
                    ViewBag.ButtonName = "Contact Us";
                    return View("CommonValidationPrinter");
                }
                else
                {
                    ModelState.AddModelError("", msg);
                    return View();
                }
                
            }

            return View();
        }

        public ActionResult VerifyForgotPassword(string userName, string key)
        {
            ForgotPassword objForgot = new ForgotPassword();
            try
            {
                var ID = new Guid(key);
                objForgot.Username = userName;
                objForgot.Key = ID;
            }
            catch (Exception)
            {
                objForgot.Message = "Invalid Key..!!";
                return View(objForgot);
            }

            var responseTask = hc.PostAsJsonAsync<ForgotPassword>("API/Account/GetForgotPasswordDetails", objForgot);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsStringAsync();
                readResult.Wait();
                var stringData = readResult.Result;

                objForgot = JsonConvert.DeserializeObject<ForgotPassword>(stringData);

                if (!string.IsNullOrEmpty(objForgot.Message))
                {
                    return View(objForgot);
                }
            }

            return View(objForgot);
        }

        [HttpPost]
        public ActionResult VerifyForgotPassword(ForgotPassword objForgot)
        {
            var responseTask = hc.PostAsJsonAsync<ForgotPassword>("API/Account/VerifyForgotPassword", objForgot);
            responseTask.Wait();
            var result = responseTask.Result;

            var readResult = result.Content.ReadAsStringAsync();
            readResult.Wait();
            var stringData = readResult.Result;

            var msg = JsonConvert.DeserializeObject<ForgotPassword>(stringData);

            if (!string.IsNullOrEmpty(msg.Message))
            {
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = msg.Message;
                    ViewBag.Link = msg.Link;
                    ViewBag.ButtonName = "Retry";
                    return View("CommonValidationPrinter");
                }
                else
                {
                    ViewBag.Message = msg.Message;
                    ViewBag.Link = constants.ContactUsURL;
                    ViewBag.ButtonName = "Contact Us";
                    return View("CommonValidationPrinter");
                }
            }
            else
            {
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Password Changed Successfully..!!";
                    ViewBag.Link = constants.LoginURL;
                    ViewBag.ButtonName = "Login";
                    return View("CommonValidationPrinter");
                }
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword objChange)
        {
            return View();
        }

        public ActionResult AddAddress(string type)
        {
            Address address = new Address
            {
                Type = type
            };
            ViewBag.lstStateList = GetStateList();
            return PartialView(address);
        }

        [HttpPost]
        public JsonResult AddAddress(Address address)
        {
            address.UserID = User.Identity.Name;

            var responseTask = hc.PostAsJsonAsync<Address>("API/AddAddress", address);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return Json(new { IsSuccess = true, Type = address.Type }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {IsSuccess = false, ErrorMsg = "Some Error has Occures While Processing Your Request."}, JsonRequestBehavior.AllowGet);
        }

    }
}