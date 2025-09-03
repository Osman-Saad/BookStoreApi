using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Models
{
    public class Basket
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();
    }
}
