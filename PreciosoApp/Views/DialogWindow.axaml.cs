using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace PreciosoApp.Views
{
    public partial class DialogWindow : Window
    {

        private Point _previousPosition;

        public DialogWindow()
        {
            InitializeComponent();

        }
        public string DialogText
        {
            get { return dialogTextBlock.Text; }
            set { dialogTextBlock.Text = value; }
        }
        public void SetDialogText(string text)
        {
            dialogTextBlock.Text = text;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
