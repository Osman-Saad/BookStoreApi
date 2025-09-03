using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Models
{
    public class OrderItem:BaseEntity
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public string CoverUrl { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
