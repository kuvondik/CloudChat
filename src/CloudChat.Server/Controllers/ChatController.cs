using CloudChat.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    [AuthorizeToken]
    public class ChatController : Controller
    {
        #region Protected Members

        protected IDataRepository mRepository;
        #endregion

        #region Constructor

        public ChatController(IDataRepository repository)
        {
            mRepository = repository;
        }
        #endregion

        #region Get / Add conversations and contacts

        [Route("/chat/get/user/info")]
        public async Task<ApiResponse<UserApiModel>> GetUserInfoAsync([FromBody]string username)
        {
            return new ApiResponse<UserApiModel>
            {
                Response = await mRepository.GetUserInfoAsync(username),
            };
        }

        [Route("/chat/get/contacts")]
        public async Task<ApiResponse<IEnumerable<ContactApiModel>>> GetContactsAsync([FromBody]string username)
        {
            return new ApiResponse<IEnumerable<ContactApiModel>>
            {
                Response = await mRepository.GetContactsAsync(username),
            };
        }

        [Route("/chat/add/contact")]
        public async Task<ApiResponse<ContactApiModel>> AddNewContactAsync([FromBody]ContactApiModel contact)
        {
            var response = await mRepository.AddNewContactAsync(contact);
            if (response == null)
                return new ApiResponse<ContactApiModel>
                {
                    ErrorMessage = "There is no any user with this email"
                };
            else
                return new ApiResponse<ContactApiModel>
                {
                    Response = response
                };
        }

        [Route("/chat/get/conversations")]
        public async Task<ApiResponse<IEnumerable<ConversationApiModel>>> GetConversationsAsync([FromBody]string username)
        {
            return new ApiResponse<IEnumerable<ConversationApiModel>>
            {
                Response = await mRepository.GetConversationsAsync(username),
            };
        }

        [Route("/chat/check/new/conversations")]
        public async Task<ApiResponse<IEnumerable<ConversationApiModel>>> GetNewConversationsAsync([FromBody]string username)
        {
            return new ApiResponse<IEnumerable<ConversationApiModel>>
            {
                Response = await mRepository.GetNewConversationsAsync(username),
            };
        }

        [Route("/chat/add/conversation")]
        public async Task<ApiResponse<ConversationApiModel>> AddNewConversationAsync([FromBody]AddConversationRequestApiModel addConversationRequest)
        {
            //var invalidErrorMessage = "You can't send a message to this user";
            
            return new ApiResponse<ConversationApiModel>()
            {
                Response = await mRepository.AddNewConversationAsync(addConversationRequest)
            };
        }

        #endregion

        [Route("/chat/get/messages")]
        public async Task<ApiResponse<IEnumerable<MessageApiModel>>> GetMessagesAsync([FromBody]string channelId)
        {
            await Task.Delay(1);
            return new ApiResponse<IEnumerable<MessageApiModel>>
            {
                Response = mRepository.GetMessagesAsync(channelId),
            };
        }

        [Route("/chat/add/message")]
        public async Task<ApiResponse<MessageApiModel>> AddNewMessage([FromBody]MessageApiModel message)
        {
            return new ApiResponse<MessageApiModel>
            {
                Response = await mRepository.AddNewMessageAsync(message),
            };
        }

        [Route("/chat/check/new/messages")]
        public async Task<ApiResponse<IEnumerable<MessageApiModel>>> GetNewMessagesAsync([FromBody]GetNewMessageApiModel getNewMessage)
        {
            return new ApiResponse<IEnumerable<MessageApiModel>>
            {
                Response = await mRepository.GetNewMessagesAsync(getNewMessage),
            };
        }
    }

}
