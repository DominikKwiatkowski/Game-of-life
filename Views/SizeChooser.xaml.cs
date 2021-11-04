using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife.Views
{
    /// <summary>
    /// Interaction logic for SizeChooser.xaml
    /// </summary>
    public partial class SizeChooser : UserControl
    {
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(99));

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(8));

        public int Size
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(14));

        public string ChooserName
        {
            get { return (string)GetValue(ChooserNameProperty); }
            set { SetValue(ChooserNameProperty, value); }
        }

        public static readonly DependencyProperty ChooserNameProperty =
            DependencyProperty.Register("ChooserName", typeof(string), typeof(SizeChooser), new UIPropertyMetadata(""));
        public SizeChooser()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            bool match = regex.IsMatch(e.Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void UpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Size < MaxValue;
        }

        private void UpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Size++;
        }

        private void DownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Size > MinValue;
        }
        private void DownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Size--;
        }
    }
}
