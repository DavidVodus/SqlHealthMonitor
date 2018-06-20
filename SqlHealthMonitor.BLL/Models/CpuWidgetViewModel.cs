using SqlHealthMonitor.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
   public class CpuWidgetViewModel: WidgetViewModelBase
    {
        public int NumberOfRecords { get; set; }
    }
}
