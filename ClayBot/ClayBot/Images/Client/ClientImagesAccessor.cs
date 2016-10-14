using System.Drawing;

namespace ClayBot.Images.Client
{
    static class ClientImagesAccessor
    {
        public static Bitmap GetImage(ClientRectangle clientRectangle)
        {
            switch (clientRectangle)
            {
                case ClientRectangle.InactiveLogin:
                    return ClientImages.InactiveLogin;
                case ClientRectangle.Login:
                    return ClientImages.Login;
                case ClientRectangle.LoginIndicator:
                    return ClientImages.LoginIndicator;
                case ClientRectangle.LoginIndicator2:
                    return ClientImages.LoginIndicator2;
                case ClientRectangle.LoginIndicatorThreshold:
                    return ClientImages.LoginIndicatorThreshold;
            }

            return null;
        }
    }
}
