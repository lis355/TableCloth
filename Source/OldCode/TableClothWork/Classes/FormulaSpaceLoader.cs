using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace BinaryCalc
{
    /// <summary>
    /// Класс для загрузки пространств из файла
    /// </summary>
    public static class FormulaSpaceLoader
    {
        static XmlDocument Doc, Scheme;

        public static void LoadScheme()
        {
//             Scheme = XDocument.Load("tcflscheme.xsd");
//             XmlSchemaSet shem = new XmlSchemaSet();
//             shem.Add(null, Scheme.CreateReader());
            //Doc.Validate
        }

        public static bool LoadFromFile(ref string FileName, BCalcProgram P, VariablesForms VF)
        {
            /*debug*/return true;
            /*
            try
            {
                Doc = new XmlDocument();
                Doc.LoadXml(System.IO.File.ReadAllText(FileName));
                P.FSpaceList.ClearTotalAmount();

                XmlNodeList Title = Doc.GetElementsByTagName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString() + "Note");
                if (Title.Count == 0) throw new System.Exception("Нет главного тега тетради");
                if (Title[0].Attributes.Count == 0) throw new System.Exception("Нет атрибутов у тега тетради");
                else if (Title[0].Attributes[0].Name != "name") throw new System.Exception("Некорректный атрибут у тега тетради");
                else FileName = Title[0].Attributes[0].Value;

                // рисуем пространства ввода
                foreach (XmlNode x in Doc.GetElementsByTagName("Space"))
                {
                    if (x.ChildNodes[0].Name != "In" &&
                        x.ChildNodes[1].Name != "Out" &&
                        x.ChildNodes[2].Name != "Min")
                        throw new System.Exception("Недостаточное количество тегов у пространства ввода");

                    P.FSpaceList.AddBack();
                    if (x.ChildNodes[0].InnerText != "NULL")
                        P.FSpaceList[P.FSpaceList.Count - 1].TextInput = x.ChildNodes[0].InnerText;
                    //if (x.ChildNodes[1].InnerText == "#") P.FSpaceList[P.FSpaceList.Count - 1].MakeText();
                    else if (x.ChildNodes[1].InnerText != "NULL")
                    {
                        P.FSpaceList[P.FSpaceList.Count - 1].TextOutput = x.ChildNodes[1].InnerText;
                        P.FSpaceList[P.FSpaceList.Count - 1].SecondRowVisible = true;
                    }
                    if (x.ChildNodes[2].InnerText == "True")
                        P.FSpaceList[P.FSpaceList.Count - 1].Min();
                    else if (x.ChildNodes[2].InnerText != "False") throw new System.Exception("Некорректное значение у тега <Min>");
                }

                foreach (XmlNode x in Doc.GetElementsByTagName("Variable"))
                {
                    if (x.ChildNodes[0].Name != "Name" &&
                        x.ChildNodes[1].Name != "Commands" &&
                        x.ChildNodes[2].Name != "Arguments")
                        throw new System.Exception("Недостаточное количество тегов у тега функции");

                    Formula F = new Formula();
                    foreach (XmlNode xc in x.ChildNodes[2].ChildNodes)
                    {
                        if (xc.Name != "Id") throw new System.Exception("Некорректный тег идентификатора");
                        else if (Syntax.IsVariable(xc.InnerText) == false) throw new System.Exception("Некорректный тег идентификатора");
                        F.AddVariableName(xc.InnerText);
                    }

                    string com = x.ChildNodes[1].InnerText, num = "";
                    for (int i = 0; i < com.Length; i++)
                    {
                        if (com[i] == '2')
                        {
                            F.Commands.Add(CommandsCode.PushVariable);
                            i+=2;
                            num = "";
                            while (Char.IsDigit(com[i])) num += com[i++];
                            F.Commands.Add(Convert.ToInt32(num));
                        }
                        else if (com[i] == '0') F.Commands.Add(CommandsCode.PushFalse);
                        else if (com[i] == '1') F.Commands.Add(CommandsCode.PushTrue);
                        else if (com[i] == '3') F.Commands.Add(CommandsCode.OperationNot);
                        else if (com[i] == '4') F.Commands.Add(CommandsCode.OperationAnd);
                        else if (com[i] == '5') F.Commands.Add(CommandsCode.OperationOr);
                        else if (com[i] == '6') F.Commands.Add(CommandsCode.OperationXor);
                        else if (com[i] == '7') F.Commands.Add(CommandsCode.OperationImplication);
                        else if (com[i] == '8') F.Commands.Add(CommandsCode.OperationSheffer);
                        else if (com[i] == '9') F.Commands.Add(CommandsCode.OperationEquivalence);
                        else throw new System.Exception("Некорректные команды формулы");
                    }

                    if (Syntax.IsVariable(x.ChildNodes[0].InnerText) == false) throw new System.Exception("Некорректный идентификатор");
                    else P.FVariables.Set(x.ChildNodes[0].InnerText, F);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Файл поврежден\n" + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VF.DeleteVarSpace(System.IO.Path.GetFileNameWithoutExtension(FileName));
                return false;
            }
        */
        }
    }
}
