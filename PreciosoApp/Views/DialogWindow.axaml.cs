using Avalonia.Controls;

namespace PreciosoApp.Views
{
    public partial class DialogWindow : Window
    {
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
    }
}
