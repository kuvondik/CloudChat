using Dna;
using CloudChat.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static CloudChat.DI;

namespace CloudChat
{
    public static class WebRequestResultExtensions
    {
        /// <summary>
        /// Checks the web request result for any errors, displaying them if there are any
        /// </summary>
        /// <param name="response"></param>
        /// <param name="title"></param>
        /// <returns>Returns true if there was an error, or false if all was OK.</returns>
        public static async Task<bool> DisplayErrorIfFailedAsync(this WebRequestResult response, string title)
        {
            if (response == null || response.ServerResponse == null || !(response.ServerResponse as ApiResponse).Succesful)
            {
                // Default error message
                // TODO: Localize strings
                var message = "Unknown error from server call";

                // If we got a response from server...
                if (response?.ServerResponse is ApiResponse apiResponse)
                    // Set message to server response
                    message = apiResponse.ErrorMessage;
                // If we have result  but deserialize failed...
                else if (!string.IsNullOrWhiteSpace(response?.RawServerResponse))
                    // Set error message
                    message = $"Unexpected response from server. {response.RawServerResponse}";
                // If we have a result but no server response details at all...
                else if (response != null)
                    // Set message to standart HTTP server response details
                    message = response.ErrorMessage ?? $"{response.StatusDescription} ({response.StatusCode})";

                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    FrameworkDI.Logger.LogInformationSource("Log the user out due to unauthorized response from server");
                    // Automatically log the user out
                    await AccountInfo.LogoutAsync();
                    ViewModelApplication.IsLoggedOut = true;
                }
                else
                {
                    // Display error
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = title,
                        Message = message
                    });
                }

                return true;
            }
            return false;
        }
    }
}