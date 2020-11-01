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
    public class FindingCallNumbersController : Controller
    {
        public string Initialise()
        {
            // Get data from file
            CallNumberJSONModel fileJSON = GetTreeJson();
            var callNumberTree = fileJSON.CallNumberTree;
            var level1CallNumbers = fileJSON.Level1CallNumbers;
            var level2CallNumbers = fileJSON.Level2CallNumbers;
            var level3CallNumbers = fileJSON.Level3CallNumbers;
            
            // Set Parents of nodes manually to avoid circular reference in JSON
            callNumberTree.RecursivelySetParents();
            
            // Randomly select level 3 call number
            Random random = new Random();
            int index = random.Next(level3CallNumbers.Count);
            CallNumber randomCallNumber = level3CallNumbers[index];

            // Get correct nodes at each level
            Node<CallNumber> level3CallNumberNode = callNumberTree.FindNodeBreadthFirst(randomCallNumber);
            Node<CallNumber> level2CallNumberNode = level3CallNumberNode.Parent;
            Node<CallNumber> level1CallNumberNode = level2CallNumberNode.Parent;

            int i = 0;
            //Shuffle lists of call numbers and take 3
            level1CallNumbers = level1CallNumbers.OrderBy(x => random.Next()).ToList();
            List<CallNumber> level1Answers = new List<CallNumber>();
            level1Answers.Add(level1CallNumberNode.Data);
            while(level1Answers.Count < 4)
            {
                var level1Number = level1CallNumbers[i];
                if (!level1Answers.Contains(level1Number))
                {
                    level1Answers.Add(level1Number);
                }
                i++;
            }

            i = 0;
            level2CallNumbers = level2CallNumbers.OrderBy(x => random.Next()).ToList();
            List<CallNumber> level2Answers = new List<CallNumber>();
            level2Answers.Add(level2CallNumberNode.Data);
            while (level2Answers.Count < 4)
            {
                var level2Number = level2CallNumbers[i];
                if (!level2Answers.Contains(level2Number) && Convert.ToInt32(level2Number.Number)/100 == Convert.ToInt32(level2CallNumberNode.Data.Number) / 100)
                {
                    level2Answers.Add(level2Number);
                }
                i++;
            }

            i = 0;
            level3CallNumbers = level3CallNumbers.OrderBy(x => random.Next()).ToList();
            List<CallNumber> level3Answers = new List<CallNumber>();
            level3Answers.Add(level3CallNumberNode.Data);
            string callNumber = level3CallNumberNode.Data.Number.Substring(0, 2);
            while (level3Answers.Count < 4)
            {
                int lastDigit = random.Next(0, 10);
                CallNumber test = new CallNumber() { Number = callNumber+lastDigit.ToString(), Name = level3CallNumberNode.Data.Name};
                if (!level3Answers.Any(cn => cn.Number.Equals(test.Number)))
                {
                    level3Answers.Add(test);
                }
                i++;
            }

            // Order lists properly
            level1Answers = level1Answers.OrderBy(c => c.Number).ToList();
            level2Answers = level2Answers.OrderBy(c => c.Number).ToList();
            level3Answers = level3Answers.OrderBy(c => c.Number).ToList();

            List<CallNumber> correctAnswers = new List<CallNumber>(){ level1CallNumberNode.Data, level2CallNumberNode.Data, level3CallNumberNode.Data };

            //Session variable name
            string lastUsedName = HttpContext.Session.GetString("Name");

            return JsonConvert.SerializeObject(new
            {
                lastName= lastUsedName,
                randomCallNumber,
                level1Answers,
                level2Answers,
                level3Answers,
                correctAnswers
            });
        }


        private static CallNumberJSONModel GetTreeJson()
        {
            CallNumberJSONModel fileJSON;
            using (StreamReader sr = new StreamReader(FileURLHelper.CALL_NUMBERS_TREE_JSON_FILE))
            {
                fileJSON = JsonConvert.DeserializeObject<CallNumberJSONModel>(sr.ReadToEnd());
            }
            return fileJSON;
        }

        [HttpPost]
        public string SaveScore(string name, int points)
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
                scoresInFile.Add(new ScoreRecord(name, points));
            }
            else
            {
                // update old score if this one is bigger
                if (storedRecord.Score < points)
                {
                    storedRecord.Score = points;
                }
            }

            //Session variable name
            HttpContext.Session.SetString("Name", name);


            using (StreamWriter sw = new StreamWriter(FileURLHelper.FINDING_CALL_NUMBERS_SCORE_FILE_URL, false))
            {
                foreach (var sc in scoresInFile)
                {
                    string line = sc.Name + "," + sc.Score;
                    sw.WriteLine(line);

                }
            }
            return "OK";
        }

        private List<ScoreRecord> getScoresFromFile()
        {
            List<ScoreRecord> scores = new List<ScoreRecord>();
            using (StreamReader sr = new StreamReader(FileURLHelper.FINDING_CALL_NUMBERS_SCORE_FILE_URL))
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