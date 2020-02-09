using Dna;
using CloudChat.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static CloudChat.DI;

namespace CloudChat
{
    public class ChatListItemViewModel : BaseViewModel
    {
        #region Public Properties

        public List<string> UsernameList { get; set; }
        public ConversationType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string MessageDate { get; set; }
        public string Initials { get; set; }
        public string ProfilePictureRGB { get; set; }
        public bool NewContentAvailable { get; set; }
        public uint NewContentsNumber { get; set; }
        public bool IsNotRead { get; set; }
        public bool IsSelected { get; set; }
        public string ChannelId { get; set; }
        public bool MessagesAreLoading { get; set; }

        public bool Deleting { get; set; } = false;
        #endregion

        #region Public Commands

        public ICommand OpenMessageCommand { get; set; }
        public ICommand DeleteChatCommand { get; set; }
        #endregion

        #region Constructor
        public ChatListItemViewModel()
        {
            OpenMessageCommand = new RelayCommand(OpenMessage);
            DeleteChatCommand = new RelayCommand(async () => await DeleteChatAsync());
        }
        #endregion

        public async Task DeleteChatAsync()
        {
            // Delete from UI
            Deleting = true;
            await Task.Delay(200);
            DI.ChatList.Items.Remove(this);

            // TODO: Delete from the server
        }

        public void OpenMessage()
        {
            if (!IsSelected)
            {
                // Clear other thread messages
                ChatMessageList.Items.Clear();
                ChatMessageList.FilteredItems.Clear();

                ChatMessageList.ReceiverUsernamesList = new ObservableCollection<string>(UsernameList);
                ChatMessageList.ChannelId = ChannelId;
                ChatMessageList.Title = Title;

                if (!ViewModelApplication.ChatPageVisible)
                    ViewModelApplication.ChatPageVisible = true;

                // Collapse all UI object like Attachment menu,...
                ChatMessageList.AttachmentMenuVisible = false;
                ChatMessageList.EmojiListVisible = false;

                if (ChatList.SelectedBefore != null)
                    ChatList.SelectedBefore.IsSelected = false;
                ChatList.SelectedBefore = this;
                IsSelected = true;

                // If new messages are available, newContentNumber will be zero
                NewContentsNumber = 0;

                RunCommandAsync(() => MessagesAreLoading, async () =>
                {
                    await DownloadMessagesAsync();
                })
                .ContinueWith(t =>
                {
                    CheckNewMessages();
                });
            }
        }
        public async Task DownloadMessagesAsync()
        {
            var credentials = await ClientDataStore.GetLoginCredentialsAsync();

            // Sets ChatMessageList Statusbar Receiver
            ChatMessageList.SenderUsername = credentials?.Username;
            ChatMessageList.SenderFirstName = credentials?.FirstName;
            ChatMessageList.SenderLastName = credentials?.LastName;

            var request = await WebRequests.PostAsync<ApiResponse<IEnumerable<MessageApiModel>>>(
                RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetMessages),
                ChannelId,
                bearerToken: credentials?.Token);

            var messages = request.ServerResponse?.Response;
            Application.Current.Dispatcher.Invoke(() =>
                {
                    messages?.ToList().ForEach(m =>
                    {
                        var chatMessageItemViewModel = new ChatMessageItemViewModel
                        {
                            Message = m.Message,
                            SenderUsername = m.SenderUsername,
                            Initials = m.SenderUsername[0].ToString(),
                            MessageSentTime = TimeConverter.GetShortTime(m.CreatedAt),
                            ImageAttachment = m.AttachmentThumbUrl != null ? new ChatMessageItemImageAttachmentViewModel
                            {
                                FileThumbnailName = m.AttachmentThumbUrl,
                                FileName = m.AttachmentUrl,
                                IsSentByMe = m.SenderUsername == ChatMessageList.SenderUsername,
                            } : null,
                            IsSentByMe = m.SenderUsername == ChatMessageList.SenderUsername,
                            ProfilePictureRGB = ProfilePictureRGB,
                        };
                        ChatMessageList.Items.Add(chatMessageItemViewModel);
                        ChatMessageList.FilteredItems.Add(chatMessageItemViewModel);
                    });
                });
        }
        public void CheckNewMessages()
        {
            CoreDI.TaskManager.Run(async () =>
            {
                var credentials = await ClientDataStore.GetLoginCredentialsAsync();

                while (true)
                {
                    if (ViewModelApplication.IsLoggedOut)
                        break;

                    if (credentials == null)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    var request = await WebRequests.PostAsync<ApiResponse<IEnumerable<MessageApiModel>>>(
                       RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetNewMessages),
                       new GetNewMessageApiModel
                       {
                           Username = credentials?.Username,
                           ConversationChannelId = ChannelId,
                       },
                       bearerToken: credentials?.Token);

                    if (!request.Successful || request.ServerResponse == null || request.ServerResponse.Response == null)
                    {
                        await Task.Delay(100);
                        continue;
                    }
                    var newMessages = request.ServerResponse?.Response;
                    if (newMessages.Count() == 0)
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        newMessages?.ToList().ForEach(m =>
                        {
                            var chatMessageItemViewModel = new ChatMessageItemViewModel
                            {
                                Message = m.Message,
                                SenderUsername = m.SenderUsername,
                                Initials = m.SenderUsername[0].ToString(),
                                MessageSentTime = TimeConverter.GetShortTime(m.CreatedAt),
                                ImageAttachment = m.AttachmentThumbUrl != null ? new ChatMessageItemImageAttachmentViewModel
                                {
                                    IsSentByMe = m.SenderUsername == credentials?.Username,
                                    FileThumbnailName = m.AttachmentThumbUrl,
                                    FileName = m.AttachmentUrl,
                                } : null,
                                IsSentByMe = m.SenderUsername == credentials?.Username,
                                ProfilePictureRGB = ProfilePictureRGB,
                            };
                            ChatMessageList.Items.Add(chatMessageItemViewModel);
                            ChatMessageList.FilteredItems.Add(chatMessageItemViewModel);
                        });
                    });
                    await Task.Delay(100);
                }
            });

        }
    }
}



