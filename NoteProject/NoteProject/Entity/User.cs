using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastNmae { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
