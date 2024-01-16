using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class BudgetData
    {
        public List<CommonDropDown> lstBudgetTypes { get; set; }
        public List<Budget> lstBudget { get; set; }
        public List<Expense> lstExpense { get; set; }
    }

    public class Budget
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }
        public int? Balance { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
    public class Expense
    {
        public int BudgetID_FK { get; set; }
        public string Budget { get; set; }
        public string ExpName { get; set; }
        public int? Amount { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
