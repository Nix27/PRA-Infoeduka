using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRA_Infoeduka.Models
{
    public class ProfessorVM
    {
        public AppUser Professor { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
        public IEnumerable<Course> CurrentCourses { get; set; }
        public string SelectedCourses { get; set; }
    }
}
