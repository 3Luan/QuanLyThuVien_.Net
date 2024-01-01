using System.Net;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Messages { get; set; }
    }

    public class ApiNotFoundResponse : ApiResponse
    {
        public ApiNotFoundResponse(string message)
        {
            Code = (int)HttpStatusCode.NotFound;
            Success = false;
            Result = null;
            Messages = message;
        }
    }

    public class ApiBadResponse : ApiResponse
    {
        public ApiBadResponse(string message)
        {
            Code = (int)HttpStatusCode.BadRequest;
            Success = false;
            Result = null;
            Messages = message;
        }
    }

    public class ApiOkResponse : ApiResponse
    {
        public ApiOkResponse(object data)
        {
            Code = (int)HttpStatusCode.OK;
            Success = true;
            Result = data;
            Messages = string.Empty;
        }
    }
}