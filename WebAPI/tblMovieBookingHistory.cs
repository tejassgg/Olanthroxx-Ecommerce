
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace WebAPI
{

using System;
    using System.Collections.Generic;
    
public partial class tblMovieBookingHistory
{

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

    public string QRCodeImageData { get; set; }



    public virtual tblMovieScreenTimingConfig tblMovieScreenTimingConfig { get; set; }

}

}
