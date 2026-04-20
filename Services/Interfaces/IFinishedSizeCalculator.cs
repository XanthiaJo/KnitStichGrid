using KnitStichGrid.Models;

namespace KnitStichGrid.Services.Interfaces;

public interface IFinishedSizeCalculator
{
    FinishedSize Calculate(GaugeSettings gauge, PatternDimensions dimensions);
}
