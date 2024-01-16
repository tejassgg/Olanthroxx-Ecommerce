using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class BudgetAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();

        [HttpPost]
        [Route("API/Budget/AddBudget")]
        public IHttpActionResult AddBudget(Budget obj)
        {
            var user = entities.tblUsers.Where(a => a.UserName == obj.UserID).FirstOrDefault();
            if(user != null)
            {
                var dbObj = new tblBudgetDetail
                {
                    Name = obj.Name,
                    AccountID_FK = user.AccountID,
                    Amount = Convert.ToInt32(obj.Amount),
                    Balance = Convert.ToInt32(obj.Amount),
                    CreatedDate = DateTime.Now
                };

                entities.tblBudgetDetails.Add(dbObj);

                int success = entities.SaveChanges();
                if (success > 0)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("API/Budget/AddExpense")]
        public IHttpActionResult AddExpense(Expense obj)
        {
            var user = entities.tblUsers.Where(a => a.UserName == obj.UserID).FirstOrDefault();
            if (user != null)
            {
                var dbObj = new tblExpense
                {
                    ExpName = obj.ExpName,
                    UserID = obj.UserID,
                    Amount = Convert.ToInt32(obj.Amount),
                    CreatedDate = DateTime.Now,
                    BudgetID_FK = obj.BudgetID_FK,
                    IsDeleted = false
                };

                var dbBud = entities.tblBudgetDetails.Where(a => a.BudgetID == obj.BudgetID_FK && a.AccountID_FK == user.AccountID).FirstOrDefault();

                if(dbBud != null)
                {
                    dbBud.Balance -= Convert.ToInt32(obj.Amount);
                    dbBud.ModifiedDate = DateTime.Now;
                }

                entities.tblExpenses.Add(dbObj);

                int success = entities.SaveChanges();
                if (success > 0)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("API/Budget/GetAllBudgetData/{username}")]
        public BudgetData GetAllBudgetData(string username)
        {
            BudgetData objData = new BudgetData();
            var user = entities.tblUsers.Where(a => a.UserName == username).FirstOrDefault();
            
            if(user != null)
            {
                objData.lstBudget = new List<Budget>();
                objData.lstBudget = (from bud in entities.tblBudgetDetails
                                     where bud.AccountID_FK == user.AccountID
                                     select new Budget
                                     {
                                         ID = bud.BudgetID,
                                         Name = bud.Name,
                                         Amount = bud.Amount,
                                         Balance = bud.Balance,
                                         CreatedDate = bud.CreatedDate
                                     }).ToList();

                objData.lstExpense = new List<Expense>();
                objData.lstExpense = (from exp in entities.tblExpenses
                                      join bud in entities.tblBudgetDetails
                                      on exp.BudgetID_FK equals bud.BudgetID
                                      where bud.AccountID_FK == user.AccountID
                                      select new Expense
                                      {
                                          ExpName = exp.ExpName,
                                          Amount = exp.Amount,
                                          Budget = bud.Name,
                                          CreatedDate = exp.CreatedDate
                                      }).ToList();

                if(objData.lstBudget != null)
                {
                    objData.lstBudgetTypes = (from obj in objData.lstBudget
                                              select new CommonDropDown
                                              {
                                                  Text = obj.Name,
                                                  Value = obj.ID
                                              }).ToList();
                }
            }

            return objData;
        }

    }
}