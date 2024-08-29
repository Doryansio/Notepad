using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
