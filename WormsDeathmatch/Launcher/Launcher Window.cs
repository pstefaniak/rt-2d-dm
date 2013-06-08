using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Launcher
{
    public partial class LauncherWindow : Form
    {
        public class MouseData
        {
            public IntPtr Handle;
            public int ID;
            public string Name;

            public override string ToString()
            {
                return ID.ToString() + ": " + Name;
            }
        }

        MultiDeviceInput.InputDevice InputDevice;

        public LauncherWindow()
        {
            InitializeComponent();
            InputDevice = new MultiDeviceInput.InputDevice(Handle);
            InputDevice.MouseDown += new MultiDeviceInput.InputDevice.MouseEventHandler(MouseClicked);
            RefreshMices_Click(null, null);
            playerSelection1.SetMouseCB(Mouses);
            playerSelection2.SetMouseCB(Mouses);
            playerSelection3.SetMouseCB(Mouses);
            playerSelection4.SetMouseCB(Mouses);

            cxbLimitedWeapons.Checked = false;

            // Load level data
            foreach (DirectoryInfo LevelDir in new DirectoryInfo("Data").GetDirectories())
            {
                if (LevelDir.GetFiles("config.ini").Length > 0)
                {
                    lbLevels.Items.Add(LevelDir.Name);
                }
            }
        }

        public void MouseClicked(object Data, MultiDeviceInput.InputDevice.MouseControlEventArgs e)
        {
            MouseData CurrentMouse = Mouses.Find(n => n.Handle == e.DeviceData.Info.deviceHandle);
            if (CurrentMouse != null)
            {
                tbLastMouseName.Text = CurrentMouse.ToString();
            }
            else tbLastMouseName.Text = "Unknown";
        }

        public List<MouseData> Mouses = new List<MouseData>();

        private void RefreshMices_Click(object sender, EventArgs e)
        {
            lbMice.Items.Clear();
            Mouses.Clear();

            int ID = 1;
            int DeviceCount = InputDevice.EnumerateDevices();
            foreach (var KvP in InputDevice.DeviceList)
            {
                if (KvP.Value.Info.deviceType != "MOUSE") continue;

                MouseData NewMouse = new MouseData();
                NewMouse.ID = ID++;
                NewMouse.Handle = KvP.Value.Info.deviceHandle;
                NewMouse.Name = KvP.Value.Info.Name.Substring(KvP.Value.Info.Name.IndexOf(';') + 1);
                Mouses.Add(NewMouse);
                lbMice.Items.Add(NewMouse);
            }

        }


        protected override void WndProc(ref Message m)
        {
            if(InputDevice != null)
                InputDevice.ProcessMessage(m);

            base.WndProc(ref m);
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            string GameParams = "";
            bool GotPlayer = false;

            if (lbLevels.SelectedIndex == -1)
            {
                MessageBox.Show("Select level!");
                return;
            }
            GameParams += " -level=" + lbLevels.Text + " ";

            if (cxbConsole.Checked)
            {
                GameParams += "-console ";
            }

            if (!cxbLimitedWeapons.Checked)
            {
                GameParams += "-unlock_all_weapons ";
            }

            GameParams += "\"-players=";
            if (playerSelection1.IsValid())
            {
                GameParams += playerSelection1.BuildPlayerString();
                GotPlayer = true;
            }
            if (playerSelection2.IsValid())
            {
                GameParams += playerSelection2.BuildPlayerString();
                GotPlayer = true;
            }
            if (playerSelection3.IsValid())
            {
                GameParams += playerSelection3.BuildPlayerString();
                GotPlayer = true;
            }
            if (playerSelection4.IsValid())
            {
                GameParams += playerSelection4.BuildPlayerString();
                GotPlayer = true;
            }
            GameParams += "\"";

            if (!GotPlayer)
            {
                MessageBox.Show("Setup at least one player!");
                return;
            }

            Close();
            ProcessStartInfo start = new ProcessStartInfo();
            using (var Fs = new StreamWriter( File.Create("log.txt")))
            {
                Fs.Write(GameParams);
            }
            start.Arguments = GameParams;
            start.FileName = new DirectoryInfo(Application.StartupPath).GetFiles("WormsDeathmatch.exe")[0].FullName;
            Process proc = Process.Start(start);
        }

        private void lbLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbLevels.SelectedIndex != -1)
            {
                clbWeapons.Items.Clear();
                foreach (FileInfo WeaponFile in new DirectoryInfo(Path.Combine(new string[] {"Data", lbLevels.Text, "Weapons"})).GetFiles("*.ini"))
                {
                    clbWeapons.Items.Add(WeaponFile.Name.Substring(0, WeaponFile.Name.IndexOf('.')));   
                }
                for (int i = 0; i < clbWeapons.Items.Count; i++)
                {
                    clbWeapons.SetItemChecked(i, true);
                }
            }
        }

        public string SelectWeapons()
        {
            WeaponSelectionDialog Dialog = new WeaponSelectionDialog();
            if (Dialog.ShowDialog() == DialogResult.OK)
            {

            }
            return "";
        }

        private void cxbLimitedWeapons_CheckedChanged(object sender, EventArgs e)
        {
            nudWeaponsLimit.Enabled = cxbLimitedWeapons.Checked;
        }

        private void LauncherWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
