using System.Drawing;

namespace ClayBot.Images.Patcher.Small
{
    static class SmallPatcherImagesAccessor
    {
        public static Bitmap GetImage(PatcherRectangle patcherRectangle)
        {
            switch (patcherRectangle)
            {
                case PatcherRectangle.Accept:
                    return PatcherImages.Accept;
                case PatcherRectangle.Launch:
                    return PatcherImages.Launch;
                case PatcherRectangle.Online:
                    return PatcherImages.Online;
                case PatcherRectangle.PatcherIndicator:
                    return PatcherImages.PatcherIndicator;
            }

            return null;
        }
    }
}
