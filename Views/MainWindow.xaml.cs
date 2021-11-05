using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameOfLife.Common;
using GameOfLife.Converters;
using GameOfLife.Enums;
using GameOfLife.Models;
using Microsoft.Win32;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Board board;
        public int WidthBoardSize { get; set; }  = 800;
        public int HeightBoardSize { get; set; } = 800;
        public int WidthValue { get; set; } = 16;
        public int HeightValue { get; set; } = 16;
        public int WidthCellSize { get; set; }
        public int HeightCellSize { get; set; }
        public int NumberOfGenerations { get; set; } = 1;

        public ShapeMenager ShapeMenager { get; set; }

        public string ImgDirectory { get; set; }
        private bool _advanced;
        public bool Advanced
        {
            get { return _advanced;}
            // If advanced value change, we have to recalculate board.
            set
            {
                _advanced = value;
                if (value == true)
                {
                    board.ApplyPreviousStatus();
                    board.ApplyFutureStatus();
                }
                else
                {
                    board.BoardSpecialToNormal();
                }
                CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
            }
        }

        public bool Dump { get; set; } = false;
        public bool ShapeMode { get; set; } = false;
        public GameShape CurrentShape { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Add methods to animated buttons, it must be done via code for some reason.
            Start.ExecuteMethod += StartGame;
            Next.ExecuteMethod += NextTurn;
            Previous.ExecuteMethod += PreviousTurn;

            ImgDirectory = System.IO.Directory.CreateDirectory("ImgDir").FullName;
            ShapeMenager = new ShapeMenager("GameShapes", ImgDirectory);
        }

        /// <summary>
        /// On click method, when user will define board size and begin game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame(object sender, EventArgs e)
        {
            board = new Board(WidthValue, HeightValue);
            WidthCellSize = WidthBoardSize / WidthValue;
            HeightCellSize = HeightBoardSize / HeightValue;
            Board.ItemsSource = board.Fields;
            ShapeMenagerItemControl.ItemsSource = ShapeMenager.ShapeList;
            StartPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;
            GameBoardPanel.Visibility = Visibility.Visible;
        }

        public ICommand ChangeStatusCommand { get { return new RelayCommand<Field>(ChangeStatus); } }

        /// <summary>
        /// Change status method for fields. If we are in ShapeMode, it will produce shape,
        /// otherwise it will change live status.
        /// </summary>
        /// <param name="field">field which was clicked</param>
        public void ChangeStatus(Field field)
        {
            if (ShapeMode)
            {
                board.AddShape(CurrentShape, field);
                ShapeMode = false;
            }
            else
            {
                board.AddToLast(field);
                if (field.IsAlive())
                    field.FieldStatus = Status.Dead;
                else
                    field.FieldStatus = Status.Alive;
            }

            if(Advanced)
                board.Recalculate();
            CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
        }

        public ICommand ChooseShapeCommand { get { return new RelayCommand<GameShape>(ChooseShape); } }

        /// <summary>
        /// Shape button left click method. It will remember chosen shape and turn on shape mode
        /// </summary>
        /// <param name="shape"></param>
        public void ChooseShape(GameShape shape)
        {
            ShapeMode = !ShapeMode;
            CurrentShape = shape;
        }

        /// <summary>
        /// Count next turns, depending on number of turns defined by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTurn(object sender, EventArgs e)
        {
            if (Dump)
            {
                board.Dump(WidthBoardSize,HeightBoardSize, WidthCellSize, HeightCellSize);
            }
            for (int i = 0; i < NumberOfGenerations; i++)
            {
                board.NextGen(Advanced);
            }
            CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
        }

        /// <summary>
        /// Restore previous turns, depending on number of turns defined by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousTurn(object sender, EventArgs e)
        {
            if (Dump)
            {
                board.Dump(WidthBoardSize, HeightBoardSize, WidthCellSize, HeightCellSize);
            }
            for (int i = 0; i < NumberOfGenerations; i++)
            {
                board.PreviousGen(Advanced);
            }
            CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
        }

        /// <summary>
        /// Save current board to file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML-File | *.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                CommonUtils.WriteToXmlFile<Board>(saveFileDialog.FileName, board);
            }
        }

        /// <summary>
        /// Load board from file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load(object sender, EventArgs e)
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.Filter = "XML-File | *.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                board=CommonUtils.ReadFromXmlFile<Board>(saveFileDialog.FileName);
                Board.ItemsSource = null;
                WidthCellSize = WidthBoardSize / board.Width;
                HeightCellSize = HeightBoardSize / board.Height;
                Board.ItemsSource = board.Fields;
                CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
            }
        }

        /// <summary>
        /// Recalculate board and cell sizes when window size change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {

            HeightBoardSize = (int)(ActualHeight - 280);
            WidthBoardSize = (int)ActualWidth - 280;
            WidthCellSize = WidthBoardSize / WidthValue;
            HeightCellSize = HeightBoardSize / HeightValue;
            if(board != null)
                CollectionViewSource.GetDefaultView(Board.ItemsSource).Refresh();
        }
    }
}
