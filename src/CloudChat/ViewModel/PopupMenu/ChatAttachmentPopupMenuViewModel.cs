using CloudChat.Core;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using static CloudChat.DI;

namespace CloudChat
{
    public class ChatAttachmentPopupMenuViewModel : BasePopupMenuViewModel
    {
        #region Contructor

        public ChatAttachmentPopupMenuViewModel()
        {
            Content = new MenuViewModel
            {
                Items = new List<MenuItemViewModel>
                {
                     new MenuItemViewModel
                     {
                         Type =MenuItemType.Header,
                         Text ="Attach a file...",
                     },
                     new MenuItemViewModel
                     {
                         Text ="From computer",
                         Icon =IconType.FileManager,
                         OptionCommand=new RelayCommand(FromComputer),
                     },
                     new MenuItemViewModel
                     {
                         Text ="From pictures",
                         Icon =IconType.Picture,
                         OptionCommand = new RelayCommand(FromPictures),
                     },
                }
            };
        }
        #endregion Contructor

        private void FromComputer()
        {
            var ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Only Image Files for the moment (*.jpg;*.bmp;*.png;*.ico;*gif)|*.jpg;*.bmp;*.png;*.ico;*gif;",
            };

            if (ofd.ShowDialog() == true)
            {
                ChatMessageList.AttachmentMenuVisible = false;
                CoreDI.TaskManager.Run(async () =>
                {
                    var credentials = await ClientDataStore.GetLoginCredentialsAsync();

                    var messageApiModel = await FTPServices.UploadAttachmentAsync(ofd.FileName,
                        credentials: credentials,
                        channelId: ChatList.SelectedBefore.ChannelId);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var chatMessageItemViewModel = new ChatMessageItemViewModel
                        {
                            ImageAttachment = new ChatMessageItemImageAttachmentViewModel
                            {
                                //Oreder is important
                                IsSentByMe = true,
                                FileName = messageApiModel.AttachmentUrl,
                            },
                            IsSentByMe = true,
                            SenderUsername = messageApiModel.SenderUsername,
                            Initials = (messageApiModel.SenderUsername)[0].ToString(),
                            ProfilePictureRGB = new RgbValue().GetRandomRgbValue(),
                            MessageSentTime = TimeConverter.GetShortTime(messageApiModel.CreatedAt),
                        };

                        ChatMessageList.Items.Add(chatMessageItemViewModel);
                        ChatMessageList.FilteredItems.Add(chatMessageItemViewModel);
                    });
                });
            }
        }

        private void FromPictures()
        {
            UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Sorry",
                Message = "Currently, No UI sorry!"
            });
        }


    }
}