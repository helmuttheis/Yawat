namespace Yawat.Wpf
{
    using Xamarin.Forms;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Forms.Init();
            this.LoadApplication(new Yawat.Xf.App());
        }
    }
}
