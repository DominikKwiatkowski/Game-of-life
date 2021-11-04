using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AnimatedButton.xaml
    /// </summary>
    public partial class AnimatedButton : UserControl
    {
        /// <summary>
        /// Path to image property definition.
        /// </summary>
        public String PathToImage
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("PathToImage", typeof(string), typeof(AnimatedButton), new UIPropertyMetadata(""));

        /// <summary>
        /// ContextText property definition.
        /// </summary>
        public String ContentText
        {
            get { return (string)GetValue(ContentTextProperty); }
            set { SetValue(ContentTextProperty, value); }
        }

        public static readonly DependencyProperty ContentTextProperty =
            DependencyProperty.Register("ContentText", typeof(string), typeof(AnimatedButton), new UIPropertyMetadata(""));

        public AnimatedButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event EventHandler ExecuteMethod;

        protected virtual void OnExecuteMethod()
        {
            if (ExecuteMethod != null) ExecuteMethod(this, EventArgs.Empty);
        }

        public void Click(object sender, EventArgs e)
        {
            OnExecuteMethod();
        }
    }
}
