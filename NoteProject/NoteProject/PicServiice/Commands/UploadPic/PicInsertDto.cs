using Microsoft.AspNetCore.Http;

namespace NoteProject.PicServiice.Commands.UploadPic
{
   public class PicInsertDto
    {
        public string EntityName { get; set; }
        public int uploader_id { get; set; }
        public IFormFile pic_file { get; set; }
        public int height { get; set; } = 300;
        public int width { get; set; } = 300;
        public int quality { get; set; } = 70;


    }
}
