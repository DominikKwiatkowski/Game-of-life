using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfLife.Converters;

namespace GameOfLife.Models
{
    static class BitmapCreator
    {
        /// <summary>
        /// Render bitmap from given parameters
        /// </summary>
        /// <param name="dumpWidth">Width of image</param>
        /// <param name="dumpHeight">Height of image</param>
        /// <param name="widthCellSize">Width size of each cell</param>
        /// <param name="heightCellSize">Height size of each cell</param>
        /// <param name="height">Height of board</param>
        /// <param name="width">Width of board</param>
        /// <param name="fields">Fields 2-dimensional list to be rendered</param>
        /// <returns></returns>
        public static RenderTargetBitmap MakeBitmap(int dumpWidth, int dumpHeight, int widthCellSize,
            int heightCellSize, int width, int height, List<List<Field>> fields)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(dumpWidth, dumpHeight, 96, 96, PixelFormats.Default);
            DrawingVisual dVisual = new DrawingVisual();
            using (DrawingContext dc = dVisual.RenderOpen())
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        dc.DrawRectangle(
                            (Brush)new StatusToBrushConverter().Convert(fields[i][j].FieldStatus, null, null,
                                CultureInfo.CurrentCulture),
                            new Pen(Brushes.Black, 1),
                            new Rect(j * widthCellSize, i * heightCellSize, widthCellSize, heightCellSize));
                    }
                }
            }
            bitmap.Render(dVisual);
            return bitmap;
        }
    }
}
