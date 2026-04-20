namespace KnitStichGrid.Models;

public sealed class GaugeSettings
{
    public double StitchesPer4Inches { get; set; } = 20;
    public double RowsPer4Inches { get; set; } = 28;

    public double StitchesPerInch => StitchesPer4Inches / 4.0;
    public double RowsPerInch => RowsPer4Inches / 4.0;
}
