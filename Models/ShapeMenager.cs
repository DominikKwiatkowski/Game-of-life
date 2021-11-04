using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public class ShapeMenager
    {
        public List<GameShape> ShapeList { get; set; } = new List<GameShape>();
        const int imgSize = 100;

        /// <summary>
        /// Get all shapes from given directory and create image of them.
        /// </summary>
        /// <param name="directory">Directory from we load shapes</param>
        /// <param name="imgDirectory">Target image directory</param>
        public ShapeMenager(string directory, string imgDirectory)
        {
            //Get all text files in directory
            try
            {
                string[] files = System.IO.Directory.GetFiles(directory, "*.txt");


                foreach (string file in files)
                {
                    GameShape shape = GameShape.ReadFromFile(file);
                    ShapeList.Add(shape);
                }
            }
            catch (System.IO.IOException)
            {

            }

            foreach (var shape in ShapeList)
            {
                shape.CreateShapeImage(imgSize, imgSize, imgDirectory);

            }
        }
    }
}
