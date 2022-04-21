using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class Comment : EntityId<int>
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public string Comments { get; set; }
        public DateTime InsertTime { get; set; }

    }
}
