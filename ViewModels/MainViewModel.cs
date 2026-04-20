using System.ComponentModel;
using System.Windows.Input;
using KnitStichGrid.Commands;
using KnitStichGrid.Models;
using KnitStichGrid.Services.Interfaces;
using KnitStichGrid.ViewModels.GridCanvas;

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
        GridCanvas = new GridCanvasViewModel();
        GridCanvas.PropertyChanged += OnGridCanvasPropertyChanged;
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
    public GridCanvasViewModel GridCanvas { get; }

    private void RecalculateSize()
    {
        var gauge = new GaugeSettings
        {
            StitchesPer4Inches = StitchesPer4Inches,
            RowsPer4Inches = RowsPer4Inches
        };

        var dimensions = new PatternDimensions
        {
            StitchCount = GridCanvas.GridColumns,
            RowCount = GridCanvas.GridRows
        };

        var finishedSize = _finishedSizeCalculator.Calculate(gauge, dimensions);
        FinishedWidthInches = Math.Round(finishedSize.WidthInches, 2);
        FinishedHeightInches = Math.Round(finishedSize.HeightInches, 2);
        GridCanvas.UpdateCellSizing(StitchesPer4Inches, RowsPer4Inches);
    }

    private void OnGridCanvasPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName is nameof(GridCanvasViewModel.GridColumns) or nameof(GridCanvasViewModel.GridRows))
        {
            RecalculateSize();
        }
    }
}
