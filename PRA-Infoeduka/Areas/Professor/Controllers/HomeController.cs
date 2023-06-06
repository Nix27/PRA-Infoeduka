using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using PRA_Infoeduka.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PRA_Infoeduka.Areas.Professor.Controllers
{
    [Area("Professor")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateNotification()
        {
            NotificationVM notificationVM = new()
            {
                Notification = new(),
                Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return View(notificationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNotification(NotificationVM notificationVM)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                notificationVM.Notification.UserId = userId.Value;

                _unitOfWork.Notification.Add(notificationVM.Notification);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create notification");
                return View(notificationVM);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}