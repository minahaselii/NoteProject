using Microsoft.AspNetCore.Hosting;
using NoteProject.PicServiice.Commands.DeletePic;
using NoteProject.PicServiice.Commands.UploadPic;
using NoteProject.PicServiice.Queries.PicUrl;

namespace NoteProject.PicServiice.FacadPicManager
{
   public class FacadPicService: IFacadPic
    {
        private IHostingEnvironment _hosting;
        public FacadPicService(IHostingEnvironment hostingEnvironment)
        {
            _hosting = hostingEnvironment;
        }

        private IUploadPic _IUploadPic { get; set; }
        public IUploadPic UploadPicService()
        {
            return new UploadPicService(_hosting);
        }

        private IDeletePic _deletePic { get; set; }
        public IDeletePic DeletePicService()
        {
            return new DeletePicService(_hosting);
        }

        private GetPicUrlService _getPicUrlService { get; set; }
        public GetPicUrlService GetPicUrlService()
        {
            return new GetPicUrlService();
        }

        

    }
}
