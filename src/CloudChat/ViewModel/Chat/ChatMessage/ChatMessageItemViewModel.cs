using CloudChat.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CloudChat
{
    public class ChatMessageItemViewModel : BaseViewModel
    {
        public string SenderUsername { get; set; }
        public string Message { get; set; }
        public string Initials { get; set; }
        public string ProfilePictureRGB { get; set; }
        public EmojiViewModel Emoji { get; set; }
        //public bool IsSelected { get; set; }
        public bool IsSentByMe { get; set; }
        public bool Deleting { get; set; } = false;
        public bool IsMessageRead => MessageReadTime != null;
        public string MessageReadTime { get; set; }
        public string MessageSentTime { get; set; }
        public ChatMessageItemImageAttachmentViewModel ImageAttachment { get; set; }
        public bool HasMessage => Message != null;
        public bool HasImageAttachment => ImageAttachment != null;
        public bool IsDownloading => HasImageAttachment ? ImageAttachment.IsDownloading : false;

        public ICommand DeleteMessageCommand { get; set; }
        public ICommand ReplyMessageCommand { get; set; }
        public ICommand MultiSelectCommand { get; set; }
        public ICommand ForwardMessageCommand { get; set; }
        public ICommand ReportMessageCommand { get; set; }

        public ChatMessageItemViewModel()
        {
            DeleteMessageCommand = new RelayParameterizedCommand((parameter) => DeleteMessage(parameter));
            ReplyMessageCommand = new RelayParameterizedCommand((parameter) => ReplyMessage(parameter));
            MultiSelectCommand = new RelayParameterizedCommand((parameter) => MultiSelect(parameter));
            ForwardMessageCommand = new RelayParameterizedCommand((parameter) => ForwardMessage(parameter));
            ReportMessageCommand = new RelayParameterizedCommand((parameter) => ReportMessage(parameter));
        }

        public void ReportMessage(object parameter)
        {
            //throw new NotImplementedException();
        }

        public void ForwardMessage(object parameter)
        {
            //throw new NotImplementedException();
        }

        public void MultiSelect(object parameter)
        {
           // throw new NotImplementedException();
        }

        public void ReplyMessage(object parameter)
        {
           // throw new NotImplementedException();
        }

        public void DeleteMessage(object parameter)
        {
            CoreDI.TaskManager.Run(async () =>
            {
                await Task.Delay(1000);
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Deleting = true;

                    //DI.ChatMessageList.FilteredItems.Remove(this);
                    //DI.ChatMessageList.Items.Remove(this);
                });
            });

            
        }
    }
}