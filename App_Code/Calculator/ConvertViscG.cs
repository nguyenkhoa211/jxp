
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertViscG
    {
        public string UnitViscG { get; set; }
        public double DenG { get; set; }
        public double CViscG { get; set; }
        public double ViscG { get; set; }

    }

    public enum ViscGEnum
    {
        [Display(Name = "cP")]
        cP,
        [Display(Name = "lbf*s/ft2")]
        lbfsft2,
        [Display(Name = "lbf*s/in2")]
        lbfsin2,
        [Display(Name = "gf*s/cm2")]
        gfscm2,
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