
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
    
public partial class tblTheaterDetail
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblTheaterDetail()
    {

        this.tblScreenDetails = new HashSet<tblScreenDetail>();

    }


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

    public string ImgPath { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblScreenDetail> tblScreenDetails { get; set; }

}

}
