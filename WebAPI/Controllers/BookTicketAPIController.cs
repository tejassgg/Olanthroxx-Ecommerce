using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class BookTicketAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();

        [HttpGet]
        [Route("API/BookTicket/GetTimings")]
        public List<CommonDropDown> GetTimings()
        {
            var paymentModes = entities.tblMasCommonTypes.Where(a => a.MasterType == "MovieTimingFrom" && a.IsActive == true).
            Select(x => new CommonDropDown
            {
                Text = x.Description,
                Value = x.Code,
            }).ToList();

            return paymentModes;
        }

        [HttpGet]
        [Route("API/BookTicket/GetMovieScreenTimingConfig/{MovieID}")]
        public List<MovieScreenTimingConfig> GetMovieScreenTimingConfig(int MovieID)
        {
            List<MovieScreenTimingConfig> lstMovie = new List<MovieScreenTimingConfig>();

            lstMovie = (from obj in entities.tblMovieScreenTimingConfigs
                        join obj2 in entities.tblMasCommonTypes on obj.TimingID equals obj2.Code
                        join obj3 in entities.tblScreenDetails on obj.ScreenID_FK equals obj3.ScreenID
                        join obj4 in entities.tblTheaterDetails on obj3.TheaterID_FK equals obj4.TheaterID
                        where obj.MovieID_FK == MovieID && obj.IsActive == true && obj2.MasterType == "MovieTimingFrom" && obj.MovieDate >= DateTime.Now
                        select new MovieScreenTimingConfig
                        {
                            ScreenTimingConfigID = obj.ScreenTimingConfigID,
                            ScreenID_FK = obj.ScreenID_FK,
                            MovieID_FK = obj.MovieID_FK,
                            TimingFrom = obj2.Description,
                            TimingID = obj.TimingID,
                            CreatedDate = obj.CreatedDate,
                            IsActive = true,
                            ModifiedDate = obj.ModifiedDate,
                            TheaterID_FK = obj4.TheaterID,
                            TheaterName = obj4.Name,
                            TheaterLandmark = obj4.Landmark,
                            MovieDate = obj.MovieDate
                        }).ToList();

            return lstMovie;
        }

        [HttpGet]
        [Route("API/BookTicket/GetMovieDetailsList")]
        public List<MovieDetails> GetMovieDetailsList()
        {
            List<MovieDetails> lstMovieDetails = new List<MovieDetails>();

            lstMovieDetails = (from obj in entities.tblMovieDetails
                               where obj.IsActive == true
                               select new MovieDetails
                               {
                                   MovieID = obj.MovieID,
                                   MovieName = obj.MovieName,
                                   Duration = obj.Duration,
                                   StarCast = obj.StarCast,
                                   Description = obj.Description,
                                   ImgPath = obj.ImgPath,
                                   IsOutOfTheater = obj.IsOutOfTheater,
                                   IsActive = true,
                                   BGImgPath = obj.BGImgPath,
                                   Language = obj.Language,
                                   MovieType = obj.MovieType,
                                   IsAdult = obj.IsAdult == true ? true : false
                               }).ToList();
            
            if(lstMovieDetails != null && lstMovieDetails.Count > 0)
            {
                foreach (var mov in lstMovieDetails)
                {
                    mov.MovieScreenTimingConfigs = new List<MovieScreenTimingConfig>();
                    mov.MovieScreenTimingConfigs = GetMovieScreenTimingConfig(mov.MovieID);
                }
            }

            return lstMovieDetails;
        }

        [HttpGet]
        [Route("API/BookTicket/GetMovieDetails/{MovieName}")]
        public IHttpActionResult GetMovieDetails(string MovieName)
        {
            MovieDetails movieDetails = new MovieDetails();
            movieDetails = GetMovieDetailsList().Where(a => a.MovieName == MovieName).FirstOrDefault();
            if(movieDetails != null)
            {
                return Ok(movieDetails);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("API/BookTicket/GetSeatCategory")]
        public List<CommonDropDown> GetSeatCategory()
        {
            var paymentModes = entities.tblMasCommonTypes.Where(a => a.MasterType == "SeatCategory" && a.IsActive == true).
            Select(x => new CommonDropDown
            {
                Text = x.Description,
                Value = x.Code,
            }).ToList();

            return paymentModes;
        }

        [HttpGet]
        [Route("API/BookTicket/GetScreenDetails/{ScreenTimingConfigID}")]
        public IHttpActionResult GetScreenDetails(int ScreenTimingConfigID)
        {
            ScreenDetail screenDetail = new ScreenDetail();

            screenDetail = (from obj in entities.tblMovieScreenTimingConfigs
                            join obj2 in entities.tblScreenDetails on obj.ScreenID_FK equals obj2.ScreenID
                            where obj.ScreenTimingConfigID == ScreenTimingConfigID
                            select new ScreenDetail
                            {
                                ScreenID = obj.ScreenID_FK,
                                ScreenNo = obj2.ScreenNo,
                                TheaterID_FK = obj2.TheaterID_FK,
                                NoOfSeatsSilver = obj2.NoOfSeatsSilver,
                                PriceSilver = obj2.PriceSilver,
                                NoOfSeatsGold = obj2.NoOfSeatsGold,
                                PriceGold = obj2.PriceGold,
                                NoOfSeatsPlatinum = obj2.NoOfSeatsPlatinum,
                                PricePlatinum = obj2.PricePlatinum,
                                NoOfSeatsRecliner = obj2.NoOfSeatsRecliner,
                                PriceRecliner = obj2.PriceRecliner,
                            }).FirstOrDefault();

            if(screenDetail != null)
            {
                return Ok(screenDetail);
            }

            return BadRequest();
            
        }

        [HttpGet]
        [Route("API/BookTicket/GetAlreadySelectedSeatsForScreenConfigID/{ScreenConfigID}")]
        public IHttpActionResult GetAlreadySelectedSeatsForScreenConfigID(int ScreenConfigID)
        {
            string hdnAlreadySelectedSeats = "";
            if (ScreenConfigID > 0)
            {
                var SelectedSeatData = entities.tblMovieBookingHistories.Where(a => a.ScreenTimingConfigID_FK == ScreenConfigID).ToList();
                if(SelectedSeatData != null && SelectedSeatData.Count() >0)
                {
                    foreach(var seat in SelectedSeatData)
                    {
                        hdnAlreadySelectedSeats += (seat.SeatNo + ",");
                    }
                }
            }
            return Ok(hdnAlreadySelectedSeats);
        }

        [HttpPost]
        [Route("API/BookTicket/GetCalculateMovieTicketCost")]
        public MovieBooking GetCalculateMovieTicketCost(MovieBooking movie)
        {
            int TicketPrice = 0;
            var ScreenConfig = entities.tblMovieScreenTimingConfigs.Where(a => a.ScreenTimingConfigID == movie.ScreenTimingConfigID_FK).FirstOrDefault();
            if (ScreenConfig != null)
            {
                switch (movie.SeatCategory)
                {
                    case 101:
                        TicketPrice = entities.tblScreenDetails.Where(a => a.ScreenID == ScreenConfig.ScreenID_FK).Select(a => a.PriceSilver).FirstOrDefault();
                        break;
                    case 102:
                        TicketPrice = entities.tblScreenDetails.Where(a => a.ScreenID == ScreenConfig.ScreenID_FK).Select(a => a.PriceGold).FirstOrDefault();
                        break;
                    case 103:
                        TicketPrice = entities.tblScreenDetails.Where(a => a.ScreenID == ScreenConfig.ScreenID_FK).Select(a => a.PricePlatinum).FirstOrDefault();
                        break;
                    case 104:
                        TicketPrice = entities.tblScreenDetails.Where(a => a.ScreenID == ScreenConfig.ScreenID_FK).Select(a => a.PriceRecliner).FirstOrDefault();
                        break;
                }
                movie.MovieName = entities.tblMovieDetails.Where(a => a.MovieID == ScreenConfig.MovieID_FK).Select(a => a.MovieName).FirstOrDefault();
            }

            movie.Amount = movie.NoOfSeats * (TicketPrice);

            return movie;
        }

        [HttpGet]
        [Route("API/BookTicket/GetUsersTheatreDetails/{UserID}")]
        public List<TheaterDetails> GetUsersTheatreDetails(string UserID)
        {
            var theatreDetails = new List<TheaterDetails>();
            if (!string.IsNullOrEmpty(UserID))
            {
                theatreDetails = (from obj in entities.tblTheaterDetails
                                  where obj.UserName_FK == UserID
                                  select new TheaterDetails
                                  {
                                      TheaterID  = obj.TheaterID,
                                      UserName_FK = obj.UserName_FK,
                                      Name = obj.Name,
                                      NoOfScreens =obj.NoOfScreens,
                                      Pincode = obj.Pincode,
                                      Location = obj.Location,
                                      Landmark = obj.Landmark,
                                      IsActive = obj.IsActive,
                                      ImgPath = obj.ImgPath
                                  }).ToList();
            }
            return theatreDetails;
        }

        [HttpGet]
        [Route("API/BookTicket/GetTheatreDetailsByID/{TheatreID}")]
        public TheaterDetails GetTheatreDetailsByID(int TheatreID)
        {
            var theatreDetail = new TheaterDetails();
            if (!string.IsNullOrEmpty(TheatreID.ToString()))
            {
                theatreDetail = (from obj in entities.tblTheaterDetails
                                  where obj.TheaterID == TheatreID && obj.IsActive == true
                                  select new TheaterDetails
                                  {
                                      TheaterID = obj.TheaterID,
                                      UserName_FK = obj.UserName_FK,
                                      Name = obj.Name,
                                      NoOfScreens = obj.NoOfScreens,
                                      Pincode = obj.Pincode,
                                      Location = obj.Location,
                                      Landmark = obj.Landmark,
                                      IsActive = obj.IsActive,
                                      ImgPath = obj.ImgPath,
                                  }).ToList().FirstOrDefault();
                if (theatreDetail != null)
                {
                    theatreDetail.lstScreens = (from obj in entities.tblScreenDetails
                                                where obj.TheaterID_FK == TheatreID
                                                select new ScreenDetail
                                                {
                                                    ScreenID = obj.ScreenID,
                                                    ScreenNo = obj.ScreenNo,
                                                    NoOfSeatsSilver = obj.NoOfSeatsSilver,
                                                    PriceSilver = obj.PriceSilver,
                                                    NoOfSeatsGold = obj.NoOfSeatsGold,
                                                    PriceGold = obj.PriceGold,
                                                    NoOfSeatsPlatinum = obj.NoOfSeatsPlatinum,
                                                    PricePlatinum = obj.PricePlatinum,
                                                    NoOfSeatsRecliner = obj.NoOfSeatsRecliner,
                                                    PriceRecliner = obj.PriceRecliner,
                                                }).ToList();
                }
            }
            
            return theatreDetail;
        }

        [HttpPost]
        [Route("API/BookTicket/AddTheatreDetails")]
        public IHttpActionResult AddTheatreDetails(TheaterDetails obj)
        {
            var dbObj = new tblTheaterDetail
            {
                UserName_FK = obj.UserName_FK,
                Name = obj.Name,
                IsActive = true,
                Landmark = obj.Landmark,
                Location = obj.Location,
                NoOfScreens = obj.NoOfScreens,
                CreatedDate = DateTime.Now,
                Pincode = obj.Pincode,
                ImgPath = obj.ImgPath,
            };
            entities.tblTheaterDetails.Add(dbObj);

            int success = entities.SaveChanges();
            if (success > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("API/BookTicket/AddNewScreen")]
        public IHttpActionResult AddNewScreen(ScreenDetail obj)
        {
            int ScreenNo = 1;
            var data = entities.tblScreenDetails.Where(a => a.TheaterID_FK == obj.TheaterID_FK).OrderByDescending(a=>a.ScreenNo).FirstOrDefault();
            if( data != null)
            {
                ScreenNo = data.ScreenNo + 1;
            }
            var dbObj = new tblScreenDetail
            {
                TheaterID_FK = obj.TheaterID_FK,
                ScreenNo = ScreenNo,
                NoOfSeatsSilver = obj.NoOfSeatsSilver,
                PriceSilver = obj.PriceSilver,
                NoOfSeatsGold = obj.NoOfSeatsGold,
                PriceGold = obj.PriceGold,
                NoOfSeatsPlatinum = obj.NoOfSeatsPlatinum,
                PricePlatinum = obj.PricePlatinum,
                NoOfSeatsRecliner = obj.NoOfSeatsRecliner,
                PriceRecliner = obj.PriceRecliner,
                CreatedDate = DateTime.Now,
            };
            entities.tblScreenDetails.Add(dbObj);

            int success = entities.SaveChanges();
            if (success > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("API/BookTicket/GetScreenDetailByID/{ScreenID}")]
        public ScreenDetail GetScreenDetailByID(int ScreenID)
        {
            ScreenDetail screen = new ScreenDetail();
            screen = (from obj in entities.tblScreenDetails
                      join obj1 in entities.tblTheaterDetails on obj.TheaterID_FK equals obj1.TheaterID
                      where obj.ScreenID == ScreenID
                      select new ScreenDetail
                      {
                          ScreenID = obj.ScreenID,
                          ScreenNo = obj.ScreenNo,
                          NoOfSeatsSilver = obj.NoOfSeatsSilver,
                          PriceSilver = obj.PriceSilver,
                          NoOfSeatsGold = obj.NoOfSeatsGold,
                          PriceGold = obj.PriceGold,
                          NoOfSeatsPlatinum = obj.NoOfSeatsPlatinum,
                          PricePlatinum = obj.PricePlatinum,
                          NoOfSeatsRecliner = obj.NoOfSeatsRecliner,
                          PriceRecliner = obj.PriceRecliner,
                          TheatreName = obj1.Name
                      }).ToList().FirstOrDefault();
            if (screen != null)
            {
                screen.lstMovieDetails = new List<MovieDetails>();
                screen.lstMovieDetails = GetMovieDetailsList();
                screen.lstMovieScreenTimingConfig = new List<MovieScreenTimingConfig>();

                if (screen.lstMovieDetails != null && screen.lstMovieDetails.Count() > 0)
                {
                    foreach(var item in screen.lstMovieDetails)
                    {
                        item.MovieScreenTimingConfigs = new List<MovieScreenTimingConfig>();
                        item.MovieScreenTimingConfigs = GetMovieScreenTimingConfig(item.MovieID);
                        screen.lstMovieScreenTimingConfig.AddRange(item.MovieScreenTimingConfigs);
                    }
                }
                screen.lstMovieScreenTimingConfig = screen.lstMovieScreenTimingConfig.Where(a => a.ScreenID_FK == ScreenID).ToList();
            }

                
            return screen;
        }

    }
}
