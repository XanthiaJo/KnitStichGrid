namespace KnitStichGrid.ViewModels.GridCanvas
{
    // Measurments layer VM (file renamed for consistency)
    public class MeasurmentsLayerViewModel : LayerBaseViewModel
    {
        public MeasurmentsLayerViewModel() : base("MeasurmentsLayer") { }

        // Rectangle stored in grid units (stitches/rows)
        public double RectXSt { get; set; }
        public double RectYRow { get; set; }
        public double RectWSt { get; set; }
        public double RectHRow { get; set; }

        // Derived properties for display
        public double WidthInches(double stitchesPerIn)
        {
            return stitchesPerIn > 0 ? RectWSt / stitchesPerIn : 0.0;
        }

        public double HeightInches(double rowsPerIn)
        {
            return rowsPerIn > 0 ? RectHRow / rowsPerIn : 0.0;
        }

        public double WidthCm(double stitchesPerIn)
        {
            return WidthInches(stitchesPerIn) * 2.54;
        }

        public double HeightCm(double rowsPerIn)
        {
            return HeightInches(rowsPerIn) * 2.54;
        }
    }
}
