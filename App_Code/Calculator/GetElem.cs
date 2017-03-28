using System.ComponentModel.DataAnnotations;

namespace Calculator
{
    public class GetElem
    {
        public string JHFSELEM { get; set; }
        public double KELEM { get; set; }
        public double ELEMPRICE { get; set; }
        public double ELEMWEIGHT { get; set; }
        public string ELEMSUP { get; set; }
        public double ELEMOD { get; set; }
        public double ELEMID { get; set; }
        public double ELEMLEN { get; set; }
    }

    public enum ElemEnum
    {
        [Display(Name = "JFG-336-CE")]
        JFG336CE,
        [Display(Name = "JFG-536-CE")]
        JFG536CE,
        [Display(Name = "JMG-336-CE")]
        JMG336CE,
        [Display(Name = "JMG-536-CE")]
        JMG536CE,
        [Display(Name = "JFG-372-CE")]
        JFG372CE,
        [Display(Name = "JFG-572-CE")]
        JFG572CE,
        [Display(Name = "JFG-336-CE-316SS")]
        JFG336CE316SS,
        [Display(Name = "JFG-536-CE-316SS")]
        JFG536CE316SS,
        [Display(Name = "JMG-336-CE-316SS")]
        JMG336CE316SS,
        [Display(Name = "JMG-536-CE-316SS")]
        JMG536CE316SS,
        [Display(Name = "JFG-372-CE-316SS")]
        JFG372CE316SS,
        [Display(Name = "JFG-572-CE-316SS")]
        JFG572CE316SS
    }
}