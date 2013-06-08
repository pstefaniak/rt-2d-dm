using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Launcher
{
    public partial class PlayerSelection : UserControl
    {
        public PlayerSelection()
        {
            InitializeComponent();
        }

        public Color PlayerColor;
        public string Weapons;

        public void SetMouseCB(List<LauncherWindow.MouseData> Mouses)
        {
            cbMouse.Items.Clear();
            foreach (var Mouse in Mouses)
            {
                cbMouse.Items.Add(Mouse);
            }
        }
        
        public string BuildPlayerString()
        {
            StringBuilder Data = new StringBuilder();
            Data.Append("Player(");
            // Name
            Data.Append("name=").Append(tbName.Text).Append(";");

            // Color
            Data.Append(string.Format("color={0},{1},{2};", PlayerColor.R, PlayerColor.G, PlayerColor.B));

            // Controls
            Data.Append(string.Format("keyboard={0};mouse={1};", cbKeyboard.Text, ((LauncherWindow.MouseData)cbMouse.SelectedItem).Handle));

            Data.Append(string.Format("weapons={0};", Weapons));

            Data.Append(").");

            return Data.ToString();
        }

        public bool IsValid()
        {
            if (tbName.Text == "Unnamed")
                return false;

            if (cbMouse.SelectedIndex == -1)
                return false;

            if (cbKeyboard.SelectedIndex == -1)
                return false;

            if (PlayerColor == null)
                return false;

            return true;
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap Color = new Bitmap(20, 20);
                pbColor.Image = Color;

                PlayerColor = colorDialog1.Color;
                Graphics g = Graphics.FromImage(Color);
                g.Clear(colorDialog1.Color);
            }
        }

        private void bWpns_Click(object sender, EventArgs e)
        {
            Weapons = ((LauncherWindow)Parent).SelectWeapons();
        }
    }
}
