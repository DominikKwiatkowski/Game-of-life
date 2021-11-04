using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GameOfLife.Enums;

namespace GameOfLife.Models
{
    public class GameShape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<List<Field>> Fields { get; set; } = new List<List<Field>>();

        public string Name { get; set; }
        public string FilePath { get; set; } = "";

        public GameShape(int width, int height)
        {
            Width = width;
            Height = height;
            for (int i = 0; i < Height; i++)
            {
                Fields.Add(new List<Field>());
                for (int j = 0; j < Width; j++)
                {
                    Fields[i].Add(new Field());
                }
            }
        }

        public static GameShape ReadFromFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);

            int width = lines[0].Length;
            int height = lines.Length;
            GameShape gameShape = new GameShape(width, height);
            gameShape.Name = Path.GetFileNameWithoutExtension(fileName);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (lines[i][j] == '1')
                    {
                        gameShape.Fields[i][j].FieldStatus = Status.Alive;
                    }
                }
            }
            return gameShape;
        }

        public void CreateShapeImage(int dumpWidth, int dumpHeight, string pathToSave)
        {
            int widthCellSize = dumpWidth / Width;
            int heightCellSize = dumpHeight / Height;
            RenderTargetBitmap bitmap = BitmapCreator.MakeBitmap(dumpWidth, dumpHeight, widthCellSize, heightCellSize,
                Height, Width, Fields);
            FilePath = $"{pathToSave}//{Name}.bmp";
            using (FileStream stream = new FileStream(FilePath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }
        }
    }
}
