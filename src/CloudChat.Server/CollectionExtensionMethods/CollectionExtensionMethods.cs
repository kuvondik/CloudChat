using CloudChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudChat.Server
{
    public static class CollectionExtensionMethods
    {
        /// <summary>
        /// Takes last N elements from collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Given collection</param>
        /// <param name="N">Number of elements that should be taken</param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static IEnumerable<MessageApiModel> DeepCopy(this IEnumerable<MessageApiModel> list)
        {
            var other = new List<MessageApiModel>();
            foreach (var o in list)
                other.Add(o.DeepCopy());

            return other;
        }

        public static IEnumerable<ConversationApiModel> DeepCopy(this IEnumerable<ConversationApiModel> list)
        {
            var other = new List<ConversationApiModel>();
            foreach (var o in list)
                other.Add(o.DeepCopy());

            return other;
        }

        public static Dictionary<string, string> DeepCopy(this Dictionary<string, string> dict)
        {
            var other = new Dictionary<string, string>();

            foreach (var o in dict)
                other.Add(string.Copy(o.Key), string.Copy(o.Value));

            return other;
        }
    }
}
