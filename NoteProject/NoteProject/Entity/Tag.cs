using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Entity
{
    public class Tag : EntityId<int>
    {
        public string tagName { get; set; }
        public int postId { get; set; }
    }
}
