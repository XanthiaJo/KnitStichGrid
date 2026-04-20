using KnitStichGrid.Models;
using KnitStichGrid.Services.Interfaces;

namespace KnitStichGrid.Services;

public sealed class FinishedSizeCalculator : IFinishedSizeCalculator
{
    public FinishedSize Calculate(GaugeSettings gauge, PatternDimensions dimensions)
    {
        var width = gauge.StitchesPerInch <= 0 ? 0 : dimensions.StitchCount / gauge.StitchesPerInch;
        var height = gauge.RowsPerInch <= 0 ? 0 : dimensions.RowCount / gauge.RowsPerInch;
        return new FinishedSize(width, height);
    }
}
