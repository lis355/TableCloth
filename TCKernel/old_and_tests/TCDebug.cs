using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TableClothKernel
{
    public class TcDebug
    {
        static void Main()
        {
            Log( "Starting..." );

            Console.ReadLine();
        }
        
        static readonly List<string> _logStrings = new List<string>();

        public static void Log( string s )
        {
            s = String.Format( "[{0}] : {1}", DateTime.Now.ToUniversalTime(), s );
            _logStrings.Add( s );
            Console.WriteLine( s );
        }
        
        public static void SaveLog()
        {
            File.WriteAllLines( "log.txt", _logStrings, Encoding.UTF8 );
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////

        /*public static void LOG( string s ) { TB.Text += String.Format( "{0}) {1}\r\n", ttt++, s ); }
        
        static string[] Expressions =
        {  
            "!(!(1 ^ 0) => !(1 [equ] wy)) && r!q", "True"
            //"x = false", "False",
            //"y = true || false", "True",
            //"y => x", "True",
            //"q || !0 && 1", "True",
            //"!!!1 && 0 && 1", "False",
            //"!(0 && 1)", "True",
            //"1", "True",
            //"!1", "False",
            //"0 || 0 && !1", "0; ",
            //"!!(0 && 1)", "0; ",
            //"true", "1; "
        };

        static System.Windows.Forms.Form MF;
        static System.Windows.Forms.TextBox TB, TBI, TBV;
        static System.Windows.Forms.TabControl TC;
        static int NUMBEREXP = 0;

        static uint ttt;
        static public Expression TODRAW;

        static void Main()
        {
            #if !DEBUG
            try
            {
            #endif
                App();
            #if !DEBUG
            }
            catch( Exception e )
            {
                Log( "Exception : " + e.Message );
            }
            finally
            {
                SaveLog();
            }
            #endif
        }

        static void App()
        {
            //Operand op = new OrOperator( Constant.False, new Variable( "qq" ) );

            ///////////////////////////////
            Init();

            Options.ConstantOutType = StringConstantType.Word;

            for ( int i = 0; i < Expressions.Length; i += 2, NUMBEREXP++ )
                CheckExp( i / 2, Expressions[ i ], Expressions[ i + 1 ] );

            foreach ( String vp in GlobalVariableList.V.GetAllVariablesNames() )
            {
                TBV.Text += "{ " + vp + " := " + GlobalVariableList.GetCalcValue( vp ).ToString() + " } ";
            }

            TB.Text += "[not] [and] [or] [xor \u2295] [equ] [impl \u21D2] [shef] [pirse \u2193]";

            // start programm
            Application.Run( MF );
        }

        static void CheckExp(int num, string exp, string res)
        {
            ttt = 0;
            TB.Text += String.Format("Exp {0} : {1}\r\n", num, exp);

            string result = "";
            Calc.CalcExpression(exp);
            TB.Text += String.Format("ARRAY RES:{0}\r\nCALT RES:{1}\r\n", res, result);
            if (result == res)
            {
                TB.Text += String.Format("SUCCESS\r\n");
            }
            //if (TODRAW != null)
            //{
            //    System.Windows.Forms.TextBox SHMI = new System.Windows.Forms.TextBox();
            //    SHMI.Dock = System.Windows.Forms.DockStyle.Top;
            //    SHMI.Font = new System.Drawing.Font("Courier New", 12.0f);
            //    SHMI.Text = exp + "         RES: " + res + " STRING: " + TODRAW.Root.ToString();
            //    TC.TabPages.Add("SHM " + num.ToString());
            //    ExpressionScheme ES = new ExpressionScheme(TODRAW);
            //    //ES.SaveScheme("sctc");
            //    TC.TabPages[TC.TabPages.Count - 1].Controls.Add(ES);
            //    TC.TabPages[TC.TabPages.Count - 1].Controls.Add(SHMI);
            //    TC.SelectedIndex = TC.TabPages.Count - 1;
            //}
            TB.Text += String.Format("\r\n");
        }

        static void CALC_Click(object sender, EventArgs e)
        {
            CheckExp(NUMBEREXP++, TBI.Text, "");
            TBI.Text = "";
        }

        static void Init()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault( false );
            MF = new System.Windows.Forms.Form();
            MF.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            MF.Size = new System.Drawing.Size( 800, 600 );
            TB = new System.Windows.Forms.TextBox();
            TB.Multiline = true;
            TBV = new System.Windows.Forms.TextBox();
            TBV.Multiline = true;
            TBV.Dock = System.Windows.Forms.DockStyle.Top;
            TBV.BackColor = Color.LightGreen;
            TB.Font = new System.Drawing.Font( "Courier New", 12.0f );
            TB.Dock = System.Windows.Forms.DockStyle.Fill;
            TC = new System.Windows.Forms.TabControl();
            TC.Dock = System.Windows.Forms.DockStyle.Fill;
            TC.TabPages.Add( "EXP" );
            System.Windows.Forms.Button CALC = new System.Windows.Forms.Button();
            CALC.Dock = System.Windows.Forms.DockStyle.Top;
            CALC.Text = "CHECK";
            CALC.Click += new EventHandler( CALC_Click );
            TBI = new System.Windows.Forms.TextBox();
            TBI.Dock = System.Windows.Forms.DockStyle.Top;
            TBI.Font = new System.Drawing.Font( "Courier New", 12.0f );
            TBI.Text = "!1 && !0 == !(1 || 0)";
            TC.TabPages[ 0 ].Controls.Add( TB );
            TC.TabPages[ 0 ].Controls.Add( TBV );
            TC.TabPages[ 0 ].Controls.Add( CALC );
            TC.TabPages[ 0 ].Controls.Add( TBI );
            TB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            MF.Controls.Add( TC );
        }
    }*/
}
