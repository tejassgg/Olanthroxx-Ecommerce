using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class LstOrderDetails
    {
        public UserDetails UserDetails { get; set; }
        public List<CartDetails> CartDetails { get; set; }
        public string UserName;
    }
}
