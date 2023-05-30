using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ProfessorCourse
    {
        [Key]
        public int Id { get; set; }

        public string ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public AppUser Professor { get; set; }

        public string CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

    }
}
