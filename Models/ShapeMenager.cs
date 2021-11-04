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
        // Get all GameShapes from files in /GameShape directory
        public ShapeMenager(string directory, string imgDir)
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
                shape.CreateShapeImage(imgSize, imgSize, imgDir);

            }

        }

    }
}
