using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TableClothGUI
{
    public class TCSyntax
    {
        const int EnglishABCLength = 'z' - 'a' + 1;

        bool[] FirstSymbol;
        List<string>[] SyntaxTree;

        SortedDictionary<string, Color> keywords = null;
        public SortedDictionary<string, Color> KeyWords
        {
            set
            {
                keywords = value;
            }
            get
            {
                return keywords;
            }
        }

        public TCSyntax()
        {
        }

        public Brush RoundBracketColorBrush = Brushes.DarkRed;
        Color roundBracketColor = Color.DarkRed;
        public Color RoundBracketColor
        { 
            get
            {
                return roundBracketColor;
            }
            set
            {
                roundBracketColor = value;
                RoundBracketColorBrush = new SolidBrush( roundBracketColor );
            }
        }

        public Brush SquareBracketColorBrush = Brushes.DarkRed;
        Color squareBracketColor = Color.DarkRed;
        public Color SquareBracketColor
        {
            get
            {
                return squareBracketColor;
            }
            set
            {
                squareBracketColor = value;
                SquareBracketColorBrush = new SolidBrush( squareBracketColor );
            }
        }

        public Brush TriangleBracketColorBrush = Brushes.DarkRed;
        Color triangleBracketColor = Color.DarkRed;
        public Color TriangleBracketColor
        {
            get
            {
                return triangleBracketColor;
            }
            set
            {
                triangleBracketColor = value;
                TriangleBracketColorBrush = new SolidBrush( triangleBracketColor );
            }
        }

        public Brush GroupBracketColorBrush = Brushes.DarkRed;
        Color groupBracketColor = Color.DarkRed;
        public Color GroupBracketColor
        {
            get
            {
                return groupBracketColor;
            }
            set
            {
                groupBracketColor = value;
                GroupBracketColorBrush = new SolidBrush( groupBracketColor );
            }
        }

        public Brush NumberColorBrush = Brushes.Blue;
        Color numberColor = Color.Blue;
        public Color NumberColor
        {
            get
            {
                return numberColor;
            }
            set
            {
                numberColor = value;
                NumberColorBrush = new SolidBrush( numberColor );
            }
        }

        public Brush SymbolColorBrush = Brushes.Gray;
        Color symbolColor = Color.Gray;
        public Color SymbolColor
        {
            get
            {
                return symbolColor;
            }
            set
            {
                symbolColor = value;
                SymbolColorBrush = new SolidBrush( symbolColor );
            }
        }

        public Brush StandartColorBrush = Brushes.Black;
        Color standartColor = Color.Black;
        public Color StandartColor
        {
            get
            {
                return standartColor;
            }
            set
            {
                standartColor = value;
                StandartColorBrush = new SolidBrush( standartColor );
            }
        }

        public void CreateSyntax()
        {
            FirstSymbol = new bool[ EnglishABCLength ];
            SyntaxTree = new List<string>[ EnglishABCLength * EnglishABCLength ];

            foreach ( string kw in keywords.Keys )
            {
                // проверяем длину
                if ( kw.Length < 2 ) throw new Exception( "Keyword length must be 2 or more symbols" );
                
                // проверяем правильность ключевого слова
                bool iscorrect = true;
                if ( Char.IsLetter( kw[ 0 ] ) )
                {
                    for ( int j = 1; j < kw.Length; j++ )
                        if ( !Char.IsLetterOrDigit( kw[ j ] ) )
                            iscorrect = false;
                }
                else iscorrect = false;

                if ( !iscorrect )
                    throw new Exception( "Error in keyword {" + kw + "}" );

                FirstSymbol[ GetCharIndex( kw[ 0 ] ) ] = true;
                
                int index = GetCharIndex( kw[ 0 ] ) * 26 + GetCharIndex( kw[ 1 ] );
                
                if ( SyntaxTree[ index ] == null )
                {
                    SyntaxTree[ index ] = new List<string>();
                }

                SyntaxTree[ index ].Add( kw );
            }
        }

        public bool CheckTokenOptimized( string Token, ref Brush TokenBrush )
        {
            if ( Token.Length < 2 )
                return false;

            int first = GetCharIndex( Token[ 0 ] );
            
            if ( FirstSymbol[ first ] )
            {
                int index = GetCharIndex( Token[ 0 ] ) * 26 + GetCharIndex( Token[ 1 ] );

                if ( SyntaxTree[ index ] != null )
                {
                    foreach ( string s in SyntaxTree[ index ] )
                    {
                        if ( s == Token )
                        {
                            // TODO оптимизнуть каждый раз тут создается SolidBrush
                            TokenBrush = new SolidBrush( KeyWords[ s ] );
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CheckTokenFirstSymbolOptimized( char c )
        {
            return FirstSymbol[ GetCharIndex( c ) ];
        }

        int GetCharIndex( char c )
        {
            if ( c >= 'a' && c <= 'z' )
                return c - 'a';
            else if ( c >= 'A' && c <= 'Z' )
                return c - 'A';
            return Char.ToLower( c ) - 'a';
        }

        // TODO
        public void LoadSyntax() { }
        public void SaveSyntax() { }
    }

    public static class TCSyntaxHighlighter
    {
        static TCSyntax Snt;

        static List<Letter> Text;
        static string sText;
        static int start;
        static int end;

        public static void SetSyntax( TCSyntax S )
        { 
            Snt = S;
        }

        public static void LightAllText( List<Letter> Text, string sText )
        {
            LightPartText( Text, sText, 0, Text.Count - 1 );
        }

        public static void LightPartText( List<Letter> aText, string asText, int astart, int aend )
        {
            if ( aText.Count == 0 )
                return;

            if ( !( astart >= 0 && aend >= astart && ( aText.Count - 1 ) >= aend ) )
                return;

            Text = aText;
            sText = asText;
            start = astart;
            end = aend;

            int tokenLength;
            char c;
            bool iskeyw = false;
            Brush tmpbrush = null;

            for ( int i = start; i <= end; i++ )
            {
                c = Char.ToLower( Text[ i ].Symbol[ 0 ] );

                if ( Convert.ToInt32( c ) < 33 ) 
                    continue;

                if ( Char.IsDigit( c ) )
                {
                    Text[ i ].Brush = Snt.NumberColorBrush;
                }
                else if ( c == '(' || c == ')' )
                {
                    Text[ i ].Brush = Snt.RoundBracketColorBrush;
                }
                else if ( c == '[' || c == ']' )
                {
                    Text[ i ].Brush = Snt.SquareBracketColorBrush;
                }
                else if ( c == '<' || c == '>' )
                {
                    Text[ i ].Brush = Snt.TriangleBracketColorBrush;
                }
                else if ( c == '{' || c == '}' )
                {
                    Text[ i ].Brush = Snt.GroupBracketColorBrush;
                }
                else if ( Char.IsLetter( c ) )
                {
                    // проверяем есть ли вообще ключевые слова на первую букву токена
                    iskeyw = Snt.CheckTokenFirstSymbolOptimized( c );

                    if ( !iskeyw )
                        continue;

                    // получаем токен
                    string token = GetToken( i, out tokenLength );
                    i += tokenLength - 1;

                    if ( !( iskeyw && Snt.CheckTokenOptimized( token, ref tmpbrush ) ) )
                    {
                        tmpbrush = Snt.SymbolColorBrush;
                    }  

                    // красим токен
                    PaintToken( i, tokenLength - i, tmpbrush );
                }
                else
                {
                    // красим стандартным цветом
                    Text[ i ].Brush = Snt.SymbolColorBrush;
                }
            }
        }

        static string GetToken( int tokenStart, out int tokenLength )
        {
            int tokenEnd = tokenStart + 1;
            char c;

            while ( tokenEnd <= end )
            {
                c = Text[ tokenEnd ].Symbol[ 0 ];
                if ( Char.IsLetterOrDigit( c ) )
                    tokenEnd++;
                else
                    break;
            }
            tokenLength = tokenEnd - tokenStart;

            return sText.Substring( tokenStart, tokenLength );
        }

        static void PaintToken( int tokenStart, int tokenEnd, Brush brush )
        {
            for ( int j = tokenStart; j < tokenEnd; j++ )
            {
                Text[ j ].Brush = brush;
            }
        }
    }
}
