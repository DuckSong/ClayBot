using ClayBot.Images.Patcher.Large;
using ClayBot.Images.Patcher.Small;
using System.Drawing;

namespace ClayBot.Images.Patcher
{
    static class PatcherImagesAccessor
    {
        public static Bitmap GetImage(PatcherSize patcherSize, PatcherRectangle patcherRectangle)
        {
            switch (patcherSize)
            {
                case PatcherSize.Small:
                    return SmallPatcherImagesAccessor.GetImage(patcherRectangle);
                case PatcherSize.Large:
                    return LargePatcherImagesAccessor.GetImage(patcherRectangle);
            }

            return null;
        }
    }
}
