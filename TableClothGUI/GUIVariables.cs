using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TableClothGUI
{
    static class GUIVariables
    {
        static public Color TextStdColor = Color.Black;
        static public Color TextHighlightColor = Color.DeepSkyBlue;
        static public Color BackgroundColor = Color.White;
        static public Color LineColor = Color.DeepSkyBlue;

        static public SolidBrush HighlightBrush = new SolidBrush( TextHighlightColor );
        static public SolidBrush FunctionalLineBrush = new SolidBrush( LineColor );
        static public SolidBrush BackgroundBrush = new SolidBrush( BackgroundColor );
        static public SolidBrush TextStdBrush = new SolidBrush( TextStdColor );

        static public Pen FunctionalLinePen = new Pen( FunctionalLineBrush, 1.0f );

        static public float StdFontSize = 12.0f;

        static public Font GetStdFontWithSize( float FSize )
        {
            return new Font( "Courier New", FSize, FontStyle.Regular );
        }

        static public double FirstLineCoefficient = 1.3;

        static public int FunctionalLineWidth = 25;
    }
}
