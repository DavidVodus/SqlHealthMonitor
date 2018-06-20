using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models.Widgets
{
    public class CpuWidget : WidgetBase
    {
        public int NumberOfRecords { get; set; }

    }
}
