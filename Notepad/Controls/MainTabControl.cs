﻿using System.Linq;
using System.Windows.Forms;

namespace Notepad.Controls
{
    public class MainTabControl : TabControl
    {
        private const string NAME = "MainTabControl";
        private ContextMenuStrip _contextMenuStrip;
        private MainForm _form;
        public MainTabControl()
        {
            _contextMenuStrip = new TabControlContextMenuStrip();
            Name = NAME;
            ContextMenuStrip = _contextMenuStrip;
            Dock = DockStyle.Fill;
            HandleCreated += (s, e) =>
            {
                _form = FindForm() as MainForm;
            };

            SelectedIndexChanged += (s, e) =>
            {
                _form.CurrentFile = _form.Session.TextFiles[SelectedIndex];
                _form.CurrentRtb = (CustomRichTextBox)_form.MainTabControl.TabPages[SelectedIndex].Controls.Find("RtbtextFilecontent", true).First();
                _form.Text = $"{_form.CurrentFile.SafeFileName} - NotePad.NET";
            };
            MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < TabCount; i++)
                    {
                        var rect = GetTabRect(i);
                        if (rect.Contains(e.Location))
                        {
                            SelectedIndex = i;
                            break;
                        }
                    }
                }
            };
        }

    }
}
