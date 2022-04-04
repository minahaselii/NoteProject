using System.Threading.Tasks;
using NoteProject.Dto.Common;

namespace NoteProject.PicServiice.Commands.UploadPic
{
    public interface IUploadPic
    {
        Task<ResultDto<string>> UploadFile(PicInsertDto picInsertDto);

    }
}
