using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class LikeDto : TokenDto
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public bool IsLiked { get; set; }
       
    }
}
