using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRA_Infoeduka.Models
{
    public class ProfessorVM
    {
        public AppUser Professor { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Courses { get; set; }

        [ValidateNever]
        public string SelectedCourses { get; set; }
    }
}
