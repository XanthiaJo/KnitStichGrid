namespace KnitStichGrid.ViewModels;

public sealed class GridCellViewModel : ViewModelBase
{
    private bool _isFilled;

    public bool IsFilled
    {
        get => _isFilled;
        set => SetProperty(ref _isFilled, value);
    }
}
