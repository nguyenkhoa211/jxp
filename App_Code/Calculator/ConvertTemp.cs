using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calculator
{
    /// <summary>
    /// Summary description for ConvertTemp
    /// </summary>
    public class ConvertTemp
    {
        public string UnitTemp { get; set; }
        public decimal CTemp { get; set; }
        public decimal Temp { get; set; }
        public decimal TempR { get; set; }

    }

    public enum TempEnum
    {
        degF,
        degC,
        degR,
        degK
    }
}