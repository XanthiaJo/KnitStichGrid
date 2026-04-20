namespace KnitStichGrid.ViewModels.GridCanvas
{
    // Represents the actual grid layer (cells rendering/hit-testing) separate from the overall gridcanvas
    public class GridLayerViewModel : LayerBaseViewModel
    {
        public GridLayerViewModel() : base("GridLayer") { }

        // Example properties for future expansion
        public int Columns { get; set; }
        public int Rows { get; set; }

        // Cell data and selection would be added here
    }
}
