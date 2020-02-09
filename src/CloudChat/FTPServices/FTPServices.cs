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
    public static class FTPServices
    {
        public static async Task<string> UploadAsync(string fileUri, FileType fileType, LoginCredentialsDataModel credentials = null, string channelId = null)
        {
            var fileInfo = new FileInfo(fileUri);
            var extension = fileInfo.Extension.ToLower();
            string uri = "";
            string fileName = "";

            switch (fileType)
            {
                case FileType.ProfilePicture:
                    fileName = "ProfilePic_" + Guid.NewGuid().ToString() + extension;
                    uri = FrameworkDI.Configuration["KommunikativIlovaServer:FtpUrl"] + "/ProfilePics/" + fileName;
                    break;
                case FileType.Attachment:
                    fileName = "Attachment_" + Guid.NewGuid().ToString() + extension;
                    uri = FrameworkDI.Configuration["KommunikativIlovaServer:FtpUrl"] + "/Attachments/" + fileName;
                    break;
                default:
                    Debugger.Break();
                    break;
            }
            // Create FTP request
            var reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));

            // Method
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.Credentials = new NetworkCredential("kuvondik", "12345");

            int length = 2048;
            FileStream file = fileInfo.OpenRead();
            try
            {
                Stream ftpStream = reqFTP.GetRequestStream();
                var buffer = new byte[length];
                int bytesRead = 0;
                do
                {
                    bytesRead = file.Read(buffer, 0, length);
                    ftpStream.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
                file.Close();
                ftpStream.Close();

                if (fileType == FileType.ProfilePicture)
                {

                    var result = await WebRequests.PostAsync<ApiResponse>(
                        RouteHelpers.GetAbsoluteUrl(ApiRoutes.UpdateUserProfile),
                        new UpdateUserProfileApiModel
                        {
                            ProfilePictureName = fileName,
                        },
                        bearerToken: credentials?.Token);

                    //If the response has an error...
                    if (await result.DisplayErrorIfFailedAsync("Login Failed"))
                        //We are done
                        return null;

                    credentials.ProfilePicName = fileName;
                    await ClientDataStore.SaveLoginCredentialsAsync(credentials);
                }

                if (fileType == FileType.Attachment)
                {

                    var result = await WebRequests.PostAsync<ApiResponse<MessageApiModel>>(
                        RouteHelpers.GetAbsoluteUrl(ApiRoutes.AddMessage),
                        new MessageApiModel
                        {
                            SenderUsername = credentials?.Username,
                            ConversationChannelId = channelId,
                            AttachmentUrl = fileName,
                        },
                        bearerToken: credentials.Token);
                }
            }
            catch (Exception ex)
            {
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Upload Error",
                    Message = ex.Message
                });
                return null;
            }

            return fileName;
        }
        public static async Task<MessageApiModel> UploadAttachmentAsync(string fileUri, LoginCredentialsDataModel credentials = null, string channelId = null)
        {
            var fileInfo = new FileInfo(fileUri);
            var extension = fileInfo.Extension.ToLower();

            var fileName = "Attachment_" + Guid.NewGuid().ToString() + extension;
            var uri = FrameworkDI.Configuration["KommunikativIlovaServer:FtpUrl"] + "/Attachments/" + fileName;

            // Create FTP request
            var reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));

            // Method
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.Credentials = new NetworkCredential("kuvondik", "12345");

            int length = 2048;
            FileStream file = fileInfo.OpenRead();
            try
            {
                Stream ftpStream = reqFTP.GetRequestStream();
                var buffer = new byte[length];
                int bytesRead = 0;
                do
                {
                    bytesRead = file.Read(buffer, 0, length);
                    ftpStream.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
                file.Close();
                ftpStream.Close();


                var result = await WebRequests.PostAsync<ApiResponse<MessageApiModel>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.AddMessage),
                    new MessageApiModel
                    {
                        SenderUsername = credentials?.Username,
                        ConversationChannelId = channelId,
                        AttachmentUrl = fileName,
                    },
                    bearerToken: credentials.Token);

                await DownloadAsync(fileName, FileType.Attachment);
                return result.ServerResponse.Response;
            }
            catch (Exception ex)
            {
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Upload Error",
                    Message = ex.Message
                });
                return null;
            }
        }

        public static async Task<bool> DownloadAsync(string fileName, FileType fileType)
        {
            var localPath = Directory.GetCurrentDirectory() + "\\Cache\\";
            var downloadFile = localPath + fileName;
            if (File.Exists(downloadFile))
                return true;

            // Create a WebClient
            var request = new WebClient();

            // Setup our credentials
            request.Credentials = new NetworkCredential("kuvondik", "12345");
            var initialPath = "";
            try
            {
                switch (fileType)
                {
                    case FileType.Attachment:
                        initialPath = "Attachments/";
                        break;
                    case FileType.ProfilePicture:
                        initialPath = "ProfilePics/";
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
                // Download the data into a Byte array
                byte[] fileData = await request.DownloadDataTaskAsync(
                    new Uri(FrameworkDI.Configuration["KommunikativIlovaServer:FtpUrl"]+"/"+ initialPath + fileName));


                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                var file = File.Create(downloadFile);
                await file.WriteAsync(fileData, 0, fileData.Length);
                file.Close();
            }
            catch (Exception ex)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Download Error",
                    Message = ex.Message,
                });
            }
            return true;
        }
        public static async Task<bool> DownloadThumbAsync(string fileName, FileType fileType)
        {
            var localPath = Directory.GetCurrentDirectory() + "\\Cache\\";
            var downloadFile = localPath + fileName;
            if (File.Exists(downloadFile))
                return true;

            // Create a WebClient
            var request = new WebClient();
            // Setup our credentials
            request.Credentials = new NetworkCredential("kuvondik", "12345");
            var initialPath = "";
            try
            {
                switch (fileType)
                {
                    case FileType.Attachment:
                        initialPath = "Attachments/Thumb/";
                        break;
                    case FileType.ProfilePicture:
                        initialPath = "ProfilePics/Thumb/";
                        break;
                    default:
                        Debugger.Break();
                        break;
                }

                // Download the data into a Byte array
                byte[] fileData = await request.DownloadDataTaskAsync(
                                                 new Uri(FrameworkDI.Configuration["KommunikativIlovaServer:FtpUrl"] +"/" + initialPath + fileName));

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                var file = File.Create(downloadFile);
                await file.WriteAsync(fileData, 0, fileData.Length);
                file.Close();
            }
            catch (Exception ex)
            {
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Download Error",
                    Message = ex.Message,
                });
            }
            return true;
        }
    }
}
