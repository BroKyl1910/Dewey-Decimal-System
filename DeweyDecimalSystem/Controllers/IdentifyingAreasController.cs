using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Helpers;
using DeweyDecimalSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeweyDecimalSystem.Controllers
{
    public class IdentifyingAreasController : Controller
    {
        public string Initialise(int roundNumber)
        {
            Dictionary<string, string> areaDictionary = PopulateDictionary(roundNumber);
            Dictionary<string, string> shuffledDictionary = ShuffleDictionary(areaDictionary);

            Random rand = new Random();
            List<string> keys = new List<string>(shuffledDictionary.Keys).Take(4).OrderBy(x => rand.Next()).ToList();
            List<string> values= new List<string>(shuffledDictionary.Values).Take(7).OrderBy(x => rand.Next()).ToList();

            //Session variable name
            string lastUsedName = HttpContext.Session.GetString("Name");

            return JsonConvert.SerializeObject(new { lastName = lastUsedName, areaDictionary, keys, values });
        }

        [HttpPost]
        public string SaveTime(string name, int time)
        {
            var scoresInFile = getScoresFromFile();
            ScoreRecord storedRecord = null;

            // See if there is already a record with this name
            if (scoresInFile.Any(s => s.Name == name))
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


            using (StreamWriter sw = new StreamWriter(FileURLHelper.IDENTIFYING_AREAS_SCORE_FILE_URL, false))
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
            using (StreamReader sr = new StreamReader(FileURLHelper.IDENTIFYING_AREAS_SCORE_FILE_URL))
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

        private Dictionary<string, string> ShuffleDictionary(Dictionary<string, string> areaDictionary)
        {
            Random rand = new Random();
            var shuffledDictionary = areaDictionary.OrderBy(x => rand.Next())
              .ToDictionary(item => item.Key, item => item.Value);


            return shuffledDictionary;
        }

        private Dictionary<string, string> PopulateDictionary(int roundNumber)
        {
            Dictionary<string, string> areaDictionary = new Dictionary<string, string>();
            if (roundNumber % 2 == 0)
            {
                //even rounds are area to number
                areaDictionary.Add("Computer science, information & general works", "000");
                areaDictionary.Add("Philosophy & psychology", "100");
                areaDictionary.Add("Religion", "200");
                areaDictionary.Add("Social sciences", "300");
                areaDictionary.Add("Language", "400");
                areaDictionary.Add("Pure Science", "500");
                areaDictionary.Add("Technology", "600");
                areaDictionary.Add("Arts & recreation", "700");
                areaDictionary.Add("Literature", "800");
                areaDictionary.Add("History & geography", "900");
            }
            else
            {
                //odd rounds are number to area
                areaDictionary.Add("000", "Computer science, information & general works");
                areaDictionary.Add("100", "Philosophy & psychology");
                areaDictionary.Add("200", "Religion");
                areaDictionary.Add("300", "Social sciences");
                areaDictionary.Add("400", "Language");
                areaDictionary.Add("500", "Pure Science");
                areaDictionary.Add("600", "Technology");
                areaDictionary.Add("700", "Arts & recreation");
                areaDictionary.Add("800", "Literature");
                areaDictionary.Add("900", "History & geography");
            }
            return areaDictionary;
        }
    }
}