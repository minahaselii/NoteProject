using NoteProject.PicServiice.Commands.DeletePic;
using NoteProject.PicServiice.Commands.UploadPic;
using NoteProject.PicServiice.Queries.PicUrl;

namespace NoteProject.PicServiice.FacadPicManager
{
    public interface IFacadPic
    {
        IUploadPic UploadPicService();
        IDeletePic DeletePicService();
        GetPicUrlService GetPicUrlService();
    }
}
