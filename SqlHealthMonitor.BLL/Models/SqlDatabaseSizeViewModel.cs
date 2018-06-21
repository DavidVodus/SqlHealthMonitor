using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
    public class SqlDatabasesSizeViewModel
    {
        public int DatabaseId { get; set; }
        public string DatabaseName { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public double SizeMB { get { return ((Size * 8) / 1024.0); } set { } }
    }
}
