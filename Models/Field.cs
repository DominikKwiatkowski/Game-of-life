using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using GameOfLife.Enums;

namespace GameOfLife.Models
{
    public class Field
    {
        public Status FieldStatus { get; set; }= Status.Dead;

        public bool isAlive()
        {
            return FieldStatus is Status.Alive or Status.WillDie or Status.Born;
        }

        public void specialToNormal()
        {
            if (isAlive())
            {
                FieldStatus = Status.Alive;
            }
            else
            {
                FieldStatus = Status.Dead;
            }
        }
    }
}
