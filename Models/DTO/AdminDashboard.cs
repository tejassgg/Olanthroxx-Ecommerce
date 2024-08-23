﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AdminDashboard
    {
        public int TotalSales { get; set; }
        public int TotalSalesQuantity { get; set; }
        public int TotalSalesAmount { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveUsersCount { get; set; }
        public List<UserDetails> userList { get; set; }
        public List<ProductDetails> productList = new List<ProductDetails>();
    }
}