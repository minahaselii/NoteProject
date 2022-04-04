
using NoteProject.Application.Services.PicManager.Commands.UploadPic;
using NoteProject.Application.Services.PicManager.Commands.UploadPic;
using ComputerUnion.Application.Services.PicManager.Commands.DeletePic;
using ComputerUnion.Application.Services.PicManager.Queries.PicUrl;

namespace NoteProject.Application.Services.PicManager.FacadPicManager
{
    public interface IFacadPic
    {
        IUploadPic UploadPicService();
        IDeletePic DeletePicService();
        GetPicUrlService GetPicUrlService();
    }
}
