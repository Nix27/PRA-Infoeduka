using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PRA_Infoeduka.Models;

namespace PRA_Infoeduka.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfessorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfessorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AllProfessors()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            try
            {
                var professor = _unitOfWork.AppUser.GetFirstOrDefault(x => x.Id == id, includeProperties: "ProfessorCourses");

                if(professor == null) return NotFound();

                ProfessorVM professorVM = new()
                {
                    Professor = professor,
                    Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),
                    CurrentCourses = professor.ProfessorCourses.Select(pc => pc.Course)
                };

                return View(professorVM);
            }
            catch
            {
                return RedirectToAction("AllProfessors");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProfessorVM professorVM)
        {
            try
            {
                var courses = JsonConvert.DeserializeObject<List<string>>(professorVM.SelectedCourses);

                var professorForUpdate = _unitOfWork.AppUser.GetFirstOrDefault(p => p.Id == professorVM.Professor.Id, includeProperties: "ProfessorCourses.Course");

                professorForUpdate.FirstName = professorVM.Professor.FirstName;
                professorForUpdate.LastName = professorVM.Professor.LastName;
                professorForUpdate.Email = professorVM.Professor.Email;

                var coursesForRemove = professorForUpdate.ProfessorCourses.Where(pc => !courses.Contains(pc.CourseId));
                coursesForRemove.ToList().ForEach(pc => _unitOfWork.ProfessorCourse.Delete(pc));

                var currentCourses = professorForUpdate.ProfessorCourses.Select(pc => pc.Course.Id);
                var newCourses = courses.Except(currentCourses);

                foreach (var newCourse in newCourses)
                {
                    var courseFromDb = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == newCourse);
                    if (courseFromDb == null) continue;

                    professorForUpdate.ProfessorCourses.Add(new ProfessorCourse
                    {
                        Professor = professorForUpdate,
                        Course = courseFromDb
                    });
                }

                _unitOfWork.Save();

                return RedirectToAction("AllProfessors");
            }
            catch
            {
                return View(professorVM);
            }
        }

        [HttpGet]
        public IActionResult GetAllProfessors()
        {
            var allProfessors = _unitOfWork.AppUser.GetAll();

            return Json(new
            {
                data = allProfessors
            });
        }

        [HttpDelete]
        public IActionResult DeleteProfessor(string id)
        {
            var professorForDelete = _unitOfWork.AppUser.GetFirstOrDefault(p => p.Id == id);

            if (professorForDelete == null)
                return Json(new { success = false });

            _unitOfWork.AppUser.Delete(professorForDelete);
            _unitOfWork.Save();

            return Json(new { success = true });
        }
    }
}
