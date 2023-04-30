using System.Runtime.Serialization;

namespace Demo.DTOs.Response {
    public class ResponseObject<T> {
        public int Status { get; set; }
        public string Message { get; set; }
        public String Code { get; set; }
        public DateTime TimeStamp { get; set; }
        public T Data { get; set; }

        public ResponseObject() {
        }

        public ResponseObject(int status, string message, String code, DateTime time, T data) {
            Status = status;
            Message = message;
            Code = code;
            TimeStamp = time;
            Data = data;
        }

        public ResponseObject(string message, T data)
        {
            Message = message;
            Data = data;
        }

        public ResponseObject<T> responseSuccess(string message, T data) {
            return new ResponseObject<T>(0, message, StatusCodes.Status200OK.ToString(), DateTime.Now, data);
        }

        public ResponseObject<T> responseError(string message, string code, T data) {
            return new ResponseObject<T>(1, message, code, DateTime.Now, data);
        }
    }
}
