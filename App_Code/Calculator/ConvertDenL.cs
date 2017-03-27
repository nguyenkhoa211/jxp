
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertDenL
    {
        public string UnitDenL { get; set; }
        public double CDenL { get; set; }
        public double DenL { get; set; }
        public double DenLSG { get; set; }
    }

    public enum DenLEnum
    {
        [Display(Name = "SG")]
        SG,
        [Display(Name = "lb/ft3")]
        lbft3,
        [Display(Name = "kg/m3")]
        kgm3,
        [Display(Name = "API")]
        API
    }
}