using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KnitStichGrid.Commands;
using Microsoft.Win32;

namespace KnitStichGrid.ViewModels.GridCanvas;

public sealed class GridCanvasViewModel : ViewModelBase
{
    private double _cellWidthPx = 20;
    private double _cellHeightPx = 28;
    private int _gridColumns = 24;
    private int _gridRows = 24;
    private string? _overlayImagePath;
    private ImageSource? _overlayImageSource;
    private bool _isOverlayVisible;
    private double _overlayOpacity = 0.5;
    private readonly RelayCommand _clearOverlayImageCommand;

    public GridCanvasViewModel()
    {
        PreviewCells = new ObservableCollection<GridCellViewModel>();
        BrowseOverlayImageCommand = new RelayCommand(BrowseOverlayImage);
        _clearOverlayImageCommand = new RelayCommand(ClearOverlayImage, () => !string.IsNullOrWhiteSpace(OverlayImagePath));
        ClearOverlayImageCommand = _clearOverlayImageCommand;
        RebuildPreviewCells();
    }

    public int GridColumns
    {
        get => _gridColumns;
        set
        {
            if (SetProperty(ref _gridColumns, Math.Max(1, value)))
            {
                OnPreviewDimensionsChanged();
                RebuildPreviewCells();
            }
        }
    }

    public int GridRows
    {
        get => _gridRows;
        set
        {
            if (SetProperty(ref _gridRows, Math.Max(1, value)))
            {
                OnPreviewDimensionsChanged();
                RebuildPreviewCells();
            }
        }
    }

    public string? OverlayImagePath
    {
        get => _overlayImagePath;
        set
        {
            if (SetProperty(ref _overlayImagePath, value))
            {
                LoadOverlayImage();
                _clearOverlayImageCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public ImageSource? OverlayImageSource
    {
        get => _overlayImageSource;
        private set => SetProperty(ref _overlayImageSource, value);
    }

    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        set => SetProperty(ref _isOverlayVisible, value);
    }

    public double OverlayOpacity
    {
        get => _overlayOpacity;
        set => SetProperty(ref _overlayOpacity, value);
    }

    public ObservableCollection<GridCellViewModel> PreviewCells { get; }
    public ICommand BrowseOverlayImageCommand { get; }
    public ICommand ClearOverlayImageCommand { get; }

    public int TotalCellCount => GridColumns * GridRows;
    public double CellWidthPx => _cellWidthPx;
    public double CellHeightPx => _cellHeightPx;
    public double PreviewGridWidthPx => GridColumns * CellWidthPx;
    public double PreviewGridHeightPx => GridRows * CellHeightPx;

    public void UpdateCellSizing(double stitchesPer4Inches, double rowsPer4Inches)
    {
        var widthChanged = SetProperty(ref _cellWidthPx, Math.Max(1, stitchesPer4Inches), nameof(CellWidthPx));
        var heightChanged = SetProperty(ref _cellHeightPx, Math.Max(1, rowsPer4Inches), nameof(CellHeightPx));

        if (widthChanged || heightChanged)
        {
            OnPropertyChanged(nameof(PreviewGridWidthPx));
            OnPropertyChanged(nameof(PreviewGridHeightPx));
        }
    }

    private void OnPreviewDimensionsChanged()
    {
        OnPropertyChanged(nameof(TotalCellCount));
        OnPropertyChanged(nameof(PreviewGridWidthPx));
        OnPropertyChanged(nameof(PreviewGridHeightPx));
    }

    private void RebuildPreviewCells()
    {
        PreviewCells.Clear();

        for (var index = 0; index < TotalCellCount; index++)
        {
            PreviewCells.Add(new GridCellViewModel());
        }
    }

    private void BrowseOverlayImage()
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Select Overlay Image",
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff;*.webp|All Files|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        if (fileDialog.ShowDialog() == true)
        {
            OverlayImagePath = fileDialog.FileName;
            IsOverlayVisible = true;
        }
    }

    private void ClearOverlayImage()
    {
        OverlayImagePath = null;
        IsOverlayVisible = false;
    }

    private void LoadOverlayImage()
    {
        if (string.IsNullOrWhiteSpace(OverlayImagePath))
        {
            OverlayImageSource = null;
            return;
        }

        try
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(OverlayImagePath, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            bitmap.Freeze();
            OverlayImageSource = bitmap;
        }
        catch
        {
            OverlayImageSource = null;
        }
    }
}
