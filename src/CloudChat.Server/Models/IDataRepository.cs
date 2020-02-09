using CloudChat.Core;
using CloudChat.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CloudChat.Server
{
    public interface IDataRepository
    {
        Task<ContactApiModel> AddNewContactAsync(ContactApiModel newContact);
        Task<IEnumerable<ContactApiModel>> GetContactsAsync(string username);

        Task<UserApiModel> GetUserInfoAsync(string name);

        Task<ConversationApiModel> AddNewConversationAsync(AddConversationRequestApiModel addConversationRequest);
        Task<IEnumerable<ConversationApiModel>> GetConversationsAsync(string username);
        Task<IEnumerable<ConversationApiModel>> GetNewConversationsAsync(string username);

        Task<MessageApiModel> AddNewMessageAsync(MessageApiModel chatMessageApiModel);
        IEnumerable<MessageApiModel> GetMessagesAsync(string channelId);
        Task<IEnumerable<MessageApiModel>> GetNewMessagesAsync(GetNewMessageApiModel getNewMessage);


        Task DeleteMessageAsync(MessagesEntity messageEntity);
    }

}
