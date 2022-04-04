using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace NoteProject.Entity
{
    public class Profile : EntityId<int>
    {
        public string ProfileImage { get; set; }
        public int Age { get; set; }
        public string JobType { get; set; }
        public string JobStatus { get; set; }
        public int EducationNum { get; set; }
        public int City { get; set; }
        public int Village { get; set; }
        public string ProfileDesc { get; set; }
        public string Linkdin { get; set; }
        public string Instagram { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
     /*   public DateTime BirthTime { get; set; }
        public bool IAccept { get; set; }*/
       
    }
}
