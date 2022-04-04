using System.Threading.Tasks;
using ComputerUnion.Common.Dto;
using Microsoft.AspNetCore.Hosting;

namespace ComputerUnion.Application.Services.PicManager.Commands.DeletePic
{
    public class DeletePicService:IDeletePic
    {

        private IHostingEnvironment _hosting;
        private DomainDto _domainDto;
        public DeletePicService(IHostingEnvironment hostingEnvironment,  DomainDto domainDto)
        {
            _hosting = hostingEnvironment;
            _domainDto = domainDto;
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
