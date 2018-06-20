using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.DAL.Models.WebPages
{
   public abstract class PageBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }
        public string ControllerName { get; set; }
        public string PageName { get; set; }
        public string StartActionName { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
