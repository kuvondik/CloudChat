using Dna;
using CloudChat.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static CloudChat.DI;

namespace CloudChat
{
    public class ChatListViewModel : BaseViewModel
    {
        #region Public Properties

        public ChatListItemViewModel SelectedBefore { get; set; }
        public ObservableCollection<ChatListItemViewModel> Items { get; set; }
        public bool ChatsAreDownloading { get; set; }
        #endregion

        #region Constructor

        public ChatListViewModel()
        {
            Items = new ObservableCollection<ChatListItemViewModel>();

            DownloadChatList();
            CheckNewChat();
        }
        #endregion

        public void UnselectItem()
        {
            if (SelectedBefore != null)
            {
                SelectedBefore.IsSelected = false;
                SelectedBefore = null;
                ViewModelApplication.ChatPageVisible = false;
            }
        }

        public void DownloadChatList()
        {
            var result = default(bool);
            RunCommandAsync(() => ChatsAreDownloading, async () =>
            {
                var credentials = await DI.ClientDataStore.GetLoginCredentialsAsync();
                var request = await WebRequests.PostAsync<ApiResponse<IEnumerable<ConversationApiModel>>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetConversations),
                    credentials?.Username,
                    bearerToken: credentials?.Token);

                var chatList = request.ServerResponse?.Response;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    chatList?.ToList().ForEach(o =>
                    {
                        Items.Add(
                            new ChatListItemViewModel
                            {
                                Title = o.Title,
                                Message = o.PreMessage,
                                Initials = StringHelpers.GetInitials(o.Title),
                                ProfilePictureRGB = new RgbValue().GetRandomRgbValue(),
                                ChannelId = o.ChannelId,
                                UsernameList = o.UsernameList,
                                MessageDate = TimeConverter.GetShortTime(o.CreatedAt),
                            });
                    });
                    result = true;
                });

            })
            .ContinueWith(t =>
            {
                if (result)
                    // If Chats are uploaded ...
                    DI.ViewModelApplication.ChatsAreDownloaded = true;
            });
        }

        public void CheckNewChat()
        {
            CoreDI.TaskManager.Run(async () =>
            {
                var credentials = await ClientDataStore.GetLoginCredentialsAsync();
                while (true)
                {
                    if (ViewModelApplication.IsLoggedOut)
                        break;

                    var request = await WebRequests.PostAsync<ApiResponse<IEnumerable<ConversationApiModel>>>(
                       RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetNewConversations),
                       credentials?.Username,
                       bearerToken: credentials?.Token);

                    if (!request.Successful || request.ServerResponse == null
                                            || request.ServerResponse.Response == null)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    var newChats = request.ServerResponse?.Response;

                    if (newChats.Count() == 0)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        newChats.ToList().ForEach(c =>
                        {
                            var chatListItemViewModel = new ChatListItemViewModel
                            {
                                Message = c.PreMessage,
                                Title = c.Title,
                                ChannelId = c.ChannelId,
                                UsernameList = c.UsernameList,
                                Initials = c.Title[0].ToString(),
                                MessageDate = TimeConverter.GetShortTime(c.CreatedAt),
                                ProfilePictureRGB = new RgbValue().GetRandomRgbValue(),
                            };
                            ChatList.Items.Add(chatListItemViewModel);
                        });
                    });
                    await Task.Delay(1000);
                }
            });

        }

    }
}