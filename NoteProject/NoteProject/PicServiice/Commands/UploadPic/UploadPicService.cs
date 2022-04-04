using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NoteProject.Dto.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace NoteProject.PicServiice.Commands.UploadPic
{
    public class UploadPicService:IUploadPic
    {
        private IHostingEnvironment _hosting;
        
        public UploadPicService(IHostingEnvironment hostingEnvironment)
        {
            _hosting = hostingEnvironment;
           
        }
        public async Task<ResultDto<string>> UploadFile(PicInsertDto picInsertDto)
        {
            if (picInsertDto.pic_file == null)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Data = "",
                    Message="empty-pic"
                };
            }
            string folder = $@"wwwroot/images/"+ picInsertDto.EntityName+ $@"/"+DateTime.Now.ToString("MM-yyyy")+ $@"/";
            string Dbfolder= $@"images/" + picInsertDto.EntityName + $@"/" + DateTime.Now.ToString("MM-yyyy") + $@"/";
            //var uploadRootFolder = Path.Combine(_hosting.ContentRootPath, folder);
            //var uploadRootFolder = Path.Combine(@"wwwroot", folder);
            var uploadRootFolder = Path.Combine("", folder);
            var DbuploadRootFolder = Path.Combine("", Dbfolder);
            //var uploadRootFolder = Path.Combine("PicManager", folder);

            if (!Directory.Exists(uploadRootFolder))
            {
                Directory.CreateDirectory(uploadRootFolder);
            }

            // use commands bellow for  generating random string
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[4];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            string fileExtension = Path.GetExtension(picInsertDto.pic_file.FileName);
            //string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-") + picInsertDto.uploader_id+"-"+ Guid.NewGuid().ToString()+$"-w-{picInsertDto.width},-h-{picInsertDto.height},-q-{picInsertDto.quality}"+fileExtension;
            string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss-") + picInsertDto.uploader_id+"-"+ Convert.ToBase64String(randomBytes) + $"-w-{picInsertDto.width},-h-{picInsertDto.height},-q-{picInsertDto.quality}"+fileExtension;

            string filePath = Path.Combine(uploadRootFolder, fileName);
            string DbfilePath = Path.Combine(DbuploadRootFolder, fileName);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await picInsertDto.pic_file.CopyToAsync(fileStream);
                }

                using (Image image = await Image.LoadAsync(filePath))
                {
                    image.Mutate(x => x.Resize(picInsertDto.width, picInsertDto.height));
                    await image.SaveAsync(filePath,new JpegEncoder() { Quality=picInsertDto.quality});
                }
                
                return new ResultDto<string>
                {
                    IsSuccess = true,
                    Data = DbfilePath,
                    Message = "pic-upload-succeded"
                };
            }
            catch(Exception e)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Data = "",
                    Message = "pic-upload-failed"
                };
            }


        }

      
    }
}
