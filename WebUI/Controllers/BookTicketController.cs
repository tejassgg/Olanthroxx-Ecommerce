using Models.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAPI.Helpers;

namespace WebUI.Controllers
{
    public class BookTicketController : Controller
    {
        readonly Constants constants = new Constants();
        HttpClient hc = new HttpClient
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"].ToString())
        };

        public ActionResult Index()
        {
            List<MovieDetails> objList = new List<MovieDetails>();
            objList = GetMovieDetailsList();
            return View(objList);
        }

        public ActionResult MovieDetails(string MovieName)
        {
            var MovieDetails = new MovieDetails();

            var responseTask = hc.GetAsync("API/BookTicket/GetMovieDetails/" + MovieName);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<MovieDetails>();
                readResult.Wait();
                MovieDetails = readResult.Result;
            }
            return View(MovieDetails);
        }

        public ActionResult BookTicket(int MovieID)
        {
            var MovieDetails = new MovieDetails();

            MovieDetails = GetMovieDetailsList().Where(a => a.MovieID == MovieID).FirstOrDefault();

            return View(MovieDetails);
        }

        public ActionResult SelectSeats(int ScreenTimingConfigID )
        {
            MovieBooking movieBooking = new MovieBooking();
            movieBooking.ScreenTimingConfigID_FK = ScreenTimingConfigID;
            movieBooking.screenDetail = GetScreenDetails(ScreenTimingConfigID);
            movieBooking.lstSeatCategory = new List<CommonDropDown>();
            movieBooking.lstSeatCategory = GetSeatCategory();
            movieBooking.hdnAlreadySelectedSeats = GetAlreadySelectedSeatsForScreenConfigID(movieBooking.ScreenTimingConfigID_FK);

            return PartialView(movieBooking);
        }

        public List<CommonDropDown> GetTimings()
        {
            var timingList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/BookTicket/GetTimings");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                timingList = readResult.Result;
            }
            return timingList;
        }

        public List<MovieScreenTimingConfig> GetMovieScreenTimingConfig(int MovieID)
        {
            List<MovieScreenTimingConfig> lstMovieScreenTimingConfig = new List<MovieScreenTimingConfig>();

            var responseTask = hc.GetAsync("API/BookTicket/GetMovieScreenTimingConfig/" + MovieID);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<MovieScreenTimingConfig>>();
                readResult.Wait();
                lstMovieScreenTimingConfig = readResult.Result;
            }
            return lstMovieScreenTimingConfig;

        }

        public List<MovieDetails> GetMovieDetailsList()
        {
            var MovieList = new List<MovieDetails>();

            var responseTask = hc.GetAsync("API/BookTicket/GetMovieDetailsList");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<MovieDetails>>();
                readResult.Wait();
                MovieList = readResult.Result;
            }
            return MovieList;
        }

        public List<CommonDropDown> GetSeatCategory()
        {
            var timingList = new List<CommonDropDown>();

            var responseTask = hc.GetAsync("API/BookTicket/GetSeatCategory");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<List<CommonDropDown>>();
                readResult.Wait();
                timingList = readResult.Result;
            }
            return timingList;
        }

        public ScreenDetail GetScreenDetails(int ScreenTimingConfigID)
        {
            ScreenDetail screenDetail = new ScreenDetail();

            var responseTask = hc.GetAsync("API/BookTicket/GetScreenDetails/" + ScreenTimingConfigID);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<ScreenDetail>();
                readResult.Wait();
                screenDetail = readResult.Result;
            }

            return screenDetail;
        }

        public string GetAlreadySelectedSeatsForScreenConfigID(int ScreenConfigID)
        {
            string hdnAlreadySelectedSeats = "";
            var responseTask = hc.GetAsync("API/BookTicket/GetAlreadySelectedSeatsForScreenConfigID/" + ScreenConfigID);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsStringAsync();
                readResult.Wait();
                hdnAlreadySelectedSeats = readResult.Result;
            }

            return hdnAlreadySelectedSeats;
        }

        public ActionResult TheatreDetails(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                TheaterDetails theaterDetail = new TheaterDetails();

                var responseTask = hc.GetAsync("API/BookTicket/GetTheatreDetailsByID/" + id);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readResult = result.Content.ReadAsAsync<TheaterDetails>();
                    readResult.Wait();
                    theaterDetail = readResult.Result;
                }

                return View("TheatreDetails", theaterDetail);
            }
            else
            {
                List<TheaterDetails> listTheatreDetail = new List<TheaterDetails>();

                var responseTask = hc.GetAsync("API/BookTicket/GetUsersTheatreDetails/" + User.Identity.Name);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readResult = result.Content.ReadAsAsync<List<TheaterDetails>>();
                    readResult.Wait();
                    listTheatreDetail = readResult.Result;
                }

                return View("UsersTheatreDetails", listTheatreDetail);
            }
        }

        public ActionResult AddTheatreDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTheatreDetails(TheaterDetails obj, HttpPostedFileBase imgFile)
        {
            obj.UserName_FK = User.Identity.Name;

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
                            string tempFileName = User.Identity.Name + "-" + obj.Name.Replace(" ", "") + "-" + rnd + ext;
                            path = Path.Combine(Server.MapPath(constants.TheatreImagePath), tempFileName);
                            imgFile.SaveAs(path);
                            path = constants.TheatreImagePath + "/" + tempFileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Some Error Occured While Saving the Image.( " + ex.Message + " )");
                            return View(obj);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only .JPG, .JPEG, .PNG format is allowed.");
                        return View(obj);
                    }
                }
            }

            obj.ImgPath = path;

            var responseTask = hc.PostAsJsonAsync<TheaterDetails>("API/BookTicket/AddTheatreDetails", obj);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Theatre Added Succesfully..!";
                ViewBag.Link = constants.MyTheatreURL;
                ViewBag.ButtonName = "Close";
                return View("CommonValidationPrinter");
            }

            ViewBag.Message = "Theatre Additon Failed.!";
            return View();
        }

        public ActionResult ScreenDetails(int id)
        {
            ScreenDetail screen = new ScreenDetail();

            var responseTask = hc.GetAsync("API/BookTicket/GetScreenDetailByID/" + id.ToString());
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readResult = result.Content.ReadAsAsync<ScreenDetail>();
                readResult.Wait();
                screen = readResult.Result;
            }

            return View("ScreenDetails", screen);
        }

        public ActionResult AddMovieToScreen(int id)
        {
            return View();
        }

        public ActionResult AddNewScreen(int id)
        {
            var screenDetail = new ScreenDetail();
            screenDetail.TheaterID_FK = id;
            return View(screenDetail);
        }

        [HttpPost]
        public ActionResult AddNewScreen(ScreenDetail obj)
        {
            var responseTask = hc.PostAsJsonAsync<ScreenDetail>("API/BookTicket/AddNewScreen", obj);
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                ViewBag.Message = "Screen Added Succesfully..!";
                ViewBag.Link = constants.MyTheatreURL + "/" + obj.TheaterID_FK;
                ViewBag.ButtonName = "Close";
                return View("CommonValidationPrinter");
            }
            else
            {
                ModelState.AddModelError("", "Screen Additon Failed. Due to " + result.ReasonPhrase);
                return View();
            }
        }


    }
}