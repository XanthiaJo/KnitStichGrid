namespace KnitStichGrid.Models;

public sealed class ImageOverlaySettings
{
    public string? ImagePath { get; set; }
    public double Opacity { get; set; } = 0.5;
    public bool IsVisible { get; set; }
}
