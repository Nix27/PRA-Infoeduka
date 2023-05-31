using Microsoft.AspNetCore.Mvc;

namespace PRA_Infoeduka.Areas.Admin.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult AllCourses()
        {
            return View();
        }
    }
}
