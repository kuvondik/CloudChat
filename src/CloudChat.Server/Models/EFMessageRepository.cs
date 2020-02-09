using CloudChat.Core;
using CloudChat.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public class EFMessageRepository : IDataRepository
    {
        #region Protected Members

        protected ChatMessageDbContext mContext;
        #endregion

        #region Contructor

        public EFMessageRepository(ChatMessageDbContext context)
        {
            mContext = context;
        }
        #endregion

        #region Contact
        // vvv
        public async Task<ContactApiModel> AddNewContactAsync(ContactApiModel newContact)
        {
            var user = await mContext.GetUserByEmailAsync(newContact.Email);

            if (user == null)
                return null;

            var contact = new ContactsEntity
            {
                User = user,
                Owner = await mContext.GetUserByUsernameAsync(newContact.OwnerUsername),
            };

            await mContext.Contacts.AddAsync(contact);
            await mContext.SaveChangesAsync();

            newContact.Username = user.Username;
            newContact.FirstName = user.FirstName;
            newContact.LastName = user.LastName;
            //newContact.PictureName = await mContext.GetUserProfilePictureUrlAsync(user);
           // newContact.PictureThumbName = await mContext.GetUserProfilePictureUrlAsync(user);

            return newContact;
        }

        // vvv
        public async Task<IEnumerable<ContactApiModel>> GetContactsAsync(string ownerUsername)
        {
            var contactApiModels = mContext.Contacts.Where(c => c.Owner.Username == ownerUsername)
                                    .OrderBy(contact => contact.User.FirstName ?? contact.User.LastName)
                                    .Select(contact=>contact.User)
                                    .Select(user =>
                                    new ContactApiModel
                                    {
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        Username = user.Username,
                                        PhoneNumber = user.Phone,
                                        Email = user.Email,
                                    }).ToList();

            await mContext.AssignContactPicturesAsync(contactApiModels);
            return contactApiModels;
        }

        #endregion

        #region Conversation

        // vvv
        public async Task<ConversationApiModel> AddNewConversationAsync(AddConversationRequestApiModel addConversationRequest)
        {
            var conversationApiModel = addConversationRequest.Conversation;
            var participantApiModels = addConversationRequest.Participants;
            var conversation = new ConversationsEntity
            {
                Title = conversationApiModel.Title,
                Type = conversationApiModel.Type,
                ChannelId = conversationApiModel.ChannelId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Creator = await mContext.GetUserByUsernameAsync(conversationApiModel.CreatorUsername),
            };

            // Adds Participants to database
            await mContext.Conversations.AddAsync(conversation);

            // Adds Participants to database
            await mContext.AddParticipantsToConversationAsync(participantApiModels, conversation);

            await mContext.SaveChangesAsync();

            conversationApiModel.Id = conversation.Id;

            var user = await mContext.Participants.Where(p => p.Conversation.Id == conversation.Id &&
                                                              p.User.Username != conversation.Creator.Username)
                                                  .Select(p=>p.User)
                                                  .FirstOrDefaultAsync();

            // Gets the title by Conversation Type
            conversationApiModel.Title = GetTitle(conversation,
                                                  conversation.Title,
                                                  user.FirstName,
                                                  user.LastName,
                                                  user.Username);

            conversationApiModel.CreatedAt = conversation.CreatedAt;
            conversationApiModel.UpdatedAt = conversation.UpdatedAt;
            conversationApiModel.CreatorUsername = conversation.Creator.Username;
            conversationApiModel.AddParticipantsUsername(participantApiModels);

            // Add new conversations to local variable
            DI.NewConversations.Add(conversationApiModel);

            //TODO: Must be changed
            DI.ConversationsDictionary.Add(conversationApiModel.Id, conversationApiModel.UsernameList);
            // and Delete after 5 seconds
            DeleteLocalSavedConversation(conversationApiModel);
            return conversationApiModel;
        }

        // vvv
        public async Task<IEnumerable<ConversationApiModel>> GetConversationsAsync(string username)
        {
            await Task.Delay(1);
            var conversationDict = DI.ConversationsDictionary.Where(d => d.Value.Contains(username))
                                                             .FirstOrDefault();
            if (conversationDict.Key != null)
            {
                // Removing local Conversations
                DI.NewConversations.RemoveAll(c => c.Id == conversationDict.Key);
                // Removing from Local Coversation Dictionary
                DI.ConversationsDictionary.Remove(conversationDict.Key);
            }

            var participantDetails = new List<ParticipantDetails>();

            var conversationApiModels = new List<ConversationApiModel>();

           return mContext.Participants.Where(p => p.User.Username == username)
                                       .OrderByDescending(p => p.Conversation.UpdatedAt)
                                       //.Select(p =>new ParticipantDetails
                                       //{
                                       //     Participant=p,
                                       //     Conversation=p.Conversation,
                                       //     Creator = p.Conversation.Creator,
                                       //})
                                       .Select(p => new ConversationApiModel
                                       {
                                           ChannelId = p.Conversation.ChannelId,
                                           CreatedAt = p.Conversation.CreatedAt,
                                           UpdatedAt = p.Conversation.UpdatedAt,
                                           Type = p.Conversation.Type,
                                           Title = mContext.Participants.Where(partic => partic.Conversation.Id == p.Conversation.Id &&
                                                                                         partic.User!=p.User)
                                                                        .Select(partic => GetTitle(partic.Conversation,
                                                                                                   partic.Conversation.Title,
                                                                                                   partic.User.FirstName,
                                                                                                   partic.User.LastName,
                                                                                                   partic.User.Username))
                                                                        .FirstOrDefault(),
                                           CreatorUsername = p.Conversation.Creator.Username,
                                           UsernameList = mContext.Participants.Where(partic => partic.Conversation.Id == p.Conversation.Id)
                                                   .Select(partic => partic.User.Username)
                                                   .ToList(),
                                           PreMessage = mContext.Messages.Where(message => message.Conversation.ChannelId == p.Conversation.ChannelId)
                                               .OrderBy(message => message.CreatedAt)
                                               .Select(message => message.Message)
                                               .LastOrDefault(),

                                       });

           // return conversationApiModels;
        }

        // vvv
        public async Task<IEnumerable<ConversationApiModel>> GetNewConversationsAsync(string username)
        {
            await Task.Delay(1);
            // Gets key of dictionary as conversation Id
            var conversationDict = DI.ConversationsDictionary.Where(d => d.Value.Contains(username))
                                                             .FirstOrDefault();
            if (conversationDict.Key == null)
                return null;

            var newConversations = DI.NewConversations.Where(c => c.Id == conversationDict.Key).DeepCopy();

            // Removing local Conversations
            DI.NewConversations.RemoveAll(c => c.Id == conversationDict.Key);
            // Removing from Local Coversation Dictionary
            DI.ConversationsDictionary.Remove(conversationDict.Key);

            return newConversations;
        }

        #endregion

        //vvv
        public async Task<UserApiModel> GetUserInfoAsync(string username)
        {
            var user = await mContext.GetUserByUsernameAsync(username);

            return new UserApiModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.Phone,
                Username = user.Username,
                ProfilePictureThumbName = await mContext.GetUserProfilePictureThumbUrlAsync(user),
                ProfilePictureName = await mContext.GetUserProfilePictureUrlAsync(user),
            };
        }

        #region Message
        // vvv
        public async Task<MessageApiModel> AddNewMessageAsync(MessageApiModel chatMessageApiModel)
        {
            var message = new MessagesEntity
            {
                Message = chatMessageApiModel.Message,
                MessageType = chatMessageApiModel.MessageType,
                Sender = await mContext.GetUserByUsernameAsync(chatMessageApiModel.SenderUsername),
                Conversation = await mContext.GetConversationByChannelId(chatMessageApiModel.ConversationChannelId),
                CreatedAt = DateTimeOffset.UtcNow,
                Participant = await mContext.GetParticipantByUsername(chatMessageApiModel.SenderUsername),
                AttachmentUrl = chatMessageApiModel.AttachmentUrl,
                AttachmentThumbUrl = chatMessageApiModel.AttachmentUrl != null ? await ThumbCreator.CreateThumbnailAndUpload(chatMessageApiModel.AttachmentUrl, FileType.Attachment) : null,
            };

            await mContext.Messages.AddAsync(message);
            await mContext.SaveChangesAsync();

            chatMessageApiModel.AttachmentThumbUrl = message.AttachmentThumbUrl;
            chatMessageApiModel.CreatedAt = message.CreatedAt;


            // Add new message to local variable
            DI.NewMessages.Add(chatMessageApiModel);
            // and Delete after 5 seconds
            DeleteLocalSavedMessage(chatMessageApiModel);

            return chatMessageApiModel;
        }

        // vvv
        public IEnumerable<MessageApiModel> GetMessagesAsync(string channelId)
        {
            DI.NewMessages.RemoveAll(m => m.ConversationChannelId == channelId);

            return mContext.Messages.Where(m => m.Conversation.ChannelId == channelId)
                                    .OrderBy(m => m.CreatedAt)
                                    .Select(m => new MessageApiModel
                                    {
                                        SenderUsername = m.Sender.Username,
                                        Message = m.Message,
                                        CreatedAt = m.CreatedAt,
                                        AttachmentUrl = m.AttachmentUrl,
                                        AttachmentThumbUrl = m.AttachmentThumbUrl,
                                    });
        }

        // vvv
        public async Task<IEnumerable<MessageApiModel>> GetNewMessagesAsync(GetNewMessageApiModel getNewMessage)
        {
            await Task.Delay(1);
            var newMessages = DI.NewMessages.Where(m => m.ConversationChannelId == getNewMessage.ConversationChannelId &&
                                                        m.SenderUsername != getNewMessage.Username).DeepCopy();

            DI.NewMessages.RemoveAll(m => m.ConversationChannelId == getNewMessage.ConversationChannelId &&
                                          m.SenderUsername != getNewMessage.Username);
            return newMessages;
        }


        #endregion
        public async Task DeleteMessageAsync(MessagesEntity messageEntity)
        {
            await Task.Delay(1);
        }
        
        #region Private Helpers
        /// <summary>
        /// Deletes local messageApiModels after 5 seconds
        /// </summary>
        /// <param name="message"></param>
        private async void DeleteLocalSavedMessage(MessageApiModel message)
        {
            await Task.Delay(5000);
            DI.NewMessages.Remove(message);
        }

        /// <summary>
        /// Deletes local conversationApiModels after 5 seconds
        /// </summary>
        /// <param name="conversation"></param>
        private async void DeleteLocalSavedConversation(ConversationApiModel conversation)
        {
            await Task.Delay(5000);
            DI.NewConversations.Remove(conversation);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversationApiModel"></param>
        /// <param name="Title"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private string GetTitle(ConversationsEntity conversation,
                                           string Title,
                                           string firstName,
                                           string lastName,
                                           string username)
        {
            if (conversation.Type == ConversationType.Group)
                return Title;
            else
                return StringHelpers.GetShortName(firstName: firstName,
                                                   lastName: lastName,
                                                   userName: username);
        }
        #endregion
    }
}
