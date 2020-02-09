namespace CloudChat.Core
{
    public static class IconTypeToFontAwesome
    {
        public static string ToFontAwesome(this IconType type)
        {
            switch (type)
            {
                case IconType.FileManager:
                    return "\uf0f6";
                case IconType.Picture:
                    return "\uf1c5";
                default:
                    return null;
            }
        }

        public static string ToFontAwesome(this ContentIcon type)
        {
            switch (type)
            {
                case ContentIcon.Loading:
                    return "\uf1ce";
                case ContentIcon.Chat:
                    return "\uf086";
                case ContentIcon.Picture:
                    return "\uf03e";
                case ContentIcon.Settings:
                    return "\uf085";
                case ContentIcon.PaperClip:
                    return "\uf0f6";
                case ContentIcon.Emoji:
                    return "\uf1c5";
                case ContentIcon.Phone:
                    return "\uf095";
                case ContentIcon.Video:
                    return "\uf1c5";
                case ContentIcon.Person:
                    return "\uf0f6";
                case ContentIcon.Voice:
                    return "\uf1c5";
                case ContentIcon.FileManager:
                    return "\uf0f6";
                case ContentIcon.Search:
                    return "\uf1c5";
                case ContentIcon.HorizontalDots:
                    return "\uf0f6";
                case ContentIcon.VerticalDots:
                    return "\uf1c5";
                case ContentIcon.PaperPlane:
                    return "\uf1d8";
                case ContentIcon.True:
                    return "\uf05d";
                case ContentIcon.False:
                    return "\uf05c";



                default:
                    return null;
            }
        }
    }
}
