using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Tag
{
    public class AddTagDto : TokenDto
    {
        public string tagName { get; set; }
        public int postId { get; set; }
    }
}
