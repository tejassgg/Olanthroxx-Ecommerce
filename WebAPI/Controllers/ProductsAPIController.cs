using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Models.DTO;
using Newtonsoft.Json;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    public class ProductsAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();
        AccountAPIController accountAPIController = new AccountAPIController();
        Constants constants = new Constants();

        [HttpGet]
        [Route("API/GetProductDetails/{userType}/{userName}/{type}")]
        public IHttpActionResult GetProductDetails(string userType, string userName, string type)
        {
            var list = new List<tblProduct>();

            try
            {
                if (type == "index")
                {
                    if (userType == "seller")
                    {
                        list = entities.tblProducts.OrderByDescending(a => a.ProductID).Where(a => a.SellerName == userName).ToList();
                    }
                    else if (userType == "admin" || userType == "botlogin")
                    {
                        list = entities.tblProducts.OrderByDescending(a => a.ProductID).ToList();
                    }
                }
                else if (type == "shop")
                {
                    if (userType == "botlogin")
                    {
                        list = entities.tblProducts.Where(a => a.SellerName != userName).OrderByDescending(a => a.ProductID).ToList();
                    }
                    else if (userType == "buyer" || userType == "notlogged")
                    {
                        list = entities.tblProducts.OrderByDescending(a => a.ProductID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("DB Error:- " + ex.Message + " " + ex.StackTrace);
            }

            List<ProductDetails> productDetails = new List<ProductDetails>();

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var obj = new ProductDetails();
                    var catType = entities.tblCategories.Where(a => a.CategoryID == item.CategoryID_FK).Select(a => a.CategoryType).FirstOrDefault();
                    obj.ProductID = item.ProductID;
                    obj.PName = item.PName;
                    obj.PTitle = item.PTitle;
                    obj.PDescription = item.PDescription;
                    obj.CategoryID_FK = item.CategoryID_FK;
                    obj.Category = catType;
                    obj.Quantity = item.Quantity;
                    obj.Price = item.Price;
                    obj.SellerName = item.SellerName;
                    obj.ImgPath = item.ImgPath;

                    var ratingData = entities.tblProductReviews.Where(a => a.ProductID_FK == obj.ProductID).ToList();
                    if (ratingData != null && ratingData.Count > 0)
                    {
                        var sumRating = 0;
                        for (var i = 0; i < ratingData.Count; i++)
                        {
                            sumRating += ratingData[i].Rating;
                        }

                        obj.OverallRating = Convert.ToDouble(sumRating) / Convert.ToDouble(ratingData.Count);
                    }
                    else
                        obj.OverallRating = 0;

                    productDetails.Add(obj);
                }
            }
            return Ok(productDetails);
        }

        [HttpGet]
        [Route("API/GetProductDetailsByID/{id}")]
        public IHttpActionResult GetProductDetailsByID(int id)
        {
            var item = entities.tblProducts.Where(a => a.ProductID == id).FirstOrDefault();
            ProductDetails obj = new ProductDetails();
            if (item != null)
            {
                var catType = entities.tblCategories.Where(a => a.CategoryID == item.CategoryID_FK).Select(a => a.CategoryType).FirstOrDefault();
                obj.ProductID = item.ProductID;
                obj.PName = item.PName;
                obj.PTitle = item.PTitle;
                obj.PDescription = item.PDescription;
                obj.CategoryID_FK = item.CategoryID_FK;
                obj.Category = catType;
                obj.Quantity = item.Quantity;
                obj.Price = item.Price;
                obj.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                obj.ModifiedDate = Convert.ToDateTime(item.ModifiedDate);
                obj.SellerName = item.SellerName;
                obj.ImgPath = item.ImgPath;

                obj.lstProductReviews = new List<ProductReview>();
                obj.lstProductReviews = (from rev in entities.tblProductReviews
                                         join user in entities.tblUsers on rev.UserID equals user.UserName
                                         join userD in entities.tblUserDetails on user.AccountID equals userD.AccountID_FK
                                         where rev.ProductID_FK == id
                                         select new ProductReview
                                         {
                                             OrderID = rev.OrderID_FK,
                                             ProductID = rev.ProductID_FK,
                                             Review = rev.Review,
                                             Rating = rev.Rating,
                                             ImgPath = rev.ImgPath,
                                             UserID = rev.UserID,
                                             ReviewerName = userD.FirstName + " "+ userD.LastName,
                                             CreatedDate = rev.CreatedDate,
                                         }).ToList();

                if(obj.lstProductReviews != null && obj.lstProductReviews.Count > 0)
                {
                    var sumRating = 0;
                    for (var i = 0; i < obj.lstProductReviews.Count; i++)
                    {
                        sumRating += obj.lstProductReviews[i].Rating;
                    }

                    obj.OverallRating = Convert.ToDouble(sumRating) / Convert.ToDouble(obj.lstProductReviews.Count);
                }

                obj.LstOtherProducts = new List<ProductDetails>();
                obj.LstOtherProducts = (from prod in entities.tblProducts
                                        join obj1 in entities.tblCategories on prod.CategoryID_FK equals obj1.CategoryID
                                        where prod.SellerName == obj.SellerName && prod.ProductID != id
                                        select new ProductDetails
                                        {
                                            ProductID = prod.ProductID,
                                            PName = prod.PName,
                                            PTitle = prod.PTitle,
                                            PDescription = prod.PDescription,
                                            CategoryID_FK = prod.CategoryID_FK,
                                            Category = obj1.CategoryType,
                                            Quantity = prod.Quantity,
                                            Price = prod.Price,
                                            CreatedDate = prod.CreatedDate,
                                            ModifiedDate =prod.ModifiedDate,
                                            SellerName = prod.SellerName,
                                            ImgPath = prod.ImgPath,
                                        }).ToList();

                if(obj.LstOtherProducts != null && obj.LstOtherProducts.Count > 0)
                {
                    foreach (var prod in obj.LstOtherProducts)
                    {
                        var ratingData = entities.tblProductReviews.Where(a => a.ProductID_FK == prod.ProductID).ToList();
                        if (ratingData != null && ratingData.Count > 0)
                        {
                            var sumRating = 0;
                            for (var i = 0; i < ratingData.Count; i++)
                            {
                                sumRating += ratingData[i].Rating;
                            }

                            prod.OverallRating = Convert.ToDouble(sumRating) / Convert.ToDouble(ratingData.Count);
                        }
                        else
                            prod.OverallRating = 0;
                    }
                }

                return Ok(obj);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("API/AddProduct")]
        public IHttpActionResult AddProduct(ProductDetails obj)
        {
            var dbObj = new tblProduct
            {
                PName = obj.PName,
                PTitle = obj.PTitle,
                PDescription = obj.PDescription,
                CategoryID_FK = obj.CategoryID_FK,
                Quantity = obj.Quantity,
                Price = obj.Price,
                CreatedDate = DateTime.Now,
                SellerName = obj.SellerName,
                ImgPath = obj.ImgPath,
            };
            entities.tblProducts.Add(dbObj);
            int success = entities.SaveChanges();
            if (success > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("API/UpdateProduct")]
        public IHttpActionResult UpdateProduct(ProductDetails obj)
        {
            var prevObj = entities.tblProducts.Where(a => a.ProductID == obj.ProductID).FirstOrDefault();

            if (prevObj != null)
            {
                prevObj.PName = obj.PName;
                prevObj.PTitle = obj.PTitle;
                prevObj.PDescription = obj.PDescription;
                prevObj.CategoryID_FK = obj.CategoryID_FK;
                prevObj.Quantity = obj.Quantity;
                prevObj.Price = obj.Price;
                prevObj.ModifiedDate = DateTime.Now;
                prevObj.ImgPath = obj.ImgPath;

                int success = entities.SaveChanges();
                if (success > 0)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("API/DeleteProduct/{id}/{username}")]
        public IHttpActionResult DeleteProduct(int id, string username)
        {
            var User = entities.tblUsers.Where(a=>a.UserName == username).FirstOrDefault();
            var item = new tblProduct();
            if (User != null && (User.LoginType == constants.BotLoginType || User.LoginType != "Buyer") )
            {
                if(User.LoginType == "Buyer") { return BadRequest("You are Not a Seller nor the Product Owner. So, you cannot delete the product."); }
                if (User.LoginType == constants.BotLoginType)
                {
                    item = entities.tblProducts.Where(a => a.ProductID == id).FirstOrDefault();
                }
                else
                {
                    item = entities.tblProducts.Where(a => a.ProductID == id && a.SellerName == username).FirstOrDefault();
                }

                if (item != null)
                {
                    entities.tblProducts.Remove(item);
                    int success = entities.SaveChanges();
                    if (success > 0)
                        return Ok("Product Deleted Successfully");
                    return BadRequest("Some Error Occurred While Processing Your Request. Please Contact System Administrator..!!");
                }

            }            
            return BadRequest();
        }

        [HttpGet]
        [Route("API/GetUserDetailsByUserID/{username}")]
        public IHttpActionResult GetUserDetailsByUserID(string username)
        {
            string TempOrderStatus = "";
            LstOrderDetails orders = new LstOrderDetails();

            orders.UserDetails = new UserDetails();
            var accountID = entities.tblUsers.Where(b => b.UserName == username).Select(b => b.AccountID).FirstOrDefault();
            orders.UserDetails = entities.tblUserDetails.Where(a => a.AccountID_FK == accountID).
                Select(x => new UserDetails
                {
                    FirstName = x.FirstName,
                    MidName = x.MidName,
                    LastName = x.LastName,
                    MobileNo = x.MobileNo,
                    AadharNumber = x.AadharNumber,
                    EmailID = x.EmailID,
                    State = x.State,
                    City = x.City,
                    PinCode = x.PinCode,
                    FullAddress = x.FullAddress,
                    CreatedDate = x.CreatedDate
                }).FirstOrDefault();

            orders.CartDetails = new List<CartDetails>();
            orders.UserName = username;
            var dbOrders = entities.tblOrderDetails.Where(a => a.UserID == username).DistinctBy(a=>a.OrderID).ToList();
            foreach(var item in dbOrders)
            {
                CartDetails obj = new CartDetails();
                obj.ModifiedDate = item.ModifiedDate;
                obj.lstOrderDetails = new List<OrderDetails>();
                obj.lstOrderDetails = (from obj2 in entities.tblOrderDetails
                                       join obj1 in entities.tblProducts on obj2.ProductID equals obj1.ProductID
                                       join obj3 in entities.tblMasCommonTypes on obj2.OrderStatus equals obj3.Code
                                       where obj2.OrderID == item.OrderID && obj3.MasterType == "OrderStatus"
                                       select new OrderDetails
                                       {
                                           OrderID = obj2.OrderID,
                                           ProductID = obj2.ProductID,
                                           Amount = obj2.Amount,
                                           Quantity = obj2.Quantity,
                                           OrderDate = obj2.CreatedDate,
                                           ProductName = obj1.PName,
                                           OrderStatus = obj3.Description
                                       }).ToList();

                foreach (var ord in obj.lstOrderDetails)
                {
                    obj.TotalAmount += ord.Amount;
                    if(obj.lstOrderDetails.Count > 1)
                    {
                        if (string.IsNullOrEmpty(TempOrderStatus))
                        {
                            TempOrderStatus = ord.OrderStatus;
                        }
                        else
                        {
                            if (ord.OrderStatus != TempOrderStatus)
                                obj.OrderStatus = "In Progress";
                            else
                                obj.OrderStatus = ord.OrderStatus;
                        }
                    }
                    else
                        obj.OrderStatus = ord.OrderStatus;
                }
                TempOrderStatus = "";
                obj.OrderID = item.OrderID;
                obj.OrderDate = item.CreatedDate;

                orders.CartDetails.Add(obj);
            }
            return Ok(orders);
        }

        [HttpGet]
        [Route("API/GetOrderDetailsByOrderID/{orderid}")]
        public IHttpActionResult GetOrderDetailsByOrderID(Guid orderid)
        {
            CartDetails cartDetail = new CartDetails();
            cartDetail.OrderID = orderid;

            GetOrderDetailsByID(cartDetail);

            return Ok(cartDetail);
        }

        public CartDetails GetOrderDetailsByID(CartDetails cartDetail)
        {
            cartDetail.lstOrderDetails = new List<OrderDetails>();
            cartDetail.lstOrderDetails = (from obj in entities.tblOrderDetails
                                          join obj1 in entities.tblProducts on obj.ProductID equals obj1.ProductID
                                          join obj2 in entities.tblMasCommonTypes on obj.OrderStatus equals obj2.Code
                                          where obj.OrderID == cartDetail.OrderID && obj2.MasterType == "OrderStatus"
                                          select new OrderDetails
                                          {
                                              OrderID = obj.OrderID,
                                              ProductID = obj.ProductID,
                                              Amount = obj.Amount,
                                              Quantity = obj.Quantity,
                                              OrderDate = obj.CreatedDate,
                                              ProductName = obj1.PName,
                                              ModifiedDate = obj.ModifiedDate,
                                              UserID = obj.UserID,
                                              OrderStatus = obj2.Description,
                                              ImgPath = obj1.ImgPath,
                                              SoldBy = obj1.SellerName
                                          }).ToList();

            if (cartDetail.lstOrderDetails != null && cartDetail.lstOrderDetails.Count > 0)
            {
                foreach (var item in cartDetail.lstOrderDetails)
                {
                    if (entities.tblProductReviews.Where(a => a.ProductID_FK == item.ProductID && a.OrderID_FK == item.OrderID).FirstOrDefault() != null)
                    {
                        item.IsReviewed = true;
                    }
                    cartDetail.OrderStatus = item.OrderStatus;
                }

                cartDetail.OrderDate = cartDetail.lstOrderDetails[0].OrderDate;
                if (cartDetail.lstOrderDetails[0].ModifiedDate != null)
                    cartDetail.ModifiedDate = cartDetail.lstOrderDetails[0].ModifiedDate;

                cartDetail.TotalAmount = cartDetail.lstOrderDetails.Sum(a => a.Amount);
            }
            return cartDetail;
        }

        [HttpGet]
        [Route("API/GetSellerDashBoardDetails/{sellerName}")]
        public IHttpActionResult GetSellerDashBoardDetails(string sellerName)
        {
            LstOrderDetailsForSeller SellerOrders = new LstOrderDetailsForSeller();

            try
            {
                var User = entities.tblUsers.Where(a => a.UserName == sellerName).FirstOrDefault();
                if (User != null)
                {
                    if (User.LoginType == "Seller" || User.LoginType == "BotLogin")
                    {
                        SellerOrders.UserDetails = new UserDetails();
                        SellerOrders.UserDetails = entities.tblUserDetails.Where(a => a.AccountID_FK == User.AccountID).
                            Select(x => new UserDetails
                            {
                                FirstName = x.FirstName,
                                MidName = x.MidName,
                                LastName = x.LastName,
                                MobileNo = x.MobileNo,
                                AadharNumber = x.AadharNumber,
                                EmailID = x.EmailID,
                                State = x.State,
                                City = x.City,
                                PinCode = x.PinCode,
                                FullAddress = x.FullAddress,
                                CreatedDate = x.CreatedDate
                            }).FirstOrDefault();

                        SellerOrders.OrderDetails = new List<OrderDetails>();
                        SellerOrders.OrderDetails = (from obj in entities.tblOrderDetails
                                                     join obj2 in entities.tblProducts on obj.ProductID equals obj2.ProductID
                                                     where obj2.SellerName == sellerName
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
                                                         BilllingAddressID = obj.BillingAddressID_FK
                                                     }).OrderByDescending(a => a.OrderStatus).ToList();

                        SellerOrders.TotalSales = SellerOrders.OrderDetails.Count();
                        SellerOrders.TotalSalesQuantity = SellerOrders.OrderDetails.Sum(a => a.Quantity);
                        SellerOrders.TotalSalesAmount = SellerOrders.OrderDetails.Sum(a => a.Amount);
                        SellerOrders.TotalProducts = entities.tblProducts.Where(a => a.SellerName == sellerName).Count();
                        SellerOrders.ActiveUsers = entities.tblUsers.Where(a=>a.IsActive == true).Count();

                        SellerOrders.OrderDetails = SellerOrders.OrderDetails.GroupBy(a => a.OrderID).Select(obj => new OrderDetails
                        {
                            OrderID = obj.Max(a=>a.OrderID),
                            Amount = obj.Sum(a => a.Amount),
                            Quantity = obj.Sum(a => a.Quantity),
                            OrderStatus = obj.Max(a=>a.OrderStatus),
                            OrderDate = obj.Max(a=>a.OrderDate),
                        }).ToList();

                        

                        return Ok(SellerOrders);
                        //return Ok(new { SellerOrders, Data });

                    }
                    else
                    {
                        return BadRequest("User Is Not A Seller.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Some Error Occured While Fetching the Details..!");
                throw;
            }

            return BadRequest("User Not Found..!!");
            
        }

        [HttpPost]
        [Route("API/UpdateSellerOrderDetails")]
        public IHttpActionResult UpdateSellerOrderDetails(CartDetails obj)
        {
            var OrderStatus = Convert.ToInt32(obj.OrderStatus);
            if (obj.lstOrderDetails == null)
            {
                obj.lstOrderDetails = new List<OrderDetails>();
                obj.lstOrderDetails = GetOrderDetailsByID(obj).lstOrderDetails.Where(a=>a.SoldBy == obj.LoggedInUser).ToList();
            }
            if(obj.lstOrderDetails != null & obj.lstOrderDetails.Count > 0)
            {
                foreach(var item in obj.lstOrderDetails)
                {
                    var orderDetails = entities.tblOrderDetails.Where(a => a.OrderID == item.OrderID && a.ProductID == item.ProductID).FirstOrDefault();
                    if (orderDetails == null)
                        return BadRequest("OrderDetailsNotFound..!");

                    orderDetails.OrderStatus = OrderStatus;
                    orderDetails.ModifiedDate = DateTime.Now;
                    if(OrderStatus == 3)  //Out For Delivery
                        orderDetails.OTPToBeShown = true;
                }

                //SendEmailForDeliveryOTP
                if (OrderStatus == 3)  //Out For Delivery
                {
                    var orderDetails = entities.tblOrderDetails.FirstOrDefault(a => a.OrderID == obj.OrderID);
                    EmailDetails objEmail = new EmailDetails();
                    objEmail.UserName = orderDetails.UserID;
                    objEmail.EmailID = entities.tblUserDetails.FirstOrDefault(a => a.AccountID_FK == entities.tblUsers.FirstOrDefault(b => b.UserName == orderDetails.UserID).AccountID).EmailID;
                    objEmail.EmailType = constants.OutForDelivery;
                    objEmail.OrderID = orderDetails.OrderID.ToString();
                    Random generator = new Random();
                    objEmail.OTP = generator.Next(0, 1000000).ToString("D6");
                    orderDetails.OTP = Convert.ToInt32(objEmail.OTP);
                    objEmail.NoOfItems = obj.lstOrderDetails.Sum(a => a.Quantity);
                    accountAPIController.SendEmail(objEmail);
                }
            }

            int success = entities.SaveChanges();

            if (success > 0)
            {
                return Ok("Order Updated Successfully..!!");
            }
            return BadRequest("Some Error Occurred While Updating the Order Details");
        }

        [HttpPost]
        [Route("API/AddReview")]
        public IHttpActionResult AddReview(ProductReview obj)
        {
            var dbObj = new tblProductReview
            {
                ProductID_FK = obj.ProductID,
                OrderID_FK = obj.OrderID,
                Rating = obj.Rating,
                Review = obj.Review,
                CreatedDate = obj.CreatedDate,
                UserID = obj.UserID,
                ImgPath = obj.ImgPath,
            };

            entities.tblProductReviews.Add(dbObj);
            int success = entities.SaveChanges();
            
            if (success > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}