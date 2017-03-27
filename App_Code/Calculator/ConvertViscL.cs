
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertViscL
    {
        public string UnitViscL { get; set; }
        public double DenL { get; set; }
        public double CViscL { get; set; }
        public double ViscL { get; set; }

    }

    public enum ViscLEnum
    {
        [Display(Name = "cP")]
        cP,
        [Display(Name = "lbf*s/ft2")]
        lbfsft2,
        [Display(Name = "lbf*s/in2")]
        lbfsin2,
        [Display(Name = "gf*s/cm2")]
        gfscm2,
        [Display(Name = "kg/m*hr")]
        kgmhr,
        [Display(Name = "kgf*s/cm2")]
        kgfscm2,
        [Display(Name = "kgf*s/m2")]
        kgfsm2,
        [Display(Name = "dyne*s/cm2")]
        dynescm2,
        [Display(Name = "cSt (mm2/s)")]
        cStmm2s,
        [Display(Name = "cm2/s")]
        cm2s,
        [Display(Name = "cm2/min")]
        cm2min,
        [Display(Name = "cm2/hr")]
        cm2hr,
        [Display(Name = "m2/s")]
        m2s,
        [Display(Name = "mPa*s")]
        mPas
    }
}