using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TableClothGUI
{
    internal partial class CommandBox : UserControl
    {
        // Текущий шрифт
        Font CommandBoxFont = GUIVariables.GetStdFontWithSize(GUIVariables.StdFontSize);

        // Графика
        Graphics myGraphics;

        // Текст в боксе как список структур Letter
        List<Letter> myText = new List<Letter>(50);
        string mySText = "";

        // Размеры текущего шрифта
        Size CommandBoxFontSize;

        // Число символов в строке и столбце, также общее допустимое число символов
        int SymbolsInLine, SymbolsInColumn, SymbolsMaximum;

        // Выделение текста
        bool isSelection = false;
        int StartSelection, EndSelection;

        // таймер и делегат на событие таймера
        Timer ParentTimer;
        EventHandler TimerEvent;

        // отображается ли курсор и его позиция
        bool isCursor = false;
        int CursorPos;

        // вызывается при изменении размеров
        public delegate void ExternalReSize();
        ExternalReSize ExReSize;

        // родительское пронстранство ввода
        public InputSpace ParentInputSpace;

        public bool ReadOnly { set; get; }
    }
}