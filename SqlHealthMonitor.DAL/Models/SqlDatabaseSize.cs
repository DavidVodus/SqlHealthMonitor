using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models
{
    public class SqlDatabasesSize
    {
        public int DatabaseId { get; set; }
        public string DatabaseName { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
     
    }
}
