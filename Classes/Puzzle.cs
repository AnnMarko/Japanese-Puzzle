using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapanezePuzzle.Classes
{
    public class Puzzle
    {
        private string _name;
        private int _rows;
        private int _columns;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
        public int Cols
        {
            get { return _columns; }
            set { _columns = value; }
        }
    }
}
