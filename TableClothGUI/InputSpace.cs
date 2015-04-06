using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TableClothGUI
{
    partial class InputSpace : UserControl
    {
        TableLayoutPanel GridLayoutPanel;
        CommandBox InCommandBox;
        CommandBox OutCommandBox;
        Label Inlabel;
        Label Outlabel;
        Panel FunctionalLine;
        Note ParentNote;
        Timer ParentTimer;

        bool ActiveSpace = false;
        bool HideSpace;

        bool MouseOnCloseButton = false;
        bool MouseOnRoundButton = false;

        public InputSpace( Note Parent, Timer T, int CommandBoxWidth = 150, bool isHide = true )
        {
            InitializeComponent();

            SuspendLayout();

            HideSpace = isHide;

            ParentNote = Parent;
            ParentTimer = T;

            Inlabel = new Label();
            Inlabel.Font = GUIVariables.GetStdFontWithSize( GUIVariables.StdFontSize );

            Outlabel = new Label();
            Outlabel.Font = GUIVariables.GetStdFontWithSize( GUIVariables.StdFontSize );

            InCommandBox = new TableClothGUI.CommandBox();
            InCommandBox.ParentInputSpace = this;
            InCommandBox.BackgroundImageLayout = ImageLayout.None;
            InCommandBox.BorderStyle = BorderStyle.None;
            InCommandBox.Margin = new Padding( 0 );
            InCommandBox.TabIndex = 0;
            InCommandBox.Initialize( T, ReSizeLine, CommandBoxWidth - Inlabel.Width - GUIVariables.FunctionalLineWidth );

            OutCommandBox = new TableClothGUI.CommandBox();
            OutCommandBox.ParentInputSpace = this;
            OutCommandBox.BackgroundImageLayout = ImageLayout.None;
            OutCommandBox.BorderStyle = BorderStyle.None;
            OutCommandBox.Margin = new Padding( 0 );
            OutCommandBox.TabIndex = 1;
            OutCommandBox.ReadOnly = true;
            OutCommandBox.Initialize( T, ReSizeLine, CommandBoxWidth - Inlabel.Width - GUIVariables.FunctionalLineWidth );

            FunctionalLine = new Panel();
            FunctionalLine.BackColor = GUIVariables.BackgroundColor;
            FunctionalLine.Size = new Size( GUIVariables.FunctionalLineWidth, Height );
            FunctionalLine.Margin = new Padding( 0 );

            GridLayoutPanel = new TableLayoutPanel();
            GridLayoutPanel.AutoSize = true;
            GridLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GridLayoutPanel.BackColor = GUIVariables.BackgroundColor;
            GridLayoutPanel.ColumnCount = 3;
            GridLayoutPanel.ColumnStyles.Add( new ColumnStyle( SizeType.AutoSize ) );
            GridLayoutPanel.ColumnStyles.Add( new ColumnStyle( SizeType.AutoSize ) );
            GridLayoutPanel.RowCount = 2;
            GridLayoutPanel.Margin = new Padding( 0 );
            GridLayoutPanel.Controls.Add( InCommandBox, 1, 0 );
            GridLayoutPanel.Controls.Add( OutCommandBox, 1, 1 );
            GridLayoutPanel.Controls.Add( FunctionalLine, 2, 0 );
            GridLayoutPanel.Controls.Add( Inlabel, 0, 0 );
            GridLayoutPanel.Controls.Add( Outlabel, 0, 1 );
            GridLayoutPanel.SetRowSpan( FunctionalLine, 2 );

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = GUIVariables.BackgroundColor;
            Name = "InputSpace";
            DoubleBuffered = true;
            Controls.Add( GridLayoutPanel );

            GridLayoutPanel.Dock = DockStyle.Fill;
            Dock = DockStyle.Top;

            if ( HideSpace ) HideOut();

            Pnts[ 0 ] = new Point( 5, 5 );
            Pnts[ 1 ] = new Point( 10, 5 );

            Pnts[ 4 ] = new Point( 15, 5 );
            Pnts[ 5 ] = new Point( 20, 5 );
            Pnts[ 6 ] = new Point( 15, 10 );
            Pnts[ 7 ] = new Point( 20, 10 );

            Pnts[ 8 ] = new Point( 15, 13 );

            InitializeLineGraphics();

            Enter += new System.EventHandler( this.InputSpace_Enter );
            Leave += new System.EventHandler( this.InputSpace_Leave );

            FunctionalLine.MouseMove += new MouseEventHandler( FunctionalLine_MouseMove );
            FunctionalLine.MouseClick += new MouseEventHandler( FunctionalLine_MouseClick );

            ResumeLayout( false );
            PerformLayout();
        }

        private new int Height
        {
            get { return InCommandBox.Height + ( ( !HideSpace ) ? OutCommandBox.Height : 0 ); }
        }

        public void ReSize( int D )
        {
            InCommandBox.ReSize( D );
            OutCommandBox.ReSize( D );
        }

        public float FontSize
        {
            get
            {
                return InCommandBox.FontSize;
            }
            set
            {
                SuspendLayout();

                Inlabel.Font = GUIVariables.GetStdFontWithSize( value );
                Outlabel.Font = GUIVariables.GetStdFontWithSize( value );

                int oldinwidth = Inlabel.Width/*, oldoutwidth = Outlabel.Width*/;

                Inlabel.Size = TextRenderer.MeasureText( Inlabel.Text, Inlabel.Font );
                Outlabel.Size = TextRenderer.MeasureText( Outlabel.Text, Outlabel.Font );

                ReSize( oldinwidth - Inlabel.Width );

                InCommandBox.FontSize = value;
                OutCommandBox.FontSize = value;

                ResumeLayout();
            }
        }

        public string InText
        {
            set
            {
                InCommandBox.Text = value;
            }
            get
            {
                return InCommandBox.Text;
            }
        }

        public string OutText
        {
            set
            {
                OutCommandBox.Text = value;
            }
            get
            {
                return OutCommandBox.Text;
            }
        }

        uint sNumber = 0;
        public uint SpaceNumber
        {
            set
            {
                sNumber = value % 999;
                Inlabel.Text = String.Format( "In  [{0}]", sNumber );
                Outlabel.Text = String.Format( "Out [{0}]", sNumber );
            }
            get
            {
                return sNumber;
            }
        }

        public void ExecuteCommand( string Text )
        {
            if ( ParentNote != null && ParentNote.NoteKernel != null )
            {
                //ParentNote.CurrentFuncInputSpace = ShowResultCommand;
                ParentNote.NoteKernel.ExecuteCommand( Text );
                if ( ParentNote.NoteKernel.Result != String.Empty )
                {
                    OutText = ParentNote.NoteKernel.Result;
                    ShowOut();
                    //InitializeLineGraphics();
                }
            }
        }

        public void ShowOut()
        {
            OutCommandBox.Visible = Outlabel.Visible = true;
        }

        public void HideOut()
        {
            OutCommandBox.Visible = Outlabel.Visible = false;
        }

        public bool IsOutVisible()
        {
            return OutCommandBox.Visible;
        }

        Graphics LineGraphics;

        // точки для рисования 
        Point[] Pnts = new Point[ 9 ];

        void InitializeLineGraphics()
        {
            FunctionalLine.Height = Height;

            Pnts[ 2 ] = new Point( 10, FunctionalLine.Height - 5 );
            Pnts[ 3 ] = new Point( 5, FunctionalLine.Height - 5 );

            FunctionalLine.BackgroundImage = new Bitmap( FunctionalLine.Width, FunctionalLine.Height );
            LineGraphics = Graphics.FromImage( FunctionalLine.BackgroundImage );

            LineGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            ReDrawLine();
        }

        void ReDrawLine()
        {
            SuspendLayout();

            // чистим все ранее нарисованное
            LineGraphics.Clear( GUIVariables.BackgroundColor );
            //LineGraphics.FillRectangle( GUIVariables.BackgroundBrush, 0, 0, FunctionalLine.Width, FunctionalLine.Height );

            if ( ActiveSpace )
            {
                LineGraphics.FillRectangle( GUIVariables.FunctionalLineBrush, Pnts[ 0 ].X, Pnts[ 0 ].Y, 5, FunctionalLine.Height - 10 );
            }
            else
            {
                LineGraphics.DrawLine( GUIVariables.FunctionalLinePen, Pnts[ 0 ], Pnts[ 1 ] );
                LineGraphics.DrawLine( GUIVariables.FunctionalLinePen, Pnts[ 1 ], Pnts[ 2 ] );
                LineGraphics.DrawLine( GUIVariables.FunctionalLinePen, Pnts[ 2 ], Pnts[ 3 ] );
            }

            if ( MouseOnCloseButton )
            {
                LineGraphics.FillRectangle( Brushes.Red, Pnts[ 4 ].X - 1, Pnts[ 4 ].Y - 1, 7, 7 );
                LineGraphics.DrawLine( Pens.White, Pnts[ 4 ], Pnts[ 7 ] );
                LineGraphics.DrawLine( Pens.White, Pnts[ 5 ], Pnts[ 6 ] );
            }
            else
            {
                LineGraphics.DrawLine( GUIVariables.FunctionalLinePen, Pnts[ 4 ], Pnts[ 7 ] );
                LineGraphics.DrawLine( GUIVariables.FunctionalLinePen, Pnts[ 5 ], Pnts[ 6 ] );
            }

            if ( MouseOnRoundButton )
            {
                LineGraphics.FillRectangle( Brushes.Red, Pnts[ 8 ].X - 1, Pnts[ 8 ].Y - 1, 7, 7 );
                LineGraphics.DrawEllipse( Pens.White, Pnts[ 8 ].X, Pnts[ 8 ].Y, 5, 5 );
            }
            else
            {
                LineGraphics.DrawEllipse( GUIVariables.FunctionalLinePen, Pnts[ 8 ].X, Pnts[ 8 ].Y, 5, 5 );
            }

            MouseOnCloseButton = MouseOnRoundButton = false;

            //Invalidate();
            Refresh();

            ResumeLayout();
        }

        void ReSizeLine()
        {
            InitializeLineGraphics();
        }

        private void InputSpace_Enter( object sender, EventArgs e )
        {
            ActiveSpace = true;
            ReDrawLine();
        }

        private void InputSpace_Leave( object sender, EventArgs e )
        {
            ActiveSpace = false;
            ReDrawLine();
        }

        // чтобы лишний раз не перерисовывать
        bool justReDrawLine = false;
        void FunctionalLine_MouseMove( object sender, MouseEventArgs e )
        {
            if ( MouseOnCloseButton || MouseOnRoundButton ) return;
            if ( e.X >= Pnts[ 4 ].X && e.X <= Pnts[ 7 ].X && e.Y >= Pnts[ 4 ].Y && e.Y <= Pnts[ 7 ].Y )
            {
                MouseOnCloseButton = true;
                justReDrawLine = true;
                ReDrawLine();
                return;
            }
            if ( e.X >= Pnts[ 8 ].X && e.X <= ( Pnts[ 8 ].X + 5 ) && e.Y >= Pnts[ 8 ].Y && e.Y <= ( Pnts[ 8 ].Y + 5 ) )
            {
                MouseOnRoundButton = true;
                justReDrawLine = true;
                ReDrawLine();
                return;
            }
            if ( justReDrawLine )
            {
                justReDrawLine = false;
                ReDrawLine();
                return;
            }
        }

        void FunctionalLine_MouseClick( object sender, MouseEventArgs e )
        {
            if ( e.X >= Pnts[ 4 ].X && e.X <= Pnts[ 7 ].X && e.Y >= Pnts[ 4 ].Y && e.Y <= Pnts[ 7 ].Y )
            {
                ParentNote.RemoveInputSpace( sNumber );
            }
            if ( e.X >= Pnts[ 8 ].X && e.X <= ( Pnts[ 8 ].X + 5 ) && e.Y >= Pnts[ 8 ].Y && e.Y <= ( Pnts[ 8 ].Y + 5 ) )
            {
                if ( HideSpace )
                    ShowOut();
                else
                    HideOut();
                HideSpace = !HideSpace;
                ReSizeLine();
            }
        }
    }
}
