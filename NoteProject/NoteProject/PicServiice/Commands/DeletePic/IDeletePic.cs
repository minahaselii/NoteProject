using System.Threading.Tasks;
using NoteProject.Dto.Common;

namespace NoteProject.PicServiice.Commands.DeletePic
{
    public interface IDeletePic
    {
        Task<ResultDto<string>> DeletePic(DeletePicDto deletePicDto);
    }
}
