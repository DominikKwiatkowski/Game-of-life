using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Enums;

namespace GameOfLife.Models
{
    public class Change
    {
        public int XPos;
        public int YPos;
        public Status OldStatus;
        public Status NewStatus;

        /// <summary>
        /// Creates board change from given parameters
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        public Change(int xPos, int yPos, Status oldStatus, Status newStatus)
        {
            XPos = xPos;
            YPos = yPos;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        public Change() {}
    }
}
