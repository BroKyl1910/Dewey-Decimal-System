using DeweyDecimalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeweyDecimalSystem.ViewModels
{
    public class BookViewModel
    {
        public Book Book{ get; set; }
        public string ColorHex { get; set; }

        public BookViewModel()
        {
        }

        public BookViewModel(Book book, string colorHex)
        {
            Book = book;
            ColorHex = colorHex;
        }
    }
}
