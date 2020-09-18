using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeweyDecimalSystem.Models
{
    public class ScoreRecord
    {
        public string Name{ get; set; }
        public int Score{ get; set; }

        public ScoreRecord()
        {
        }

        public ScoreRecord(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
