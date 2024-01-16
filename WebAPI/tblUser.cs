
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
    
public partial class tblUser
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblUser()
    {

        this.tblUserDetails = new HashSet<tblUserDetail>();

        this.tblBudgetDetails = new HashSet<tblBudgetDetail>();

    }


    public int AccountID { get; set; }

    public System.Guid UserID { get; set; }

    public string UserName { get; set; }

    public string PasswordSalt { get; set; }

    public string PasswordHistory { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public System.DateTime LastPasswordChange { get; set; }

    public string LoginType { get; set; }

    public string Password { get; set; }

    public Nullable<bool> IsActive { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblUserDetail> tblUserDetails { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblBudgetDetail> tblBudgetDetails { get; set; }

}

}
