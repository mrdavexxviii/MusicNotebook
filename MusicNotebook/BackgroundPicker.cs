using MusicNotebook.NotebookDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicNotebook
{
    

    public class BackgroundPicker
    {
        const double SparseSpace = 60;
        const double DenseSpace = 30;
        const double BaseSize = 4;
        const double LineSize = 32;


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
    }
}
