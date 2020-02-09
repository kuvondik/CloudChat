using Dna;
using CloudChat.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CloudChat.Server
{
    /// <summary>
    /// A shorthand access class to get DI services  with nice clean short code 
    /// </summary>
    public static class DI
    {
        public static ApplicationDbContext ApplicationDbContext => Framework.Provider.GetService<ApplicationDbContext>();
        public static IEmailSender EmailSender = Framework.Provider.GetService<IEmailSender>();
        public static IEmailTemplateSender EmailTemplateSender = Framework.Provider.GetService<IEmailTemplateSender>();
        public static List<MessageApiModel> NewMessages = Framework.Provider.GetService<List<MessageApiModel>>();
        public static List<ConversationApiModel> NewConversations = Framework.Provider.GetService<List<ConversationApiModel>>();
        public static Dictionary<string, IEnumerable<string>> ConversationsDictionary = Framework.Provider.GetService<Dictionary<string, IEnumerable<string>>>();
    }
}
