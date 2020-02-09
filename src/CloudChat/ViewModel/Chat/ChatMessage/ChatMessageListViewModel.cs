using Dna;
using CloudChat.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static CloudChat.DI;

namespace CloudChat
{
    public class ChatMessageListViewModel : BaseViewModel
    {
        #region Protected Members

        protected string mLastSearchText;
        protected string mSearchText;
        protected ObservableCollection<ChatMessageItemViewModel> mItems;
        protected bool mSearchIsOpen;
        #endregion

        #region Public Properties

        public ObservableCollection<ChatMessageItemViewModel> FilteredItems { get; set; }
        public ObservableCollection<ChatMessageItemViewModel> Items
        {
            get => mItems;
            set
            {
                if (value == mItems)
                    return;
                mItems = value;
                FilteredItems = new ObservableCollection<ChatMessageItemViewModel>(mItems);
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public ConversationType Type { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderUsername { get; set; }
        public string Title { get; set; }
        public ObservableCollection<string> ReceiverUsernamesList { get; set; }
        public string ChannelId { get; set; }
        public bool AttachmentMenuVisible { get; set; } = false;
        public bool EmojiListVisible { get; set; } = false;
        public bool ChatInfoControlVisible { get; set; } = false;
        public bool AnyPopupVisible { get { return AttachmentMenuVisible || EmojiListVisible; } }
        public bool MessageIsSending { get; set; }
        public ChatAttachmentPopupMenuViewModel AttachmentMenu { get; set; }
        public EmojiListViewModel EmojiList { get; set; }
        public string PendingMessageText { get; set; }
        public string SearchText
        {
            get => mSearchText;
            set
            {
                if (value == mSearchText)
                    return;
                mSearchText = value;
                if (!string.IsNullOrEmpty(SearchText))
                    Search();
            }
        }
        public bool SearchIsOpen
        {
            get => mSearchIsOpen;
            set
            {
                if (value == mSearchIsOpen)
                    return;
                mSearchIsOpen = value;

                if (mSearchIsOpen)
                    SearchText = string.Empty;
                else
                {
                    SearchText = string.Empty;
                    Search();
                }
            }
        }
        #endregion

        #region Public Commands

        public ICommand AttachmentButtonCommand { get; set; }
        public ICommand EmojiButtonCommand { get; set; }
        public ICommand ToFullScreenMessaging { get; set; }
        public ICommand PopupClickawayCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand OpenSearchCommand { get; set; }
        public ICommand CloseSearchCommand { get; set; }
        public ICommand ClearSearchCommand { get; set; }
        public ICommand ChatInfoControlOpenCommand { get; set; }
        public ICommand ScrollUpCommand { get; set; }
        #endregion

        #region Contructor

        public ChatMessageListViewModel()
        {
            FilteredItems = new ObservableCollection<ChatMessageItemViewModel>();
            Items = new ObservableCollection<ChatMessageItemViewModel>();
            ReceiverUsernamesList = new ObservableCollection<string>();

            AttachmentButtonCommand = new RelayCommand(AttachmentButton);
            EmojiButtonCommand = new RelayCommand(EmojiButton);
            PopupClickawayCommand = new RelayCommand(PopupClickaway);
            SendCommand = new RelayParameterizedCommand(parameter => Send(parameter));
            SearchCommand = new RelayCommand(Search);
            OpenSearchCommand = new RelayCommand(OpenSearch);
            CloseSearchCommand = new RelayCommand(CloseSearch);
            ClearSearchCommand = new RelayCommand(ClearSearch);
            ChatInfoControlOpenCommand = new RelayCommand(InfoOpen);
            //ScrollUpCommand = new RelayParameterizedCommand(async(e) => await ScrollUpAsync(e));
            AttachmentMenu = new ChatAttachmentPopupMenuViewModel();
            EmojiList = new EmojiListViewModel();
        }

        //public async Task ScrollUpAsync(object e)
        //{
        //    //if((e as MouseWheelEventArgs).Delta > 20)
        //    //{
        //    //    
        //    //}
        //    await Task.Delay(1);
        //}
        #endregion

        #region Public Methods

        public void SetSenderInfo()
        {
            CoreDI.TaskManager.Run(async () =>
            {
                var credentials = await ClientDataStore.GetLoginCredentialsAsync();
                SenderFirstName = credentials?.FirstName;
                SenderLastName = credentials?.LastName;
                SenderUsername = credentials?.Username;
            });
        }
        public void InfoOpen()
        {
            if (Type == ConversationType.Chat)
            {
                ChatInfo.Clear();
                var contact = ContactList.Items.Where(c => ReceiverUsernamesList.Contains(c.Username))
                                               .FirstOrDefault();
                if (contact != null)
                {
                    ViewModelApplication.DimmableOverlayVisible = true;
                    ViewModelApplication.ChatInfoControlVisible = true;
                    ChatInfo.FirstName = contact.FirstName;
                    ChatInfo.LastName = contact.LastName;
                    ChatInfo.Email = contact.Email;
                    ChatInfo.PhoneNumber = contact.PhoneNumber;
                    ChatInfo.Username = contact.Username;

                    ChatInfo.PictureThumbnailName = contact.PictureThumbName;
                    ChatInfo.PictureUrl = contact.PictureUrl;

                    // TODO: Change this for blocking system
                    ChatInfo.SendMessageAvailable = true;
                }
                else
                    CoreDI.TaskManager.Run(async () =>
                    {
                        ViewModelApplication.DimmableOverlayVisible = true;
                        ViewModelApplication.ChatInfoControlVisible = true;
                        var credentials = await ClientDataStore.GetLoginCredentialsAsync();

                        // Gets the username of whom is chatting with
                        var userName = ReceiverUsernamesList.Where(u => u != credentials?.Username)
                                                            .FirstOrDefault();
                        var request = await WebRequests.PostAsync<ApiResponse<UserApiModel>>(
                            RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetUserInfo),
                            userName,
                            bearerToken: credentials?.Token);

                        if (await request.DisplayErrorIfFailedAsync("You can't see this profile"))
                            return;

                        var userApiModel = request.ServerResponse.Response;

                        ChatInfo.FirstName = userApiModel.FirstName;
                        ChatInfo.LastName = userApiModel.LastName;
                        ChatInfo.Email = userApiModel.Email;
                        ChatInfo.PhoneNumber = userApiModel.PhoneNumber;
                        ChatInfo.Username = userApiModel.Username;
                        ChatInfo.PictureThumbnailName = userApiModel.ProfilePictureThumbName;
                        ChatInfo.PictureUrl = userApiModel.ProfilePictureName;

                        ChatInfo.SendMessageAvailable = true;
                    });
            }

            if(Type == ConversationType.Group)
            {
                // TODO: Group Info Logic
            }
        }
        public void Search()
        {
            if (string.IsNullOrEmpty(SearchText) && string.IsNullOrEmpty(mLastSearchText) ||
                string.Equals(SearchText, mLastSearchText))
                return;
            if (string.IsNullOrEmpty(SearchText) || Items == null || Items.Count <= 0)
            {
                FilteredItems = new ObservableCollection<ChatMessageItemViewModel>();
                mLastSearchText = SearchText;
            }

            FilteredItems = new ObservableCollection<ChatMessageItemViewModel>(
                Items.Where(item => (item.Message ?? "").ToLower().Contains(SearchText.ToLower())));
            mLastSearchText = SearchText;
        }
        public void ClearSearch()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = string.Empty;
                Search();
            }
            else
            {
                SearchIsOpen = false;
            }
        }
        public void CloseSearch()
        {
            SearchIsOpen = false;
        }
        public void OpenSearch()
        {
            SearchIsOpen = true;
        }
        public void Send(object parameter)
        {
            ChatMessageItemViewModel message=null;
            
            // Collapse all UI object like Attachment menu,...
            AttachmentMenuVisible = false;
            EmojiListVisible = false;

            if (string.IsNullOrEmpty(PendingMessageText))
                return;
            if (string.IsNullOrEmpty(PendingMessageText.Trim()))
                return;

            RunCommandAsync(() => MessageIsSending, async () =>
            {
                // Adds a chat if it's new from (it comes contacts) ... 
                if (ChatList.SelectedBefore == null)
                  {
                      // Adding a chat to the server
                      var conversationApiModel = await CreateNewChatAsync(ReceiverUsernamesList);

                      // Adding a chat to ChatList UI
                      var newChat = new ChatListItemViewModel
                      {
                          UsernameList = conversationApiModel.UsernameList,
                          Title = conversationApiModel.Title,
                          Initials = conversationApiModel.Title[0].ToString(),
                          ProfilePictureRGB = new RgbValue().GetRandomRgbValue(),
                          ChannelId = conversationApiModel.ChannelId,
                          Message = PendingMessageText.Trim(),
                          MessageDate =TimeConverter.GetShortTime(conversationApiModel.UpdatedAt),
                      };

                      ChannelId = newChat.ChannelId;

                      ChatList.Items.Add(newChat);
                      newChat.OpenMessage();
                  }

                message = new ChatMessageItemViewModel
                {
                    Initials = SenderUsername[0].ToString(),
                    Message =string.Copy(PendingMessageText.Trim()),
                    IsSentByMe = true,
                    ProfilePictureRGB = new RgbValue().GetRandomRgbValue(),
                    SenderUsername = SenderUsername,
                };

                PendingMessageText = string.Empty;
                
                // TODO: Changing message on ChatListItem
                
                // Adding Message to ChatMessageList UI
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FilteredItems.Add(message);
                    Items.Add(message);
                });
                
            })
            .ContinueWith(async t =>
            {
                //Adding a message to the server and getting response from server
                var messageApimModel = await SendMessageToServerAsync(message);
            });

            // Scrolling down ChatMessageList
            (parameter as ChatMessageListControl).scrollViewver.ScrollToEnd();
        }
        public void PopupClickaway()
        {
            AttachmentMenuVisible = false;
            EmojiListVisible = false;
        }
        public void AttachmentButton()
        {
            AttachmentMenuVisible ^= true;
            EmojiListVisible = false;
        }
        public void EmojiButton()
        {
            AttachmentMenuVisible = false;
            EmojiListVisible ^= true;
        }
        public void ClearChatPage()
        {
            Items = new ObservableCollection<ChatMessageItemViewModel>();
            ReceiverUsernamesList = new ObservableCollection<string>();
            Title = "...";
        }
        public async Task<ConversationApiModel> CreateNewChatAsync(ObservableCollection<string> usernames)
        {
            var credentials = await ClientDataStore.GetLoginCredentialsAsync();

            // Adds first own username
            var participants = new List<ParticipantApiModel>
            {
                new ParticipantApiModel
                {
                    UserUsername = credentials.Username,
                },
            };
            // then, adds others username
            foreach (var username in usernames)
            {
                participants.Add(new ParticipantApiModel
                {
                    UserUsername = username,
                });
            }

            var conversation = new ConversationApiModel
            {
                CreatorUsername = credentials.Username,
                ChannelId = Guid.NewGuid().ToString(),
                Type = ConversationType.Chat,
                PreMessage = PendingMessageText.Trim(),
            };

            var request = await WebRequests.PostAsync<ApiResponse<ConversationApiModel>>(
                RouteHelpers.GetAbsoluteUrl(ApiRoutes.AddConversation),
                new AddConversationRequestApiModel
                {
                    Participants = participants,
                    Conversation = conversation,
                },
                bearerToken: credentials?.Token);

            //If the response has an error...
            if (await request.DisplayErrorIfFailedAsync("Starting Chat Failed"))
                //We are done
                return null;

            return request.ServerResponse.Response;
        }
        #endregion

        #region Private Helpers

        private async Task<MessageApiModel> SendMessageToServerAsync(ChatMessageItemViewModel message)
        {
            var credentials = await ClientDataStore.GetLoginCredentialsAsync();

            var request = await WebRequests.PostAsync<ApiResponse<MessageApiModel>>(
                RouteHelpers.GetAbsoluteUrl(ApiRoutes.AddMessage),
                new MessageApiModel
                {
                    SenderUsername = SenderUsername,
                    ConversationChannelId = ChannelId,
                    Message = message.Message,
                },
                bearerToken: credentials?.Token);

            
            //If the response has an error...
            if (await request.DisplayErrorIfFailedAsync("Sending Message Failed"))
                //We are done
                return null;

            //Setting sent time 
            message.MessageSentTime =TimeConverter.GetShortTime(request.ServerResponse.Response.CreatedAt);
            return request.ServerResponse.Response;
        }
        #endregion
    }
}