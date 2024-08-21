using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;
using WebAPI.Helpers;
using static System.Net.WebRequestMethods;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        readonly Constants constants = new Constants();
        readonly HttpClient hc = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"].ToString())
        };

        [Authorize(Roles ="Admin, BotLogin, Seller, Buyer")]
        public ActionResult Index(string userType)
        {
            if (User.Identity.IsAuthenticated)
            {
                string[] listRoles = new string[5];

                var rsTask = hc.GetAsync("API/Account/GetRoles/" + User.Identity.Name);
                rsTask.Wait();
                var rs = rsTask.Result;

                if (rs.IsSuccessStatusCode)
                {
                    var readResult = rs.Content.ReadAsAsync<string[]>();
                    readResult.Wait();
                    listRoles = readResult.Result;
                }
                userType = listRoles[0];
            }
            else
            {
                return RedirectToAction("LogOut", "Account");
            }

            if (string.IsNullOrEmpty(userType))
                return RedirectToAction("LogOut", "Account");

            if (User.IsInRole("Buyer"))
                return RedirectToAction("UserIndex");

            var ProductList = GetProductDetails(userType.ToLower(), "index");
            ViewBag.userType = userType;
            return View(ProductList);
        }

        //[Authorize(Roles = "Buyer, BotLogin")]
        [AllowAnonymous]
        public ActionResult UserIndex(string userType)
        {
            if (User.Identity.IsAuthenticated)
            {
                string[] listRoles = new string[5];

                var rsTask = hc.GetAsync("API/Account/GetRoles/" + User.Identity.Name);
                rsTask.Wait();
                var rs = rsTask.Result;

                if (rs.IsSuccessStatusCode)
                {
                    var readResult = rs.Content.ReadAsAsync<string[]>();
                    readResult.Wait();
                    listRoles = readResult.Result;
                }
                userType = listRoles[0];
            }


            if (userType == "Seller")
            {
                return RedirectToAction("LogOut", "Account");
            }
            else if (string.IsNullOrEmpty(userType)) {
                userType = "notlogged";
            }
            else
            {
                userType = "notlogged";
            }

            try
            {
                var ProductList = new List<ProductDetails>();
                ViewBag.FilterCategoryList = GetCategoryList();
                ProductList = GetProductDetails(userType.ToLower(), "shop");
                return View(ProductList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "No Items to Purchase..!!");
                return View();
            }
        }

        [Authorize(Roles = "Admin, BotLogin, Seller")]
        public ActionResult AddProduct()
        {
            ViewBag.CategoryList = GetCategoryList();
            return View();
        }

        [Authorize(Roles = "Admin, BotLogin, Seller")]
        [HttpPost]
        public ActionResult AddProduct(ProductDetails obj, HttpPostedFileBase imgFile)
        {
            obj.SellerName = User.Identity.Name;

            string path = "";
            Random random = new Random();
            int rnd = random.Next(1000,9999);
            if (imgFile != null && imgFile.ContentLength > 0)
            {
                string ext = Path.GetExtension(imgFile.FileName);
                {
                    if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".jpeg") || ext.ToLower().Equals(".png"))
                    {
                        try
                        {
                            string tempFileName = User.Identity.Name + "-" + obj.PName.Replace(" ","") + "-" + rnd + ext;
                            path = Path.Combine(Server.MapPath(constants.ProductImagePath), tempFileName);
                            imgFile.SaveAs(path);
                            path = constants.ProductImagePath + "/" + tempFileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Some Error Occured While Saving the Image.( " + ex.Message + " )");
                            ViewBag.CategoryList = GetCategoryList();
                            return View(obj);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only .JPG, .JPEG, .PNG format is allowed.");
                        ViewBag.CategoryList = GetCategoryList();
                        return View(obj);
                    }
                }
            }

            obj.ImgPath = path;

            var responseTask = hc.PostAsJsonAsync<ProductDetails>("API/AddProduct", obj);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Product Added Succesfully..!";

                string[] listRoles = new string[5];

                var rsTask = hc.GetAsync("API/Account/GetRoles/" + User.Identity.Name);
                rsTask.Wait();
                var rs = rsTask.Result;

                if (rs.IsSuccessStatusCode)
                {
                    var readResult = rs.Content.ReadAsAsync<string[]>();
                    readResult.Wait();
                    listRoles = readResult.Result;
                }
                return RedirectToAction("Index", "Products", listRoles[0]);
            }
            ViewBag.Message = "Product Additon Failed.!";
            return View("AddProduct");
        }
        
        [Authorize(Roles = "Admin, BotLogin, Seller")]
        public ActionResult EditProduct(int id)
        {
            var obj = GetProductDetailsbyID(id);
            ViewBag.CategoryList = GetCategoryList();
            return View(obj);
        }

        [Authorize(Roles = "Admin, BotLogin, Seller")]
        [HttpPost]
        public ActionResult EditProduct(ProductDetails obj, HttpPostedFileBase imgFile)
        {
            string path = "";
            Random random = new Random();
            int rnd = random.Next(1000, 9999);
            if (imgFile != null && imgFile.ContentLength > 0)
            {
                string ext = Path.GetExtension(imgFile.FileName);
                {
                    if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".jpeg") || ext.ToLower().Equals(".png"))
                    {
                        try
                        {
                            string tempFileName = User.Identity.Name + "-" + obj.PName.Replace(" ", "") + "-" + rnd + ext;
                            path = Path.Combine(Server.MapPath(constants.ProductImagePath), tempFileName);
                            imgFile.SaveAs(path);
                            path = constants.ProductImagePath + "/" + tempFileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Some Error Occured While Saving the Image.( " + ex.Message + " )");
                            ViewBag.CategoryList = GetCategoryList();
                            return View(obj);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only .JPG, .JPEG, .PNG format is allowed.");
                        ViewBag.CategoryList = GetCategoryList();
                        return View(obj);
                    }
                }
                obj.ImgPath = path;
            }            

            var responseTask = hc.PostAsJsonAsync<ProductDetails>("API/UpdateProduct", obj);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Product Added Succesfully..!";
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Product Additon Failed.!";
            return View("AddProduct");
        }

        [Authorize(Roles = "Admin, BotLogin, Seller")]
        public JsonResult DeleteProduct(int id)
        {
            var responseTask = hc.GetAsync("API/DeleteProduct/" + id + "/" + User.Identity.Name);
            responseTask.Wait();
            var result = responseTask.Result;

            return Json(new { IsSuccess = result.IsSuccessStatusCode, Message = result.IsSuccessStatusCode ? "Your Order Has Been Placed Successfully..!!" : result.ReasonPhrase }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult ViewProductDetails(int id)
        {
            var obj = GetProductDetailsbyID(id);
            return View(obj);
        }

        [Authorize(Roles = "BotLogin, Seller")]
        public ActionResult SellerOrderDetails(string orderID)
        {
            CartDetails orderDetails = new CartDetails();
            var responseTask = hc.GetAsync("API/GetOrderDetailsByOrderID/" + orderID);
            responseTask.Wait();
            HttpResponseMessage result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<CartDetails>();
                readResult.Wait();
                orderDetails = readResult.Result;
            }

            ViewBag.lstOrdStatusList = GetOrderStatusList();

            return View(orderDetails);
        }

        [Authorize(Roles = "BotLogin, Seller")]
        [HttpPost]
        public JsonResult SellerOrderDetails(CartDetails cartDetails)
        {
            cartDetails.LoggedInUser = User.Identity.Name;
            var responseTask = hc.PostAsJsonAsync<CartDetails>("API/UpdateSellerOrderDetails", cartDetails);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return Json(new { IsSuccess = true, Message = "Order Updated Succesfully..!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsSuccess = false, Message = "Some Error has Occures While Processing Your Request." }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "BotLogin, Buyer")]
        public ActionResult BuyerOrderDetails(string orderID, string from = "")
        {
            CartDetails orderDetails = new CartDetails();

            var responseTask = hc.GetAsync("API/GetOrderDetailsByOrderID/" + orderID);
            responseTask.Wait();
            HttpResponseMessage result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<CartDetails>();
                readResult.Wait();
                orderDetails = readResult.Result;
                ViewBag.From = from;
            }

            return View(orderDetails);
        }

        public ActionResult PromoteYourProducts()
        {
            return View();
        }

        public List<ProductDetails> GetProductDetails(string userType, string type)
        {
            var listProducts = new List<ProductDetails>();

            var userName = "";
            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }
            else
            {
                userName = "notlogged";
            }

            var responseTask = hc.GetAsync("API/GetProductDetails/" + userType + "/" + userName + "/" + type);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<ProductDetails>>();
                readResult.Wait();
                listProducts = readResult.Result;
            }
            return listProducts;
        }

        public ProductDetails GetProductDetailsbyID(int id)
        {
            var Product = new ProductDetails();

            var responseTask = hc.GetAsync("API/GetProductDetailsByID/" + id);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<ProductDetails>();
                readResult.Wait();
                Product = readResult.Result;
            }
            return Product;
        }

        public List<CommonDropDown> GetCategoryList()
        {
            var listCat = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetCategory");
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

        public List<CommonDropDown> GetOrderStatusList()
        {
            var listOrdStatusList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/Common/GetOrderStatusList");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                listOrdStatusList = readResult.Result;
            }
            return listOrdStatusList;
        }

        public ActionResult AddReview(string orderID, int pID, string pName)
        {
            ProductReview obj = new ProductReview
            {
                CreatedDate = DateTime.Now,
                OrderID = new Guid(orderID),
                ProductID = pID,
                Rating = 0,
                PName = pName
            };

            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult AddReview(ProductReview obj, HttpPostedFileBase imgFile)
        {
            obj.UserID = User.Identity.Name;

            string path = "";
            Random random = new Random();
            int rnd = random.Next(1000, 9999);
            if (imgFile != null && imgFile.ContentLength > 0)
            {
                string ext = Path.GetExtension(imgFile.FileName);
                {
                    if (ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".jpeg") || ext.ToLower().Equals(".png"))
                    {
                        try
                        {
                            string tempFileName =obj.PName.Replace(" ", "") + "-" + rnd + ext;
                            path = Path.Combine(Server.MapPath(constants.ProductReviewImagePath), tempFileName);
                            imgFile.SaveAs(path);
                            path = constants.ProductReviewImagePath + "/" + tempFileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Some Error Occured While Saving the Image.( " + ex.Message + " )");
                            ViewBag.CategoryList = GetCategoryList();
                            return View(obj);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only .JPG, .JPEG, .PNG format is allowed.");
                        ViewBag.CategoryList = GetCategoryList();
                        return View(obj);
                    }
                }
            }

            obj.ImgPath = path;

            var responseTask = hc.PostAsJsonAsync<ProductReview>("API/AddReview", obj);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Product Review Added Succesfully..!";

            }
            else
            {
                ViewBag.Message = "Product Review Additon Failed.!";
            }
            return RedirectToAction("BuyerOrderDetails", "Products", new { orderID = obj.OrderID.ToString() });
        }
    }
}