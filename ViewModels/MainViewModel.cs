using System.ComponentModel;
using System.Windows.Input;
using KnitStichGrid.Commands;
using KnitStichGrid.Models;
using KnitStichGrid.Services.Interfaces;

namespace KnitStichGrid.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly IFinishedSizeCalculator _finishedSizeCalculator;

    private double _stitchesPer4Inches = 20;
    private double _rowsPer4Inches = 28;
    private double _finishedWidthInches;
    private double _finishedHeightInches;

    public MainViewModel(IFinishedSizeCalculator finishedSizeCalculator)
    {
        _finishedSizeCalculator = finishedSizeCalculator;
        PatternCanvas = new PatternCanvasViewModel();
        PatternCanvas.PropertyChanged += OnPatternCanvasPropertyChanged;
        RecalculateSizeCommand = new RelayCommand(RecalculateSize);
        RecalculateSize();
    }

    public double StitchesPer4Inches
    {
        get => _stitchesPer4Inches;
        set
        {
            if (SetProperty(ref _stitchesPer4Inches, value))
            {
                RecalculateSize();
            }
        }
    }

    public double RowsPer4Inches
    {
        get => _rowsPer4Inches;
        set
        {
            if (SetProperty(ref _rowsPer4Inches, value))
            {
                RecalculateSize();
            }
        }
    }

    public double FinishedWidthInches
    {
        get => _finishedWidthInches;
        private set => SetProperty(ref _finishedWidthInches, value);
    }

    public double FinishedHeightInches
    {
        get => _finishedHeightInches;
        private set => SetProperty(ref _finishedHeightInches, value);
    }

    public ICommand RecalculateSizeCommand { get; }
    public PatternCanvasViewModel PatternCanvas { get; }

    private void RecalculateSize()
    {
        var gauge = new GaugeSettings
        {
            StitchesPer4Inches = StitchesPer4Inches,
            RowsPer4Inches = RowsPer4Inches
        };

        var dimensions = new PatternDimensions
        {
            StitchCount = PatternCanvas.GridColumns,
            RowCount = PatternCanvas.GridRows
        };

        var finishedSize = _finishedSizeCalculator.Calculate(gauge, dimensions);
        FinishedWidthInches = Math.Round(finishedSize.WidthInches, 2);
        FinishedHeightInches = Math.Round(finishedSize.HeightInches, 2);
        PatternCanvas.UpdateCellSizing(StitchesPer4Inches, RowsPer4Inches);
    }

    private void OnPatternCanvasPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is nameof(PatternCanvasViewModel.GridColumns) or nameof(PatternCanvasViewModel.GridRows))
        {
            RecalculateSize();
        }
    }
}
