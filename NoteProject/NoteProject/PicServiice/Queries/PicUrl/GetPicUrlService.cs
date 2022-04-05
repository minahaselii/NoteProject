using NoteProject.Dto.Common;

namespace NoteProject.PicServiice.Queries.PicUrl
{
    public class GetPicUrlService
    {
        private readonly DomainDto _domainDto;
         public GetPicUrlService(DomainDto domainDto)
        {
            _domainDto = domainDto;
        }

        public ResultDto<string> DeleteUrl(string picUrl)
        {
            return new ResultDto<string>
            {
                Data = @"wwwroot/" + picUrl,
                IsSuccess = true
            };
            
        }

        public ResultDto<string> GetUrl(string picUrl)
        {
            return new ResultDto<string>
            {
                Data =picUrl!=null ? _domainDto.name + picUrl :null,
                IsSuccess = true
            };

        }
    }
}
