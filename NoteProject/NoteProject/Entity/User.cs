using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NoteProject.Entity
{
    public class User: EntityId<int>
    {

      
        public string FirstName { get; set; }
        public string LastNmae { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public DateTime tokenExp { get; set; }

        public List<Note> notes { get; set; }
        public ICollection<Like> likes { get; set; }
        public Profile Profile { get; set; }
        public bool HasProfile { get; set; }
    }
}
