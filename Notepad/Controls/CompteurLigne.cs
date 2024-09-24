using System.Drawing;
using System.Windows.Forms;

namespace Notepad.Controls
{
    public class CompteurLigne : Panel
    {
        private const string NAME = "ligneCompteur";


        public CompteurLigne()
        {
            Name = NAME;
            Dock = DockStyle.Left;
            Width = 40;
            BackColor = Color.Gray;



        }
    }
}
