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

        /// <summary>
        /// Creates game shape of given size.
        /// </summary>
        /// <param name="width">Width of shape</param>
        /// <param name="height">Height of shape</param>
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

        /// <summary>
        /// Read shape from given file.
        /// </summary>
        /// <param name="filePath">Path to shape definition</param>
        /// <returns></returns>
        public static GameShape ReadFromFile(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);

            int width = lines[0].Length;
            int height = lines.Length;
            GameShape gameShape = new GameShape(width, height);
            gameShape.Name = Path.GetFileNameWithoutExtension(filePath);

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

        /// <summary>
        /// Create from this shape an bitmap image.
        /// </summary>
        /// <param name="dumpWidth">Width of Image</param>
        /// <param name="dumpHeight">Height of Image</param>
        /// <param name="pathToSave">Path to which file will be saved</param>
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
