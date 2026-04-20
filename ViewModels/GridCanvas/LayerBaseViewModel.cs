using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KnitStichGrid.ViewModels.GridCanvas
{
    public abstract class LayerBaseViewModel : ILayerViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected LayerBaseViewModel(string name)
        {
            Name = name;
            IsVisible = true;
            Opacity = 1.0;
            ZIndex = 0;
        }

        public string Name { get; }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set { isVisible = value; OnPropertyChanged(); }
        }

        private double opacity;
        public double Opacity
        {
            get => opacity;
            set { opacity = value; OnPropertyChanged(); }
        }

        private int zIndex;
        public int ZIndex
        {
            get => zIndex;
            set { zIndex = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
