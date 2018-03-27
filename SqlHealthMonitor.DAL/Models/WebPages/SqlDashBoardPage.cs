using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models.WebPages
{
    public class SqlDashBoardPage : PageBase
    {
        public class SqlServerData
        {
            public string Name { get; set; }
            public string ConnectionString { get; set; }
        }
    }
}
