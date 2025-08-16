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
    /// Logique d'interaction pour ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }
        private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.ContextMenu == null)
                {
                    var contextMenu = new ContextMenu();
                    var copyMenuItem = new MenuItem { Header = "Copy Full Text" };
                    copyMenuItem.Click += (s, args) =>
                    {
                        Clipboard.SetText(textBox.Text);
                    };
                    contextMenu.Items.Add(copyMenuItem);
                    textBox.ContextMenu = contextMenu;
                }
            }
        }

    }
}
