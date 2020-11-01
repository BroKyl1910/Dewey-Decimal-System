using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Helpers;
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

        public IActionResult IdentifyingAreas()
        {
            return View();
        }

        public IActionResult FindingCallNumbers()
        {
            return View();
        }

        public string IdentifyingAreasLeaderboardData()
        {
            List<ScoreRecord> scores = getScoresFromFile(FileURLHelper.IDENTIFYING_AREAS_SCORE_FILE_URL);
            scores = scores.OrderBy(s => s.Score).ThenBy(s => s.Name).ToList();
            return JsonConvert.SerializeObject(scores);
        }

        public string ReplacingBooksLeaderboardData()
        {
            List<ScoreRecord> scores = getScoresFromFile(FileURLHelper.REPLACING_BOOKS_SCORE_FILE_URL);
            scores = scores.OrderBy(s => s.Score).ThenBy(s => s.Name).ToList();
            return JsonConvert.SerializeObject(scores);
        }

        public string FindingCallNumbersLeaderboardData()
        {
            List<ScoreRecord> scores = getScoresFromFile(FileURLHelper.FINDING_CALL_NUMBERS_SCORE_FILE_URL);
            scores = scores.OrderByDescending(s => s.Score).ThenBy(s => s.Name).ToList();
            return JsonConvert.SerializeObject(scores);
        }

        private List<ScoreRecord> getScoresFromFile(string fileUrl)
        {
            List<ScoreRecord> scores = new List<ScoreRecord>();
            using (StreamReader sr = new StreamReader(fileUrl))
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