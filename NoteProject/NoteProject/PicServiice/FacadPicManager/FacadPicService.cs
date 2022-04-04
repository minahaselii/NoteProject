using ComputerUnion.Application.Services.PicManager.Commands.DeletePic;
using ComputerUnion.Application.Services.PicManager.Commands.UploadPic;
using ComputerUnion.Application.Services.PicManager.Queries.PicUrl;
using ComputerUnion.Common.Dto;
using Microsoft.AspNetCore.Hosting;
using NoteProject.Application.Services.PicManager.Commands.UploadPic;
using NoteProject.Application.Services.PicManager.FacadPicManager;

namespace ComputerUnion.Application.Services.PicManager.FacadPicManager
{
   public class FacadPicService: IFacadPic
    {
        private IHostingEnvironment _hosting;
        private DomainDto _domainDto;
        public FacadPicService(IHostingEnvironment hostingEnvironment,  DomainDto domainDto)
        {
            _hosting = hostingEnvironment;
            _domainDto = domainDto;
        }

        private IUploadPic _IUploadPic { get; set; }
        public IUploadPic UploadPicService()
        {
            return new UploadPicService(_hosting);
        }

        private IDeletePic _deletePic { get; set; }
        public IDeletePic DeletePicService()
        {
            return new DeletePicService(_hosting,  domainDto: _domainDto);
        }

        private GetPicUrlService _getPicUrlService { get; set; }
        public GetPicUrlService GetPicUrlService()
        {
            return new GetPicUrlService( domainDto: _domainDto);
        }

        

    }
}
