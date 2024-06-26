
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
    
public partial class tblMovieDetail
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblMovieDetail()
    {

        this.tblMovieScreenTimingConfigs = new HashSet<tblMovieScreenTimingConfig>();

    }


    public int MovieID { get; set; }

    public string MovieName { get; set; }

    public int Duration { get; set; }

    public string StarCast { get; set; }

    public string Description { get; set; }

    public string ImgPath { get; set; }

    public bool IsActive { get; set; }

    public bool IsOutOfTheater { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }

    public Nullable<System.DateTime> ReleaseDate { get; set; }

    public string BGImgPath { get; set; }

    public string Language { get; set; }

    public string MovieType { get; set; }

    public Nullable<bool> IsAdult { get; set; }

    public string TrailerLink { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblMovieScreenTimingConfig> tblMovieScreenTimingConfigs { get; set; }

}

}
