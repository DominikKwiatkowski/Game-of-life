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

        /// <summary>
        /// Check if object is alive.
        /// </summary>
        /// <returns>true if alive, false otherwise</returns>
        public bool IsAlive()
        {
            return FieldStatus is Status.Alive or Status.WillDie or Status.Born;
        }

        /// <summary>
        /// Cast advanced mode to normal one.
        /// </summary>
        public void SpecialToNormal()
        {
            if (IsAlive())
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
