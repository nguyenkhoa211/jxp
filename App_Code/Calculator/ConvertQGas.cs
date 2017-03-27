
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertQGas
    {
        public string UnitFlowG { get; set; }
        public double CQGas { get; set; }
        public double Temp { get; set; }
        public double ViscG { get; set; }
        public double Zcomp { get; set; }
        public double DenG { get; set; }
        public double Press { get; set; }
        public double DenGSG { get; set; }
        public double QGas { get; set; }
        public double QGasACFM { get; set; }
        public double QGasACFS { get; set; }

    }

    public enum QGasEnum
    {
        [Display(Name = "MMSCFD")]
        MMSCFD,
        [Display(Name = "SCFM")]
        SCFM,
        [Display(Name = "ACFM")]
        ACFM,
        [Display(Name = "(S)e3m3/day")]
        Se3m3day,
        [Display(Name = "NM3/hr")]
        NM3hr,
        [Display(Name = "SM3/hr")]
        SM3hr,
        [Display(Name = "AM3/hr")]
        AM3hr,
        [Display(Name = "lb/hr")]
        lbhr,
        [Display(Name = "kg/hr")]
        kghr
    }
}