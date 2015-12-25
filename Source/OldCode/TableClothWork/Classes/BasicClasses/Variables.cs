using System.Collections.Generic;
using UrielGuy.SyntaxHighlightingTextBox;

namespace BinaryCalc
{
    /// <summary>
    /// Список переменных, основан на словаре для более быстрой проверки на существование элемента
    /// </summary>
    public class Variables
    {
        /// <summary>
        /// Список переменных
        /// </summary>
        public Dictionary<string, Formula> VariablesTable;

        /// <summary>
        /// Список для подсветки переменных
        /// </summary>
        public Dictionary<string, HighlightDescriptor> VarsHighlight;

        /// <summary>
        /// Ссылки на обьекты классов
        /// </summary>
        VariablesForms T;

        /// <summary>
        /// Имя пространства переменных
        /// </summary>
        string VarSpaceName;

        BCalcProgram Parent;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Variables(ref VariablesForms t, string S, BCalcProgram P)
        {
            VariablesTable = new Dictionary<string, Formula>();
            VarsHighlight = new Dictionary<string, HighlightDescriptor>();
            Parent = P;
            VarSpaceName = S;
            T = t;
        }

        /// <summary>
        /// Проверка существования переменной
        /// </summary>
        public bool IsExist(string Name)
        {
            return VariablesTable.ContainsKey(Name);
        }

        /// <summary>
        /// Установить новое значение существующей переменной либо
        /// при ее отсутствии создать новую
        /// </summary>
        public void Set(string Name, Formula F)
        {
            if (IsExist(Name) == false)
            {
                // добавляем
                T.SetVarFromNote(VarSpaceName, Name, F.ConvertToString());
                VariablesTable.Add(Name, F);

                // теперь нужно добавить подсветку этой переменной
                VarsHighlight.Add(Name, new HighlightDescriptor(Name, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0))))), null, DescriptorType.Word, DescriptorRecognition.WholeWord, false));
                Parent.FSpaceList.AddHighlight(VarsHighlight[Name]);
            }
            else
            {
                VariablesTable[Name] = F;
                T.SetVarFromNote(VarSpaceName, Name, F.ConvertToString());
            }
        }

        /// <summary>
        /// Удаление переменной и ее подсветки
        /// </summary>
        public void Delete(string Name)
        {
            if (IsExist(Name) == true)
            {
                // теперь нужно удалить подсветку этой переменной
                Parent.FSpaceList.RemoveHighlight(VarsHighlight[Name]);
                VarsHighlight.Remove(Name);
                VariablesTable.Remove(Name);
            }
        }

        /// <summary>
        /// Свойство - имя пространства переменных
        /// </summary>
        public string VariablesSpaceName
        {
            get
            {
                return VarSpaceName;
            }
            set
            {
                T.SetVarSpace(value, this);
                VarSpaceName = value;
            }
        }
        
        /// <summary>
        /// Индексатор списка переменных, возвращает формулу по идентификатору
        /// </summary>
        public Formula this[string s]
        {
            get
            {
                return VariablesTable[s];
            }
        }
    }
}
