using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TableClothGUI
{
    // Структура для представления отображаемого символа
    // Параметры: сам символ, его ширина, цвет.
    public class Letter
    {
        public string Symbol;
        public Brush Brush;

        public Letter( char C, Color Col )
        {
            Symbol = C.ToString();
            Brush = new SolidBrush( Col );
        }
    }
}
