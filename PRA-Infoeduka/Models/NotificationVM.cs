using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRA_Infoeduka.Models
{
    public class NotificationVM
    {
        public Notification Notification { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}
