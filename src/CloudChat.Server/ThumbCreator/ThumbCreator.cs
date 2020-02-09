using CloudChat.Core;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public static class ThumbCreator
    {
        public static async Task<string> CreateThumbnailAndUpload(string fileName, FileType fileType)
        {
            #region Download original pic into MemoryStream and create thumbnail

            var request = new WebClient();
            // Setup our credentials
            request.Credentials = new NetworkCredential("kuvondik", "12345");

            var initialPath = "ftp://localhost:21/";
            switch (fileType)
            {
                case FileType.Attachment:
                    initialPath += "Attachments/";
                    break;
                case FileType.ProfilePicture:
                    initialPath += "ProfilePics/";
                    break;
                default:
                    Debugger.Break();
                    break;
            }

            var fileUrl = initialPath + fileName;

            // Download the data into a Byte array
            byte[] fileData = await request.DownloadDataTaskAsync(new Uri(fileUrl));

            using (var bitmapImage = Image.FromStream(new MemoryStream(fileData)))
            {
                var ratio = Math.Max(bitmapImage.Width, bitmapImage.Height) / 200;
                var newWidth = bitmapImage.Width / ratio;
                var newHeight = bitmapImage.Height / ratio;
                using (var thumb = new Bitmap(newWidth, newHeight))
                {
                    Point[] points = new Point[]
                    {
                        new Point(0,0),
                        new Point(newWidth,0),
                        new Point(0,newHeight),
                    };

                    using (var graphics = Graphics.FromImage(thumb))
                    {
                        graphics.DrawImage(bitmapImage, points);
                    }

                    var newFileUrl = "";
                    var newFileName = "";
                    switch (fileType)
                    {
                        case FileType.ProfilePicture:
                            newFileName = "ProfilePicThumbnail_" + Guid.NewGuid().ToString() + ".thumbnail";
                            newFileUrl = "ftp://localhost:21/" + "ProfilePics/Thumb/" + newFileName;
                            break;
                        case FileType.Attachment:
                            newFileName = "AttachmentThumbnail_" + Guid.NewGuid().ToString() + ".thumbnail";
                            newFileUrl = "ftp://localhost:21/" + "Attachments/Thumb/" + newFileName;
                            break;
                        default:
                            Debugger.Break();
                            break;
                    }
                    // Create FTP request   
                    var reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(newFileUrl));

                    // Method
                    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    reqFTP.Credentials = new NetworkCredential("kuvondik", "12345");

                    int length = 2048;
                    Stream file = thumb.ToStream(ImageFormat.Jpeg);
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
                    }
                    catch
                    {
                        return null;
                    }
                    return newFileName;
                }
            }
            #endregion
        }
    }
    public static class ImageHelpers
    {
        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
    }
}
