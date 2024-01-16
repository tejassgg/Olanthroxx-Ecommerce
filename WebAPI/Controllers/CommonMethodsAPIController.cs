using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class CommonMethodsAPIController : ApiController
    {
        OlanthroxxEntities entities = new OlanthroxxEntities();
        
        [HttpGet]
        [Route("API/Common/GetPaymentModes")]
        public List<CommonDropDown> GetPaymentModes()
        {
            var paymentModes = entities.tblMasCommonTypes.Where(a => a.MasterType == "PaymentModes" && a.IsActive == true).
            Select(x => new CommonDropDown
            {
                Text = x.Description,
                Value = x.Code,
            }).ToList();

            return paymentModes;
        }

        [HttpGet]
        [Route("API/Common/GetLoginTypeList")]
        public List<CommonDropDown> GetLoginTypeList()
        {
            var loginTypeList = entities.tblMasCommonTypes.Where(a => a.MasterType == "LoginType" && a.IsActive == true).
            Select(x => new CommonDropDown
                {
                    Text = x.Description,
                    Value = x.Code,
                }).ToList();

            return loginTypeList;
        }

        [HttpGet]
        [Route("API/Common/GetStateList")]
        public List<CommonDropDown> GetStateList()
        {
            var stateList = entities.tblMasStates.Where(a => a.CountryID == 1).
                Select(x => new CommonDropDown
                {
                    Text = x.State,
                    Value = x.StateID,
                }).ToList();

            return stateList;
        }

        [HttpGet]
        [Route("API/Common/GetCityList/{stateID}")]
        public List<CommonDropDown> GetCityList(int stateID)
        {
            var cityList = entities.tblMasCities.Where(a => a.StateID_FK == stateID).
                Select(x => new CommonDropDown
                {
                    Text = x.City,
                    Value = x.CityID,
                }).ToList();

            return cityList;
        }

        [HttpGet]
        [Route("API/Common/GetOrderStatusList")]
        public List<CommonDropDown> GetOrderStatusList()
        {
            var orderStatusList = entities.tblMasCommonTypes.Where(a => a.MasterType == "OrderStatus" && a.IsActive == true).
            Select(x => new CommonDropDown
            {
                Text = x.Description,
                Value = x.Code,
            }).ToList();

            return orderStatusList;
        }

        [HttpGet]
        [Route("API/Common/GetCategory")]
        public List<CommonDropDown> GetCategory()
        {
            List<CommonDropDown> categoryDetails = new List<CommonDropDown>();
            var catList = entities.tblCategories.ToList();
            if (catList != null && catList.Count > 0)
            {
                foreach (var cat in catList)
                {
                    var obj = new CommonDropDown();
                    obj.Value = cat.CategoryID;
                    obj.Text = cat.CategoryType;
                    categoryDetails.Add(obj);
                }
            }
            return categoryDetails;
        }

    }
}