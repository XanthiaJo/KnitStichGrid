using System;

namespace KnitStichGrid.ViewModels.GridCanvas
{
    // Overlay layer VM (file renamed for consistency)
    public class OverlayLayerViewModel : LayerBaseViewModel
    {
        public OverlayLayerViewModel() : base("OverlayLayer") { }

        // Image resource / visibility
        public string? ImagePath { get; set; }

        // Calibration points stored in image pixel space
        public double CalibP1ImgX { get; set; }
        public double CalibP1ImgY { get; set; }
        public double CalibP2ImgX { get; set; }
        public double CalibP2ImgY { get; set; }

        // Calibration input (user-supplied length) and unit
        public double CalibLengthInput { get; set; }
        public string CalibUnit { get; set; } = "in"; // "in" or "cm"

        // Convenience: computed length in image pixels
        public double CalibLengthImagePixels =>
            Math.Sqrt(Math.Pow(CalibP2ImgX - CalibP1ImgX, 2) + Math.Pow(CalibP2ImgY - CalibP1ImgY, 2));

        // Apply calibration using grid canvas global state (PixelsPerInch)
        public double ComputeImageScale(double pixelsPerInch)
        {
            if (CalibLengthImagePixels <= 0) return 1.0;
            var lengthInInches = CalibUnit == "cm" ? CalibLengthInput / 2.54 : CalibLengthInput;
            var targetScreenPx = lengthInInches * pixelsPerInch;
            return targetScreenPx / CalibLengthImagePixels;
        }

        // Pan/zoom are applied at GridCanvasViewModel level (ImageScale, ImageOffsetX/Y)
    }
}
