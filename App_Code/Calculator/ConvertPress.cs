
namespace Calculator
{
    public class ConvertPress
    {
        public string UnitPress { get; set; }
        public double CPress { get; set; }
        public double Press { get; set; }
    }

    public enum PressEnum
    {
        psig,
        psia,
        kPag,
        kPaa,
        MPaa,
        MPag,
        barg,
        bara,
        kgcm2g,
        kgcm2a
    }
}