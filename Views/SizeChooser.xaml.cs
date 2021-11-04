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
        /// <summary>
        /// Max Value property definition.
        /// </summary>
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(99));

        /// <summary>
        /// Min Value property definition.
        /// </summary>
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(8));

        /// <summary>
        /// Size property definition.
        /// </summary>
        public int Size
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(SizeChooser), new UIPropertyMetadata(14));

        /// <summary>
        /// Name property definition.
        /// </summary>
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

        /// <summary>
        /// Number validation rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            bool match = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Check if size can be increment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Size < MaxValue;
        }

        /// <summary>
        /// Increment size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Size++;
        }

        /// <summary>
        /// Check if size can be decremented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Size > MinValue;
        }

        /// <summary>
        /// Decrement size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Size--;
        }
    }
}
