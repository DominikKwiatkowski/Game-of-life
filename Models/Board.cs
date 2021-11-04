using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using GameOfLife.Common;
using GameOfLife.Converters;
using GameOfLife.Enums;
using Microsoft.Win32;

namespace GameOfLife.Models
{
    public class Board
    {
        public List<List<Field>> Fields { get; set; } = new List<List<Field>>();
        public int Width { get; set; }
        public int Height { get; set; }
        public int Generation = 1;
        public List<List<Change>> History { get; set; } = new List<List<Change>>();

        public Board(int width, int height)
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

        public Board() { }

        public void Recalculate()
        {
            BoardSpecialToNormal();
            ApplyPreviosuStatus();
            ApplyFutureStatus();
        }

        public void NextGen(bool isAdvanced)
        {
            if (isAdvanced)
            {
                NextGenAdvance();
            }
            else
            {
                NextGenNormal();
            }
        }

        public void PreviousGen(bool isAdvanced)
        {
            if (isAdvanced)
            {
                PreviousGenAdvanced();
            }
            else
            {
                PreviousGenNormal();
            }
        }
        public void NextGenAdvance()
        {
            BoardSpecialToNormal();
            NextGenNormal();
            ApplyPreviosuStatus();
            ApplyFutureStatus();
        }

        public void NextGenNormal()
        {
            Generation++;
            List<Change> listOfChanges = CalculateChangeList();

            foreach (var change in listOfChanges)
            {
                Fields[change.XPos][change.YPos].FieldStatus = change.NewStatus;
            }

            History.Add(listOfChanges);
        }

        public void PreviousGenNormal()
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[^1];
                History.Remove(listOfChanges);

                foreach (var change in listOfChanges)
                {
                    Fields[change.XPos][change.YPos].FieldStatus = change.OldStatus;
                }

                Generation--;
            }
        }

        public void PreviousGenAdvanced()
        {
            if (Generation > 1)
            {
                BoardSpecialToNormal();
                PreviousGenNormal();
                ApplyPreviosuStatus();
                ApplyFutureStatus();
            }
        }

        public void BoardSpecialToNormal()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Fields[i][j].specialToNormal();
                }
            }
        }

        public void ApplyFutureStatus()
        {
            List<Change> listOfChanges = CalculateChangeList();
            foreach (var change in listOfChanges)
            {
                if (change.NewStatus == Status.Alive)
                {
                    Fields[change.XPos][change.YPos].FieldStatus = Status.WillRise;
                }
                else
                {
                    Fields[change.XPos][change.YPos].FieldStatus = Status.WillDie;
                }
            }
        }

        public void ApplyPreviosuStatus()
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[^1];
                foreach (var change in listOfChanges)
                {
                    if (change.NewStatus == Status.Alive)
                    {
                        Fields[change.XPos][change.YPos].FieldStatus = Status.Born;
                    }
                    else
                    {
                        Fields[change.XPos][change.YPos].FieldStatus = Status.Died;
                    }
                }
            }
        }

        private List<Change> CalculateChangeList()
        {
            List<Change> listOfChanges = new List<Change>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if ((!Fields[i][j].isAlive()) &&
                        (NumberOfNeighbours(i, j) == 3))
                    {
                        listOfChanges.Add(new Change(
                            i, j, Status.Dead, Status.Alive));
                    }
                    else if ((Fields[i][j].isAlive()) &&
                             ((NumberOfNeighbours(i, j) != 3) && (
                                 NumberOfNeighbours(i, j) != 2)))
                    {
                        listOfChanges.Add(new Change(
                            i, j, Status.Alive, Status.Dead));
                    }
                }
            }

            return listOfChanges;
        }

        private int NumberOfNeighbours(int yPos, int xPos)
        {
            int numberOfNeighbours = 0;
            if (xPos > 0)
            {
                if ((yPos > 0) && (Fields[yPos - 1][xPos - 1].isAlive()))
                {
                    numberOfNeighbours++;
                }

                if (Fields[yPos][xPos - 1].isAlive())
                {
                    numberOfNeighbours++;
                }

                if ((yPos < Height - 1) && (Fields[yPos + 1][xPos - 1].isAlive()))
                {
                    numberOfNeighbours++;
                }
            }

            if ((yPos > 0) && (Fields[yPos - 1][xPos].isAlive()))
            {
                numberOfNeighbours++;
            }

            if ((yPos < Height - 1) && (Fields[yPos + 1][xPos].isAlive()))
            {
                numberOfNeighbours++;
            }

            if (xPos < Width - 1)
            {
                if ((yPos > 0) && (Fields[yPos - 1][xPos + 1].isAlive()))
                {
                    numberOfNeighbours++;
                }

                if (Fields[yPos][xPos + 1].isAlive())
                {
                    numberOfNeighbours++;
                }

                if ((yPos < Height - 1) && (Fields[yPos + 1][xPos + 1].isAlive()))
                {
                    numberOfNeighbours++;
                }
            }

            return numberOfNeighbours;
        }

        public void Dump(int dumpWidth, int dumpHeight, int widthCellSize, int heightCellSize)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(dumpWidth, dumpHeight, 96,96, PixelFormats.Default);
            DrawingVisual dVisual = new DrawingVisual();
            using (DrawingContext dc = dVisual.RenderOpen())
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        dc.DrawRectangle(
                            (Brush)new StatusToBrushConverter().Convert(Fields[i][j].FieldStatus, null, null,
                                CultureInfo.CurrentCulture),
                            new Pen(Brushes.Black, 1),
                            new Rect(j * widthCellSize, i * heightCellSize, widthCellSize, heightCellSize));
                    }
                }
            }
            bitmap.Render(dVisual);
            System.IO.Directory.CreateDirectory("Dump");
            using (FileStream stream = new FileStream($"Dump//Gen{Generation}.bmp", FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }
        }

        public void AddToLast(Field field)
        {
            if (Generation > 1)
            {
                List<Change> listOfChanges = History[^1];


                Tuple<int, int> pos = FindPos(field);

                if (field.isAlive())
                {
                    listOfChanges.Add(new Change(
                        pos.Item1, pos.Item2, Status.Alive, Status.Dead));
                }
                else
                {
                    listOfChanges.Add(new Change(
                        pos.Item1, pos.Item2, Status.Dead, Status.Alive));
                }
            }
        }

        private Tuple<int, int> FindPos(Field field)
        {
            int i, j = 0;
            for (i = 0; i < Height; i++)
            {
                for (j = 0; j < Width; j++)
                {
                    if (Fields[i][j].Equals(field))
                        return new Tuple<int, int>(i,j);
                }
            }

            throw new ArgumentException();
        }
    }
}