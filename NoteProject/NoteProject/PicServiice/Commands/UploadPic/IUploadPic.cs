using ComputerUnion.Application.Services.PicManager.Commands.UploadPic;
using NoteProject.Dto.Common;
using System.Threading.Tasks;


namespace NoteProject.Application.Services.PicManager.Commands.UploadPic
{
    public interface IUploadPic
    {
        Task<ResultDto<string>> UploadFile(PicInsertDto picInsertDto);

    }
}
