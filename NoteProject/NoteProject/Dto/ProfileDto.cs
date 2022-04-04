using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto
{
    public class ProfileDto
    {
        [Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        [Required(ErrorMessage = "Pleasee enter you age")]
        [Range(10, 100, ErrorMessage = "Enter number between 10 to 100")]
        public int Age { get; set; }
        public string JobList { get; set; }

        public string JobType { get; set; }
        public string JobStatus { get; set; }

        public int EducationNum { get; set; }
        public int City { get; set; }
        public int Village { get; set; }
    public string ProfileDesc { get; set; }
        public string Linkdin { get; set; }
        public string Instagram { get; set; }

    }
}
