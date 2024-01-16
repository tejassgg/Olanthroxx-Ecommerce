using Models.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
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

    }
}