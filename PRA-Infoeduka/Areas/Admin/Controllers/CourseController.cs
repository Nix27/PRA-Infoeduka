using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PRA_Infoeduka.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AllCourses()
        {
            return View();
        }

        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCourse(Course course)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(course);
                }

                _unitOfWork.Course.Add(course);

                _unitOfWork.Save();

                return RedirectToAction("AllCourses");
            }
            catch
            {
                return View(course);
            }
        }

        public IActionResult EditCourse(string id)
        {
            try
            {
                var course = _unitOfWork.Course.GetFirstOrDefault(x => x.Id == id);

                if (course == null)
                {
                    return NotFound();
                }

                return View(course);
            }
            catch 
            {
                return RedirectToAction("AllCourses");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCourse(Course course)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(course);
                }

                _unitOfWork.Course.Update(course);
                _unitOfWork.Save();

                return RedirectToAction("AllCourses");
            }
            catch
            {
                return View(course);
            }
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var allCourses = _unitOfWork.Course.GetAll();

            return Json(new
            {
                data = allCourses
            });
        }

        [HttpDelete]
        public IActionResult DeleteCourse(string id)
        {
            try
            {
                var currentCourse = _unitOfWork.Course.GetFirstOrDefault(x => x.Id == id);

                if (id == null)
                {
                    return Json(new
                    {
                        success = false
                    });
                }

                _unitOfWork.Course.Delete(currentCourse);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true
                });
            }
            catch 
            {
                return Json(new
                {
                    success = false
                });
            }
        }

    }
}
