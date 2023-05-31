using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext.DbContext _dbContext;

        public UnitOfWork(AppDbContext.DbContext dbContext)
        {
            _dbContext = dbContext;
            Course = new CourseRepository(dbContext);
            Notification = new NotificationRepository(dbContext);
            AppUser = new AppUserRepository(dbContext);
            ProfessorCourse = new ProfessorCourseRepository(dbContext);
        }

        public ICourseRepository Course
        {
            get;
            private set;
        }

        public INotificationRepository Notification
        {
            get;
            private set;
        }

        public IAppUserRepository AppUser
        {
            get;
            private set;
        }

        public IProfessorCourseRepository ProfessorCourse
        {
            get;
            private set;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
