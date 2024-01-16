using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class MovieDetails
    {
        public int MovieID { get; set; }
        public string ScreenID_FK { get; set; }
        public string MovieName { get; set; }
        public int Duration { get; set; }
        public string StarCast { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public bool IsActive { get; set; }
        public bool IsOutOfTheater { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string BGImgPath { get; set; }
        public string Language { get; set; }
        public string MovieType { get; set; }
        public bool IsAdult { get; set; }

        public List<MovieScreenTimingConfig> MovieScreenTimingConfigs { get; set; }

    }

    public class MovieScreenTimingConfig
    {
        public int ScreenTimingConfigID { get; set; }
        public int MovieID_FK { get; set; }
        public int ScreenID_FK { get; set; }
        public string TimingFrom { get; set; }
        public int TimingID { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int TheaterID_FK { get; set; }
        public string TheaterName { get; set; }
        public string TheaterLandmark { get; set; }
        public Nullable<System.DateTime> MovieDate { get; set; }
    }

    public class ScreenDetail
    {
        public int ScreenID { get; set; }
        public int ScreenNo { get; set; }
        public int TheaterID_FK { get; set; }
        public int NoOfSeatsSilver { get; set; }
        public int PriceSilver { get; set; }
        public int NoOfSeatsGold { get; set; }
        public int PriceGold { get; set; }
        public int NoOfSeatsPlatinum { get; set; }
        public int PricePlatinum { get; set; }
        public int NoOfSeatsRecliner { get; set; }
        public int PriceRecliner { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class TheaterDetails
    {
        public int TheaterID { get; set; }
        public string UserName_FK { get; set; }
        public string Name { get; set; }
        public int NoOfScreens { get; set; }
        public int Pincode { get; set; }
        public string Location { get; set; }
        public string Landmark { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    public class MovieBooking
    {
        public ScreenDetail screenDetail { get; set; }
        public System.Guid BookingID { get; set; }
        public int ScreenTimingConfigID_FK { get; set; }
        public string MovieName { get; set; }
        public int NoOfSeats { get; set; }
        public string SeatNo { get; set; }
        public Nullable<int> SeatCategory { get; set; }
        public int Amount { get; set; }
        public string UserName { get; set; }
        public bool IsUsed { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public List<CommonDropDown> lstSeatCategory { get; set; }
        public string hdnAlreadySelectedSeats {get; set;}
    }
}
