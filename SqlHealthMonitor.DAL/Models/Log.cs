using System;
using System.ComponentModel.DataAnnotations;

namespace SqlHealthMonitor.DAL.Models
{

//    public enum Level {
//Fatal=0,
//Error=1,
//Warn=2,
//Info=3,
//Debug=4 }
    public class Log
    {
        public string Exception { get; set; }

        [MaxLength(255)]
        public string Level { get; set; }

        [MaxLength(255)]
        public string Logger { get; set; }

        [Key]
        public int LogId { get; set; }

        public string Message { get; set; }
        public  DateTime TimeStamp { get; set; }
    }
}
