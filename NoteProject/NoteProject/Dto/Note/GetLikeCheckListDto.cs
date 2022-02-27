using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteProject.Dto.Note
{
    public class GetLikeCheckListDto
    {
        public int IdPost { get; set; }
        public int IsLiked { get; set; }
        public GetLikeCheckListDto(int idpost,int isliked)
        {
            IdPost = idpost;
            IsLiked = isliked;
        }
    }
}
