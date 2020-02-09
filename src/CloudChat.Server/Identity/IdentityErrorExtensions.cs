using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudChat.Server
{
    /// <summary>
    /// Manages the standart web pages
    /// </summary>
    public static class IdentityErrorExtensions
    {
        public static string AgregateErrors(this IEnumerable<IdentityError> errors)
        {
            //Get all errors into a list
            return errors?.ToList()
                          // Grab their description
                          .Select(f => f.Description)
                          //Combine them with a separator
                          .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
        }

    }
}
