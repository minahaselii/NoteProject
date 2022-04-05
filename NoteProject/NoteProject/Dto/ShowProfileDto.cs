using Microsoft.AspNetCore.Http;
using NoteProject.Dto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace NoteProject.Dto
{
    public class ShowProfileDto : TokenDto
    {
        public int UserId { get; set; }
    }
}
