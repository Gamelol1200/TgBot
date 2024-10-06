using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class User
    {
        public long Id { get; set; }
        public long IdTelegram { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
