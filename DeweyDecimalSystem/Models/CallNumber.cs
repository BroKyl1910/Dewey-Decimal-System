namespace DeweyDecimalSystem.Models
{
    public class CallNumber
    {
        public double Number { get; set; }
        public string Name { get; set; }

        public CallNumber()
        {
        }

        public CallNumber(double number, string name)
        {
            Number = number;
            Name = name;
        }
    }
}
