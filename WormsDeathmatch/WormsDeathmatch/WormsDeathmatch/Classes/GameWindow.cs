using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiDeviceInput;

namespace WormsDeathmatch
{
    /// <summary>
    /// Window grab input and send it to player manager to process
    /// </summary>
    public partial class GameWindow : Form
    {

        public InputDevice InputDevice;
        public static GameWindow Instance;

        public delegate void MouseEventHandler(object sender, MouseData Mouse);
        public event MouseEventHandler MouseEventButtonDownLeft;
        public event MouseEventHandler MouseEventButtonUpLeft;
        public event MouseEventHandler MouseEventButtonDownRight;
        public event MouseEventHandler MouseEventButtonUpRight;
        public event MouseEventHandler MouseEventWheelUp;
        public event MouseEventHandler MouseEventWheelDown;
        public GameWindow()
        {
            InitializeComponent();
            Instance = this;
            Cursor.Hide();

            // Multi mouse input init
            InputDevice = new MultiDeviceInput.InputDevice(Handle);
            InputDevice.EnumerateDevices();
            InputDevice.MouseDown += new MultiDeviceInput.InputDevice.MouseEventHandler(_MouseDown);
            InputDevice.MouseMove += new MultiDeviceInput.InputDevice.MouseEventHandler(_MouseMove);
            InputDevice.MouseUp += new MultiDeviceInput.InputDevice.MouseEventHandler(_MouseUp);
            InputDevice.MouseWheel += new MultiDeviceInput.InputDevice.MouseEventHandler(_MouseWheel);

            foreach (var KvP in InputDevice.DeviceList)
            {
                MouseData NewMouse = new MouseData();
                NewMouse.PosX = Canvas.Width / 2;
                NewMouse.PosY = Canvas.Height / 2;
                NewMouse.Data = KvP.Value;
                Mouses.Add(KvP.Value.Info.deviceHandle, NewMouse);
            }
        }

        public class MouseData
        {
            public int PosX;
            public int PosY;
            public InputDevice.DeviceData Data;
        }
        public Dictionary<IntPtr, MouseData> Mouses = new Dictionary<IntPtr, MouseData>();

        private void _MouseMove(object sender, InputDevice.MouseControlEventArgs e)
        {
            if(Focused)Cursor.Position = new Point(Location.X + Width / 2, Location.Y + Height / 2);

            if (Mouses.ContainsKey(e.DeviceData.Info.deviceHandle))
            {
                // convert to absolute mouse cooridnates
                MouseData Current = Mouses[e.DeviceData.Info.deviceHandle];
                Current.PosX += e.DeltaX;
                Current.PosY += e.DeltaY;

                if (Current.PosX < 0) Current.PosX = 0;
                if (Current.PosY < 0) Current.PosY = 0;
                if (Current.PosX >= Canvas.Width) Current.PosX = Canvas.Width-1;
                if (Current.PosY >= Canvas.Height) Current.PosY = Canvas.Height - 1;
            }
        }
        private void _MouseDown(object sender, InputDevice.MouseControlEventArgs e)
        {
            if (e.LeftButton)
            {
                MouseEventButtonDownLeft(this, Mouses[e.DeviceData.Info.deviceHandle]);
            }
            else if (e.RightButton)
            {
                MouseEventButtonDownRight(this, Mouses[e.DeviceData.Info.deviceHandle]);
            }
        }
        private void _MouseUp(object sender, InputDevice.MouseControlEventArgs e)
        {
            if (!Mouses.ContainsKey(e.DeviceData.Info.deviceHandle))
            {
                MouseData NewMouse = new MouseData();
                NewMouse.PosX = Canvas.Width / 2;
                NewMouse.PosY = Canvas.Height / 2;
                NewMouse.Data = e.DeviceData;
                Mouses.Add(e.DeviceData.Info.deviceHandle, NewMouse);
            }

            if (Mouses.ContainsKey(e.DeviceData.Info.deviceHandle) && e.LeftButton)
            {
                MouseEventButtonUpLeft(this, Mouses[e.DeviceData.Info.deviceHandle]);
            }
            else if (e.RightButton)
            {
                MouseEventButtonUpRight(this, Mouses[e.DeviceData.Info.deviceHandle]);
            }
        }

        private void _MouseWheel(object sender, InputDevice.MouseControlEventArgs e)
        {
            if (Mouses.ContainsKey(e.DeviceData.Info.deviceHandle))
            {
                if (e.Wheel > 0)
                {
                    MouseEventWheelUp(this, Mouses[e.DeviceData.Info.deviceHandle]);
                }
                else if (e.Wheel < 0)
                {
                    MouseEventWheelDown(this, Mouses[e.DeviceData.Info.deviceHandle]);
                }

            }
        }


        public IntPtr CanvasHandle
        {
            get { return Canvas.Handle; }
        }

        public Size ViewportSize
        {
            get { return Canvas.Size; }
        }

        private void GameWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        protected override void WndProc(ref Message m)
        {
            if(InputDevice != null)
                InputDevice.ProcessMessage(m);

            base.WndProc(ref m);
        }
        private void GameWindow_Load(object sender, EventArgs e)
        {
            ClientSize = Size;
        }
    }
}
