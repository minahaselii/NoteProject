using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class CommentListDto : TokenDto
    {
        public int NoteId { get; set; }
        
    }
}
