using CloudChat.Core;
using System;
using System.Globalization;

namespace CloudChat
{
    public class SideMenuContentConverter : BaseValueConverter<SideMenuContentConverter>
    {
        #region Protected Members

        protected ChatListControl mChatListConrol = new ChatListControl();
        protected ContactListControl mContactList = new ContactListControl();
        #endregion

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (SideMenuContent)value;

            switch (type)
            {
                case SideMenuContent.Chats:
                    return mChatListConrol;
                case SideMenuContent.Contacts:
                    return mContactList;
                default:
                    return "No UI for now!";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}