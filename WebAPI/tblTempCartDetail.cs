
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
    
public partial class tblTempCartDetail
{

    public int CartID { get; set; }

    public int AccountID_FK { get; set; }

    public string TempCartDetails { get; set; }

    public string SessionID { get; set; }

    public bool IsUsed { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public Nullable<System.DateTime> UsedDate { get; set; }



    public virtual tblUser tblUser { get; set; }

}

}
