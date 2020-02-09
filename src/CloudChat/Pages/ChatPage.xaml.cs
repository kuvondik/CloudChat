using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static CloudChat.DI;

namespace CloudChat

{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<ChatMessageListViewModel>
    {
        public ChatPage(ChatMessageListViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        public ChatPage() : base()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            if (ChatMessageList == null)
                return;
            var storyboard = new Storyboard();

            storyboard.AddFadeIn(0.3f, true);
            storyboard.Begin(ChatMessageList);
            MessageText.Focus();

        }

        private void MessageText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    var index = textBox.CaretIndex;
                    textBox.Text = textBox.Text.Insert(index, Environment.NewLine);
                    textBox.CaretIndex = index + Environment.NewLine.Length;
                    e.Handled = true;
                }
                else
                    ViewModel.Send(ChatMessageList);
                e.Handled = true;
            }
        }

        private void MessageText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MessageText.Text.Length > 0)
                IsBusyProperty.SetValue(sendButton, true);
            else
                IsBusyProperty.SetValue(sendButton, false);
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                HandleFileOpen(files[0]);
            }
        }

        private void HandleFileOpen(string file)
        {
            UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "Send a file",
                OkText = "Send",
                Message = file,
                PicturePath = file,
            });
            return;
        }
    }
}