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
        public static RenderTargetBitmap MakeBitmap(int dumpWidth, int dumpHeight, int widthCellSize,
            int heightCellSize, int height, int width, List<List<Field>> fields)
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
