
namespace Calculator
{
    public class ConvertTemp
    {
        public string UnitTemp { get; set; }
        public double CTemp { get; set; }
        public double Temp { get; set; }
        public double TempR { get; set; }

    }

    public enum TempEnum
    {
        degF,
        degC,
        degR,
        degK
    }
}