using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICourseRepository Course { get; }  
        INotificationRepository Notification { get; }  
        IAppUserRepository AppUser { get; }  
        IProfessorCourseRepository ProfessorCourse { get; }  

        void Save();
    }
}
