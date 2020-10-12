using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Models;
using DeweyDecimalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DeweyDecimalSystem.Helpers;

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

            //Session variable name
            string lastUsedName = HttpContext.Session.GetString("Name");

            // Sort books by number then name
            // LINQ uses a stable quicksort algorithm
            var sortedBooks = books.OrderBy(b => b.CallNumber.Number).ThenBy(b => b.CallNumber.Name).ToList();
            string json = JsonConvert.SerializeObject(new
            {
                lastName = lastUsedName,
                bookViewModels,
                sortedBooks
            });
            return json;
        }

        [HttpPost]
        public string SaveTime(string name, int time)
        {
            var scoresInFile = getScoresFromFile();
            ScoreRecord storedRecord = null;

            // See if there is already a record with this name
            if(scoresInFile.Any(s=>s.Name == name))
            {
                 storedRecord = scoresInFile.First(s => s.Name == name);
            }

            if (storedRecord == null)
            {
                // first time playing
                scoresInFile.Add(new ScoreRecord(name, time));
            }
            else
            {
                // update old score if this one is smaller
                if (storedRecord.Score > time)
                {
                    storedRecord.Score = time;
                }
            }

            //Session variable name
            HttpContext.Session.SetString("Name", name);


            using (StreamWriter sw = new StreamWriter(FileURLHelper.REPLACING_BOOKS_SCORE_FILE_URL, false))
            {
                foreach (var score in scoresInFile)
                {
                    string line = score.Name + "," + score.Score;
                    sw.WriteLine(line);

                }
            }
            return "OK";
        }

        private List<ScoreRecord> getScoresFromFile()
        {
            List<ScoreRecord> scores = new List<ScoreRecord>();
            using (StreamReader sr = new StreamReader(FileURLHelper.REPLACING_BOOKS_SCORE_FILE_URL))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    var lineParts = line.Split(',');
                    scores.Add(new ScoreRecord(lineParts[0], int.Parse(lineParts[1])));
                    line = sr.ReadLine();
                }
            }

            return scores;
        }

    }
}