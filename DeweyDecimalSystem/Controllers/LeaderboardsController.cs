using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeweyDecimalSystem.Controllers
{
    public class LeaderboardsController : Controller
    {
        public IActionResult ReplacingBooks()
        {
            return View();
        }

        public string ReplacingBooksLeaderboardData()
        {
            List<ScoreRecord> scores = getScoresFromFile();
            scores = scores.OrderBy(s => s.Score).ThenBy(s => s.Name).ToList();
            return JsonConvert.SerializeObject(scores);
        }

        private List<ScoreRecord> getScoresFromFile()
        {
            List<ScoreRecord> scores = new List<ScoreRecord>();
            using (StreamReader sr = new StreamReader("Scores.csv"))
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