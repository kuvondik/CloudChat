using Dna;
using CloudChat.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using static CloudChat.DI;

namespace CloudChat.NewContentChecker
{
    public class NewMessageChecker
    {
        #region Protected Members

        /// <summary>
        /// Flag indicating if this class is disposing
        /// </summary>
        protected bool mDisposing;

        #endregion

        #region Public Properties

        public string Endpoint { get; protected set; }

        public bool Responsive { get; set; }

        #endregion

        public NewMessageChecker(string endpoint, 
                                 int interval, 
                                 LoginCredentialsDataModel credentials,
                                 Microsoft.Extensions.Logging.ILogger logger = null)
        {
            Endpoint = endpoint;

            logger?.LogTraceSource($"NewMessageChecker started for {endpoint}");
            
            Task.Run(async () =>
            {
                while (!mDisposing)
                {
                    // Create defaults
                    var webResponse = default(WebRequestResult<ApiResponse<IEnumerable<MessageApiModel>>>);
                    var exception = default(Exception);

                    try
                    {
                        logger?.LogTraceSource($"HttpEndpointChecker fetching {endpoint}");

                        webResponse = await WebRequests.PostAsync<ApiResponse<IEnumerable<MessageApiModel>>>(
                           RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetNewMessages),
                           new GetNewMessageApiModel
                           {
                               Username = credentials?.Username,
                               //ConversationChannelId = ChannelId,
                           },
                           bearerToken: credentials?.Token);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    var responsive = webResponse != null;
                    var newMessages = webResponse.ServerResponse.Response;

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
                                //ProfilePictureRGB = ProfilePictureRGB,
                            };
                            ChatMessageList.Items.Add(chatMessageItemViewModel);
                            ChatMessageList.FilteredItems.Add(chatMessageItemViewModel);
                        });
                    });
                    
                    logger?.LogTraceSource($"HttpEndpointChecker {endpoint} { (responsive ? "is" : "is not") } responsive");

                    if (responsive != Responsive)
                        Responsive = responsive;

                    if (!mDisposing)
                        await Task.Delay(interval);
                }
            });
        }

        #region Dispose

        public void Dispose()
        {
            mDisposing = true;
        }

        #endregion
    }
}
