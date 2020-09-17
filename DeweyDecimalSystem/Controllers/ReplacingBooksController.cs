using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Models;
using DeweyDecimalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeweyDecimalSystem.Controllers
{
    public class ReplacingBooksController : Controller
    {
        public string Initialise()
        {
            // Set up colors and books
            List<string> colors = new List<string>
            {
                "#E35288",
                "#A989F1",
                "#CB8F60",
                "#B7366A",
                "#0DACB2",
                "#0577DF",
                "#E51033",
                "#5D9DBE",
                "#A77B6F",
                "#EA38AA"
            };
            List<Book> books = new List<Book> {
                new Book(new CallNumber(100.20, "SIM")),
                new Book(new CallNumber(325.00, "GAR")),
                new Book(new CallNumber(400.05, "BIL")),
                new Book(new CallNumber(250.14, "JOE")),
                new Book(new CallNumber(537.56, "ELT")),
                new Book(new CallNumber(456.89, "JOH")),
                new Book(new CallNumber(432.18, "HAR")),
                new Book(new CallNumber(432.18, "STY")),
                new Book(new CallNumber(574.89, "BIL")),
                new Book(new CallNumber(603.36, "WIT")),
            };
            
            //Jumble books and colors
            colors = colors.OrderBy(c => Guid.NewGuid()).ToList();
            books = books.OrderBy(c => Guid.NewGuid()).ToList();
            
            // Populate book view models
            List<BookViewModel> bookViewModels = new List<BookViewModel>();
            for (int i = 0; i < books.Count; i++)
            {
                bookViewModels.Add(new BookViewModel(books[i], colors[i]));
            }

            // Sort books by number then name
            var sortedBooks = books.OrderBy(b => b.CallNumber.Number).ThenBy(b => b.CallNumber.Name).ToList();
            string json = JsonConvert.SerializeObject(new { 
                bookViewModels,
                sortedBooks
            });
            return json;
        }
    }
}