using ClayBot.Images.Patcher.Large;
using ClayBot.Images.Patcher.Small;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace ClayBot.Images.Patcher
{
    static class PatcherImagesAccessor
    {
        public static Bitmap GetImage(string locale, PatcherSize patcherSize, PatcherRectangle patcherRectangle)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);

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
