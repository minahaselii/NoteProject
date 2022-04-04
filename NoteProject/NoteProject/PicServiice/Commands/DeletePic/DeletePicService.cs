using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using NoteProject.Dto.Common;

namespace NoteProject.PicServiice.Commands.DeletePic
{
    public class DeletePicService:IDeletePic
    {

        private IHostingEnvironment _hosting;
        public DeletePicService(IHostingEnvironment hostingEnvironment)
        {
            _hosting = hostingEnvironment;
        }

        public async Task<ResultDto<string>> DeletePic(DeletePicDto deletePicDto)
        {
            if (System.IO.File.Exists(deletePicDto.picAddress))
            {
                System.IO.File.Delete(deletePicDto.picAddress);
                return new ResultDto<string>
                {
                    IsSuccess = true,
                    Message = "image deleted successfully"
                };
            }
            return new ResultDto<string>
            {
                IsSuccess = true,
                Message = "image has been already deleted"
            };
        }
    }
}
