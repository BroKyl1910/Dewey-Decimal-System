using System;

namespace DeweyDecimalSystem.Models
{
    public class CallNumber
    {
        public string Number { get; set; }
        public string Name { get; set; }

        public CallNumber()
        {
        }

        public CallNumber(string number, string name)
        {
            Number = number;
            Name = name;
        }

        public static CallNumber FromTextFileLine(string line)
        {
            string num = line.Split(' ')[0];
            // NUM Desc
            string desc = line.Substring(4);
            return new CallNumber(num, desc);
        }

        public override string ToString()
        {
            return Number + " " + Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Convert.ToInt32(Number) == Convert.ToInt32(((CallNumber)obj).Number);
        }
    }
}
