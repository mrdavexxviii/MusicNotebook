using CommunityToolkit.Mvvm.ComponentModel;
using MusicNotebook.NotebookDefinitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MusicNotebook
{
    internal class BackgroundItem
    {
        public string Name { get; private set; }
        public string Resource { get; private set; }
        public double Pitch { get; private set; }
        public BackgroundItem(string name, string resource, double pitch )
        {
            Name = name;
            Resource = resource;
            Pitch = pitch;

        }
    }
   
    internal partial class ImagePagePropertiesViewModel : ObservableObject
    {
        const double SparseSpace = 60;
        const double DenseSpace = 30;
        const double BaseSize = 4;
        const double LineSize = 32;
        [ObservableProperty]
        BackgroundItem _background;
        private readonly ImagePage page;

        public ICollection<BackgroundItem> AvailableBackgrounds { get; } =
        [
            new ( "None", String.Empty,1),
            new ( "Guitar Tab", nameof(Resources.Guitar_Tab), BaseSize+ (6*LineSize)+SparseSpace), 
            new ( "Compact Guitar Tab", nameof(Resources.Guitar_Tab), BaseSize+ (6*LineSize)+DenseSpace),
            new ( "Bass Tab", nameof(Resources.Bass_Tab), BaseSize+ (4*LineSize)+SparseSpace),
            new ( "Compact Bass Tab", nameof(Resources.Bass_Tab), BaseSize+ (4*LineSize)+DenseSpace),
            new ( "5 String Bass Tab", nameof(Resources.FiveStringBass_Tab), BaseSize+ (5*LineSize)+SparseSpace),
            new ( "Compact 5 String Bass Tab", nameof(Resources.FiveStringBass_Tab), BaseSize+ (5*LineSize)+DenseSpace),
        ];


        public ImagePagePropertiesViewModel(ImagePage page)
        {
            this.page = page;
            
            Background = AvailableBackgrounds.FirstOrDefault(x => x.Resource == page.Background && x.Pitch == page.BackgroundPitch) ?? 
                         AvailableBackgrounds.FirstOrDefault(x=> x.Resource == page.Background) ??              
                         AvailableBackgrounds.First();
            
            PropertyChanged += ImagePagePropertiesViewModel_PropertyChanged;
        }

        private void ImagePagePropertiesViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Background))
            {
                page.Background = Background.Resource;
                page.BackgroundPitch = Background.Pitch;
            }
        }
    }
}
