namespace CloudChat.Core
{
    public class ApiResponse
    {
        public bool Succesful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public object Response { get; set; }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public new T Response
        {
            get => (T)base.Response;
            set => base.Response = value;
        }
    }
}
