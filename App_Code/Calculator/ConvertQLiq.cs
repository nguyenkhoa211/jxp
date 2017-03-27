
using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class ConvertQLiq
    {
        public string UnitFlowL { get; set; }
        public double CQLiq { get; set; }
        public double QLiq { get; set; }
        public double DenL { get; set; }
        public double QLiqACFM { get; set; }

    }

    public enum QLiqEnum
    {
        [Display(Name = "GPM")]
        GPM,
        [Display(Name = "ft3/s")]
        ft3s,
        [Display(Name = "ft3/min")]
        ft3min,
        [Display(Name = "ft3/hr")]
        ft3hr,
        [Display(Name = "ft3/d")]
        ft3d,
        [Display(Name = "m3/d")]
        m3d,
        [Display(Name = "m3/hr")]
        m3hr,
        [Display(Name = "m3/s")]
        m3s,
        [Display(Name = "L/d")]
        Ld,
        [Display(Name = "L/hr")]
        Lhr,
        [Display(Name = "L/min")]
        Lmin,
        [Display(Name = "L/s")]
        Ls,
        [Display(Name = "BBL/D")]
        BBLD,
        [Display(Name = "BBL/H")]
        BBLH,
        [Display(Name = "Lb/s")]
        Lbs,
        [Display(Name = "Lb/min")]
        Lbmin,
        [Display(Name = "Lb/Hr")]
        LbHr,
        [Display(Name = "Lb/d")]
        Lbd,
        [Display(Name = "kg/s")]
        kgs,
        [Display(Name = "kg/min")]
        kgmin,
        [Display(Name = "kg/h")]
        kgh,
        [Display(Name = "kg/d")]
        kgd
    }
}