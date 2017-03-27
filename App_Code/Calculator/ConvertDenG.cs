
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertDenG
    {
        public string UnitDenG { get; set; }
        public double CDenG { get; set; }
        public double Temp { get; set; }
        public double Press { get; set; }
        public double Zcomp { get; set; }
        public double DenG { get; set; }
        public double DenGSG { get; set; }

    }

    public enum DenGEnum
    {
        [Display(Name = "SG")]
        SG,
        [Display(Name = "MW")]
        MW,
        [Display(Name = "lb/ft3")]
        lbft3,
        [Display(Name = "kg/m3")]
        kgm3
    }
}