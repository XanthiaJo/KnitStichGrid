namespace KnitStichGrid.ViewModels.GridCanvas
{
    public interface ILayerViewModel
    {
        string Name { get; }
        bool IsVisible { get; set; }
        double Opacity { get; set; }
        int ZIndex { get; set; }
    }
}
