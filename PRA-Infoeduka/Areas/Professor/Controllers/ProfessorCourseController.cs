using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PRA_Infoeduka.Areas.Professor.Controllers
{
    [Area("Professor")]
    public class ProfessorCourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfessorCourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AllProfessorCourses()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllProfessorCourses()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var professorCourses = _unitOfWork.ProfessorCourse.GetAll().Where(pc => pc.ProfessorId == userId.Value);
            var allCourses = _unitOfWork.Course.GetAll();

            IList<Course> coursesOfProfessor = new List<Course>();

            foreach (var course in allCourses)
            {
                foreach (var pc in professorCourses)
                {
                    if(pc.CourseId == course.Id)
                    {
                        coursesOfProfessor.Add(course);
                        break;
                    }
                }
            }

            return Json(new
            {
                data = coursesOfProfessor
            });
        }
    }
}
