using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Models
{
    public class Author: BaseEntity
    {
        public string Name { get; set; }
        public string? Biography { get; set; }
        public DateOnly BirthDate { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();

        public int Age => calculateAge();


        private int calculateAge()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - BirthDate.Year;
            if (BirthDate > today.AddYears(-age)) age--;
            return age;
        }
    }
}
