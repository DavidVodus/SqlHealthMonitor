using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models.Widgets
{
    public enum WidgetType
    {
        CpuWidget,
        JobsWidget,
        DatabasesSizeWidget

    }
    public  class WidgetBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WidgetId { get; set; }
        public string Name { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public long UpdateInterval { get; set; }
        public int Order { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        [ForeignKey("SqlServerDataId")]
        public SqlServerData SqlServerData { get; set; }
        public int? SqlServerDataId { get; set; }
    }
}
