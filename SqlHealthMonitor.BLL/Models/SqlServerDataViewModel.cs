using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{

    public class SqlServerDataViewModel
    {

        public int? SqlServerDataId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}
