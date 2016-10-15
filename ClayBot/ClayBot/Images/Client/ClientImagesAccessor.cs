using System.Drawing;

namespace ClayBot.Images.Client
{
    static class ClientImagesAccessor
    {
        public static Bitmap GetImage(ClientRectangle clientRectangle)
        {
            switch (clientRectangle)
            {
                case ClientRectangle.Accept:
                    return ClientImages.Accept;
                case ClientRectangle.AramIndicator:
                    return ClientImages.AramIndicator;
                case ClientRectangle.Home:
                    return ClientImages.Home;
                case ClientRectangle.InactivePlay:
                    return ClientImages.InactivePlay;
                case ClientRectangle.InQueueIndicator:
                    return ClientImages.InQueueIndicator;
                case ClientRectangle.InvalidLoginOk:
                    return ClientImages.InvalidLoginOk;
                case ClientRectangle.JoinQueueFailedIndicator:
                    return ClientImages.JoinQueueFailedIndicator;
                case ClientRectangle.LeaverBusterIndicator:
                    return ClientImages.LeaverBusterIndicator;
                case ClientRectangle.LeaverBusterWarning:
                    return ClientImages.LeaverBusterWarning;
                case ClientRectangle.LoggingInIndicatorThreshold:
                    return ClientImages.LoggingInIndicatorThreshold;
                case ClientRectangle.Login:
                    return ClientImages.Login;
                case ClientRectangle.LoginIndicator:
                    return ClientImages.LoginIndicator;
                case ClientRectangle.LoginIndicator2:
                    return ClientImages.LoginIndicator2;
                case ClientRectangle.LoginIndicatorThreshold:
                    return ClientImages.LoginIndicatorThreshold;
                case ClientRectangle.Play:
                    return ClientImages.Play;
                case ClientRectangle.Reconnect:
                    return ClientImages.Reconnect;
                case ClientRectangle.Solo:
                    return ClientImages.Solo;
                case ClientRectangle.TeamChat:
                    return ClientImages.TeamChat;
            }

            return null;
        }
    }
}
