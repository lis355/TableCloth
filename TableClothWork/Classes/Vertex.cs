using System.Drawing;

namespace BinaryCalc
{
    public class Vertex
    {
        public Vertex R, L, Parent;
        public bool IsOperation;
        public string Str;//если это лист то в нем записан идентификатор операнда
        public int Level, Code;
        public Point In, Out;

        /// <summary>
        /// Создать вершину
        /// </summary>
        /// <param name="Rv">Правый сын</param>
        /// <param name="Lv">Левый сын</param>
        /// <param name="P">Отец</param>
        /// <param name="Lev">Уровень в дереве от листа</param>
        /// <param name="Op">Является ли операцией либо лист</param>
        /// <param name="Name">Имя переменной</param>
        /// <param name="C">Код операции</param>
        /// <param name="xin">Координато точки входа по x</param>
        /// <param name="yin">Координато точки входа по y</param>
        /// <param name="xout">Координато точки выхода поx</param>
        /// <param name="yout">Координато точки выхода по y</param>
        public Vertex(Vertex Rv, Vertex Lv, Vertex P, int level, bool Operation, string Name, int C, int xin = 0, int yin = 0, int xout = 0, int yout = 0)
        {
            R = Rv;
            L = Lv;
            Parent = P;
            IsOperation = Operation;
            Str = Name;
            Level = level;
            Code = C;
            In = new Point(xin, yin);
            Out = new Point(xout, yout);
        }
    }
}
