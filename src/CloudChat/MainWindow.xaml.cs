using System;
using System.Windows;
using System.Windows.Input;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
        }

        private void AppWindow_Deactivated(object sender, EventArgs e)
        {
            (DataContext as WindowViewModel).DimmableOverlayForAllVisible = true;
        }

        private void AppWindow_Activated(object sender, EventArgs e)
        {
            (DataContext as WindowViewModel).DimmableOverlayForAllVisible = false;
        }

        private void AppWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DI.ChatList.UnselectItem();
            }
        }

        private void AppWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.XButton1)
            {
                DI.ChatList.UnselectItem();
            }
        }


        //protected override void OnActivated(EventArgs e)
        //{
        //    base.OnActivated(e);
        //    if (WindowStyle != WindowStyle.None)
        //    {
        //        Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (DispatcherOperationCallback)delegate(object unused)
        //            {
        //                WindowStyle = WindowStyle.None;
        //                return null;
        //            },null);
        //    }
        //}
    }
}
