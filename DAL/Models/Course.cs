using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Course
    {
        [Key]
        [StringLength(450)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ValidateNever]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int ECTS { get; set; }
        [Required]
        public int Lectures { get; set; }
        [Required]
        public int Exercises { get; set; }

        [ValidateNever]
        public ICollection<ProfessorCourse> ProfessorCourses { get; set; }
        [ValidateNever]
        public ICollection<Notification> Notifications { get; set; }
    }
}
