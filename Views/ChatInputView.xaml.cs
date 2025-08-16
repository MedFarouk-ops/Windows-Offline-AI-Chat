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

namespace OfflineCodingBot.Views
{
    /// <summary>
    /// Logique d'interaction pour ChatInputView.xaml
    /// </summary>
    public partial class ChatInputView : UserControl
    {
        public ChatInputView()
        {
            InitializeComponent();
        }
        private void InputText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // If Shift is pressed, allow new line
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    // Let the new line be added, do nothing special
                }
                else
                {
                    // Prevent the newline character
                    e.Handled = true;

                    // Execute the send command bound in DataContext
                    var vm = DataContext as dynamic;
                    if (vm?.SendMessageCommand != null && vm.SendMessageCommand.CanExecute(null))
                    {
                        vm.SendMessageCommand.Execute(null);
                    }
                }
            }
        }


    }
}
