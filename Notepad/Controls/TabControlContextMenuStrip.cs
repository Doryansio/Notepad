using System.Windows.Forms;

namespace Notepad.Controls
{
    public class TabControlContextMenuStrip : ContextMenuStrip
    {
        private const string NAME = "TabControlContextMenuStrip";
        public TabControlContextMenuStrip()
        {
            Name = NAME;

            var closeTab = new ToolStripMenuItem("fermer");
            var closeAllTabsExceptThis = new ToolStripMenuItem("fermer tout sauf ce fichier");
            var OpenFileInExplorer = new ToolStripMenuItem("Ouvrir le repertoire du fichier en cours dans l'explorateur");


            Items.AddRange(new ToolStripMenuItem[] { closeTab, closeAllTabsExceptThis, OpenFileInExplorer });
        }
    }
}
