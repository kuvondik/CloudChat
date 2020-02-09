namespace CloudChat
{
    public class MessageBoxDialogViewModel : BaseDialogViewModel
    {
        public string Message { get; set; }
        public string PicturePath { get; set; }
        public bool HasImage { get; set; }
        public string OkText { get; set; } = "OK";
    }
}
