using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BinaryCalc
{
    /// <summary>
    /// Класс для сохранения пространств в файл
    /// </summary>
    public static class FormulaSpaceSaver
    {
        static int FormatLanguageVersion = 2;

        static XmlDocument Doc;

        public static bool SaveToFile(string FileName, BCalcProgram P)
        {/*
            Doc = new XmlDocument();

            XmlElement FormatLanguage, Title, Space, SIn, SOut, SMin,
                Variable, Name, Commands, Arguments, Identificator;

            string RealName = FileName;

            FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);

            // пишем комментарий версию программы
            XmlComment C = Doc.CreateComment(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()
                + " version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Doc.InsertBefore(C, null);

            // пишем версию языка разметки
            FormatLanguage = Doc.CreateElement(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()
                +"FormatLanguage");
            FormatLanguage.InnerText = Convert.ToString(FormatLanguageVersion);
            Doc.InsertBefore(FormatLanguage, null);

            // главный тег 
            Title = Doc.CreateElement(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString()+"Note");
            XmlAttribute TitleAttribute = Doc.CreateAttribute("name");
            TitleAttribute.Value = FileName;
            Title.SetAttributeNode(TitleAttribute);
            Title.AppendChild(FormatLanguage);

//             for (int i = 0; i < P.FSpaceList.Count; i++)
//             {
//                 Space = Doc.CreateElement("Space");
//                 SIn = Doc.CreateElement("In"); 
//                 SOut = Doc.CreateElement("Out");
//                 SMin = Doc.CreateElement("Min");
// 
//                 int n = i + P.FSpaceList[0].Number;
// 
//                 SIn.InnerText = (P.FSpaceList[n].TextInput == "") ? "NULL" : P.FSpaceList[n].TextInput;
//                 SOut.InnerText = (P.FSpaceList[n].TextOutput == "") ? "NULL" : P.FSpaceList[i].TextOutput;
//                 SMin.InnerText = (P.FSpaceList[n].Minimized == true) ? "True" : "False";
// 
//                 Space.AppendChild(SIn);
//                 Space.AppendChild(SOut);
//                 Space.AppendChild(SMin);
// 
//                 Title.AppendChild(Space);
//             }

            foreach (FormulaSpace x in P.FSpaceList.FormulaSpaceVector.Values)
            {
                Space = Doc.CreateElement("Space");
                SIn = Doc.CreateElement("In");
                SOut = Doc.CreateElement("Out");
                SMin = Doc.CreateElement("Min");

                SIn.InnerText = (x.TextInput == "") ? "NULL" : x.TextInput;
                SOut.InnerText = (x.TextOutput == "") ? "NULL" : x.TextOutput;
                SMin.InnerText = (x.Minimized == true) ? "True" : "False";

                Space.AppendChild(SIn);
                Space.AppendChild(SOut);
                Space.AppendChild(SMin);

                Title.AppendChild(Space);
            }

            foreach (KeyValuePair<string, Formula> x in P.FVariables.VariablesTable.ToArray())
            {
                Variable = Doc.CreateElement("Variable");
                Name = Doc.CreateElement("Name");
                Commands = Doc.CreateElement("Commands");
                Arguments = Doc.CreateElement("Arguments");

                Name.InnerText = x.Key;
                                
                for (int j = 0; j < x.Value.Commands.Count; j++)
                {
                    Commands.InnerText += x.Value.Commands[j];
                	if (x.Value.Commands[j] == CommandsCode.PushVariable)
                        Commands.InnerText += '(' + Convert.ToString(x.Value.Commands[++j]) + ')';
                }

                for (int j = 0; j < x.Value.VariableCount; j++)
                {
                    Identificator = Doc.CreateElement("Id");
                    Identificator.InnerText = x.Value.VariableName[j];
                    Arguments.AppendChild(Identificator);
                }

                Variable.AppendChild(Name);
                Variable.AppendChild(Commands);
                Variable.AppendChild(Arguments);

                Title.AppendChild(Variable);
            }

            P.FVariables.VariablesSpaceName = FileName;
            SelectedNote.Ref.Text = FileName;

            Doc.InsertBefore(Title,null);
            Doc.Save(RealName);*/
            return true;
        }
    }
}
