using NoteProject.Dto.Common;

namespace NoteProject.PicServiice.Queries.PicUrl
{
    public class GetPicUrlService
    {

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
                Data =picUrl!=null ? DomainDto.name + picUrl :null,
                IsSuccess = true
            };

        }
    }
}
