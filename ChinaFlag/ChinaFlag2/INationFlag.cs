using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChinaFlag2
{

    public interface INationFlag
    {
        float WidthProportion { get; }
        float HeightProportion { get; }
        SizeF Size { get; set; }

        void AdjustFlagSizeF(int width);
        Image GetFlagImage();
    }
}
