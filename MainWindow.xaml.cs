using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SupermarketHack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame? mainFrame = null;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                mainFrame = (Frame)Template.FindName("MainFrame", this);
                mainFrame.Content = new Pages.Page1();
            };

            StateChanged += (s, e) =>
            {
                if (s is Window window)
                {
                    if (window.WindowState == WindowState.Maximized || window.WindowState == WindowState.Normal)
                    {
                        Storyboard storyboard = (Storyboard)FindResource("WindowLoadAnimation");

                        if (storyboard != null)
                            storyboard.Begin(this);
                    }
                }
            };
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private async void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = (Storyboard)FindResource("WindowMinimizeAnimation");

            if (storyboard != null)
                storyboard.Begin(this);

            await Task.Delay(300);

            WindowState = WindowState.Minimized;
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = (Storyboard)FindResource("WindowMinimizeAnimation");

            if (storyboard != null)
                storyboard.Begin(this);

            await Task.Delay(300);

            Close();
        }
    }
}