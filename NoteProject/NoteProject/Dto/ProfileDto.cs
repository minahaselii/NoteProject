﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto
{
    public class ProfileDto
    {
        
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
