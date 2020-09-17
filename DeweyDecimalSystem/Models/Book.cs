using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeweyDecimalSystem.Models
{
    public class Book
    {
        public CallNumber CallNumber { get; set; }

        public Book()
        {
        }

        public Book(CallNumber callNumber)
        {
            CallNumber = callNumber;
        }
    }
}
