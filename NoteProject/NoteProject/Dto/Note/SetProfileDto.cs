using Microsoft.AspNetCore.Http;
using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class SetProfileDto: TokenDto
    {
        public bool IsClientSide { get; set; }
        public int Age { get; set; }
        public string ProfileDesc { get; set; }
        public string JobStatus { get; set; }
        public string Jobtype { get; set; }
        public int EducationNum { get; set; }
        public int City { get; set; }
        public int Village { get; set; }
        public string Linkdin { get; set; }
        public string Instagram { get; set; }
        /*public DateTime BirthTime { get; set; }
        public bool IAccept { get; set; }*/
        public int UserId { get; set; }



    }
}
