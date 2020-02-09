using System;
using System.Collections.Generic;
using System.Text;

namespace CloudChat.Core
{
    public class StringHelpers
    {
        public static string GetShortName(string firstName, string lastName, string userName)
        {
            if (!string.IsNullOrEmpty(firstName))
                return firstName;
            else
            if (!string.IsNullOrEmpty(lastName))
                return lastName;
            else
                return userName;
        }

        // TODO: Change it for numbers
        public static string GetInitials(string name)
        {
            return name[0].ToString();
        }
    }
}
