using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class Like : EntityId<int>
    {
       
        public int NoteId { get; set; }
        public int  UserId { get; set; }
        public bool IsLiked { get; set; }
        public User User { get; set; }
        public Note Note { get; set; }
    }
}
