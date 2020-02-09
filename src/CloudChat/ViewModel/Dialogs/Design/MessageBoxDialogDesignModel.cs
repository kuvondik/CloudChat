

namespace CloudChat   
{
    public class MessageBoxDialogDesignModel:MessageBoxDialogViewModel
    {
        public static MessageBoxDialogDesignModel Instance = new MessageBoxDialogDesignModel();
        public MessageBoxDialogDesignModel()
        {
            Title="Sending Message";
            OkText = "OK";
            Message = "Thanks for sending a message!";

        }
    }
}
