using ClayBot.Images.Client;
using ClayBot.Images.Patcher;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using static ClayBot.NativeMethods;

namespace ClayBot.StateMachine
{
    struct PatcherValidation
    {
        public bool ExpectedResult;
        public PatcherRectangle PatcherRectangle;
        public PatcherSize PatcherSize;
        public bool IsThreshold;

        public PatcherValidation(bool expectedResult, PatcherRectangle patcherRectangle, PatcherSize patcherSize, bool isThreshold)
        {
            ExpectedResult = expectedResult;
            PatcherRectangle = patcherRectangle;
            PatcherSize = patcherSize;
            IsThreshold = isThreshold;
        }
    }

    struct ClientValidation
    {
        public bool ExpectedResult;
        public ClientRectangle ClientRectangle;
        public bool IsThreshold;

        public ClientValidation(bool expectedResult, ClientRectangle clientRectangle, bool isThreshold)
        {
            ExpectedResult = expectedResult;
            ClientRectangle = clientRectangle;
            IsThreshold = isThreshold;
        }
    }

    partial class MainWorker
    {
        private Image<Bgr, byte> GetTargetWindowImage(Rectangle location)
        {
            SetCursorPos(targetWindow.Rect.Left, targetWindow.Rect.Top);

            using (Bitmap bitmap = new Bitmap(location.Width, location.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(
                    new Point(
                        targetWindow.Rect.Left + location.Left,
                        targetWindow.Rect.Top + location.Top),
                    Point.Empty,
                    location.Size);
                return new Image<Bgr, byte>(bitmap);
            }
        }
        
        private bool CheckImage(Image<Bgr, byte> sourceImage, Image<Bgr, byte> targetImage)
        {
            using (Image<Gray, float> result = sourceImage.MatchTemplate(targetImage, TemplateMatchingType.SqdiffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;

                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                return minValues[0] < Static.SENSITIVITY;
            }
        }

        private bool CheckThresholdImage(Image<Bgr, byte> sourceImage, Image<Bgr, byte> targetImage)
        {
            using (Image<Gray, byte> thresholdSourceImage = sourceImage.Convert<Gray, byte>().ThresholdBinary(new Gray(Static.THRESHOLD_GRAY), new Gray(255d)))
            using (Image<Gray, byte> thresholdTargetImage = targetImage.Convert<Gray, byte>().ThresholdBinary(new Gray(Static.THRESHOLD_GRAY), new Gray(255d)))
            using (Image<Gray, float> result = thresholdSourceImage.MatchTemplate(thresholdTargetImage, TemplateMatchingType.SqdiffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;

                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                return minValues[0] < Static.SENSITIVITY;
            }
        }

        private bool CheckImageOnTargetWindow(Bitmap image, Rectangle location, bool threshold = false)
        {
            using (Image<Bgr, byte> sourceImage = GetTargetWindowImage(location))
            using (Image<Bgr, byte> targetImage = new Image<Bgr, byte>(image))
            {
                return threshold ? CheckThresholdImage(sourceImage, targetImage) : CheckImage(sourceImage, targetImage);
            }
        }

        private bool ValidatePatcher(params PatcherValidation[] patcherValidations)
        {
            foreach (PatcherValidation patcherValidation in patcherValidations)
            {
                if (CheckImageOnTargetWindow(
                    PatcherImagesAccessor.GetImage(patcherValidation.PatcherSize, patcherValidation.PatcherRectangle),
                    Static.PATCHER_RECTANGLES[patcherValidation.PatcherRectangle][patcherValidation.PatcherSize],
                    patcherValidation.IsThreshold) != patcherValidation.ExpectedResult)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateClient(params ClientValidation[] clientValidations)
        {
            foreach (ClientValidation clientValidation in clientValidations)
            {
                if (CheckImageOnTargetWindow(
                    ClientImagesAccessor.GetImage(clientValidation.ClientRectangle),
                    Static.CLIENT_RECTANGLES[clientValidation.ClientRectangle],
                    clientValidation.IsThreshold) != clientValidation.ExpectedResult)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
