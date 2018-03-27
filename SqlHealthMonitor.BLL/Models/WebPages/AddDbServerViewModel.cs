using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class AddDbServerViewModel :PageViewModelBase
    {
        [Required]
        [Display(Name = "ServerName", ResourceType = typeof(Resources.Global))]
        public string ServerName { get; set; }

        [Required]
        [Display(Name = "ConnectionString", ResourceType = typeof(Resources.Global))]
        public string ConnectionString { get; set; }

        [Required]
        [Display(Name = "Login", ResourceType = typeof(Resources.Global))]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        public string Password { get; set; }
    }
}
