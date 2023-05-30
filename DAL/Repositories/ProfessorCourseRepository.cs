using DAL.AppDbContext;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProfessorCourseRepository : Repository<ProfessorCourse>, IProfessorCourseRepository
    {
        public ProfessorCourseRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
