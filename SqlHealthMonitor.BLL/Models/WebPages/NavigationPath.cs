using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class ControllerAction
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public static implicit operator List<object>(ControllerAction v)
        {
            throw new NotImplementedException();
        }
    }
    public class NavigationPath
    {
        public NavigationPath()
        {
            Children = new List<ControllerAction>();
            Parent = new ControllerAction();
        }
        public ControllerAction Parent { get; set; }
        public List<ControllerAction> Children { get; set; }
    }
}
