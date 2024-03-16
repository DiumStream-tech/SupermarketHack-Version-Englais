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
                        _ = BeginStoryboard("WindowLoadAnimation", 0);
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
            await BeginStoryboard("WindowMinimizeAnimation");
            WindowState = WindowState.Minimized;
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            await BeginStoryboard("WindowMinimizeAnimation");
            Close();
        }

        private async Task BeginStoryboard(string name, int delay = 300)
        {
            Storyboard storyboard = (Storyboard)FindResource(name);

            if (storyboard != null)
                storyboard.Begin(this);

            if (delay > 0)
                await Task.Delay(delay);
        }
    }
}