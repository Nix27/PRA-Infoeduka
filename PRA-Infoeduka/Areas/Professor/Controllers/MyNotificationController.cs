using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRA_Infoeduka.Models;
using System.Security.Claims;

namespace PRA_Infoeduka.Areas.Professor.Controllers
{
    [Area("Professor")]
    public class MyNotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MyNotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AllMyNotifications()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllMyNotifications()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var professorNotifications = _unitOfWork.Notification.GetAll(n => n.User.Id == userId.Value, includeProperties:"Course");

            return Json(new
            {
                data = professorNotifications
            });
        }

        public IActionResult EditMyNotification(int id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMyNotification(NotificationVM notificationVM)
        {
            try
            {
                var notification = _unitOfWork.Notification.GetFirstOrDefault(x => x.Id == notificationVM.Notification.Id);

                if (notification == null) return NotFound();

                notification.Title = notificationVM.Notification.Title;
                notification.Description = notificationVM.Notification.Description;
                notification.ExpirationDate = notificationVM.Notification.ExpirationDate;
                notification.CourseId = notificationVM.Notification.CourseId;

                _unitOfWork.Save();

                return View("AllMyNotifications");
            }
            catch
            {
                return View(notificationVM);
            }
        }

        [HttpDelete]
        public IActionResult DeleteMyNotification(int id)
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
