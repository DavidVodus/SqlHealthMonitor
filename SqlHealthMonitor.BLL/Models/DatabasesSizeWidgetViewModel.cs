﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
    public class DatabasesSizeWidgetViewModel : WidgetViewModelBase
    {

        public List<int> DatabaseIds { get; set; }
    }
}
