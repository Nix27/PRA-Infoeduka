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
                var professor = _unitOfWork.AppUser.GetFirstOrDefault(x => x.Id == id);

                if(professor == null) return NotFound();

                ProfessorVM professorVM = new()
                {
                    Professor = professor,
                    Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
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

                var professorForUpdate = _unitOfWork.AppUser.GetFirstOrDefault(p => p.Id == professorVM.Professor.Id);

                professorForUpdate.FirstName = professorVM.Professor.FirstName;
                professorForUpdate.LastName = professorVM.Professor.LastName;
                professorForUpdate.Email = professorVM.Professor.Email;

                return View(professorForUpdate);
            }
            catch
            {
                return RedirectToAction("AllProfessors");
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

        
    }
}
