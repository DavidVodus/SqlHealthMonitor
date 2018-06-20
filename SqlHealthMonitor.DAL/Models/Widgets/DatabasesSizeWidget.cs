using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models.Widgets
{
    public class DatabasesSizeWidget : WidgetBase
    {
        public DatabasesSizeWidget()
        {
            DatabaseIds = new List<int>();
        }
        public List<int> DatabaseIds
        {
            get
            {
                return String.IsNullOrEmpty(DatabaseIdsStorage) ? new List<int>() : DatabaseIdsStorage.Split(',')
.Select(x => int.Parse(x)).ToList();
            }
            set { DatabaseIdsStorage = string.Join(",",value.Select(x =>x.ToString())); }
        }
        public string DatabaseIdsStorage { get; set; }
    }
}
