using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRA_Infoeduka.Models;

namespace PRA_Infoeduka.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminNotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminNotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            var allNotifications = _unitOfWork.Notification.GetAll(includeProperties: "Course,User");

            return Json(new
            {
                data = allNotifications
            });
        }

        public IActionResult EditNotification(int id)
        {
            try
            {
                var notification = _unitOfWork.Notification.GetFirstOrDefault(x => x.Id == id);

                if (notification == null)
                {
                    return NotFound();
                }

                NotificationVM notificationVM = new()
                {
                    Notification = notification,
                    Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };

                return View(notificationVM);
            }
            catch
            {
                return RedirectToAction("AllCourses");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNotification(NotificationVM notificationVM)
        {
            try
            {
                var notification = _unitOfWork.Notification.GetFirstOrDefault(x => x.Id == notificationVM.Notification.Id);

                if(notification == null) return NotFound();

                notification.Title = notificationVM.Notification.Title;
                notification.Description = notificationVM.Notification.Description;
                notification.ExpirationDate = notificationVM.Notification.ExpirationDate;
                notification.CourseId = notificationVM.Notification.CourseId;

                _unitOfWork.Save();

                return View("AllNotifications");
            }
            catch
            {
                return View(notificationVM);
            }
        }

        [HttpDelete]
        public IActionResult DeleteNotification(int id)
        {
            try
            {
                var currentNotification = _unitOfWork.Notification.GetFirstOrDefault(x => x.Id == id);

                if (id == 0)
                {
                    return Json(new
                    {
                        success = false
                    });
                }

                _unitOfWork.Notification.Delete(currentNotification);
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
