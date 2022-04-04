using System.Threading.Tasks;
using ComputerUnion.Common.Dto;

namespace ComputerUnion.Application.Services.PicManager.Commands.DeletePic
{
    public interface IDeletePic
    {
        Task<ResultDto<string>> DeletePic(DeletePicDto deletePicDto);
    }
}
