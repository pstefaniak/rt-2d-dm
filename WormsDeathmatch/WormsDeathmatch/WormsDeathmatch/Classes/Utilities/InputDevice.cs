// ShadowDancer 17.02.2013
// Distributed under beerware license
// Based on "Using Raw Input from C# to handle multiple keyboards" Article from CodeProject

// Uncomment it to drop info about unknown devices and keys to console
//#define _ID_DEBUG_OUTPUT_TO_CONSOLE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Permissions;


namespace MultiDeviceInput
{
    public sealed class InputDevice
    {
        #region const definitions

        private const int RIDEV_INPUTSINK = 0x00000100;
        private const int RID_INPUT = 0x10000003;

        private const int FAPPCOMMAND_MASK = 0xF000;
        private const int FAPPCOMMAND_MOUSE = 0x8000;
        private const int FAPPCOMMAND_OEM = 0x1000;

        private const int RIM_TYPEMOUSE = 0;
        private const int RIM_TYPEKEYBOARD = 1;
        private const int RIM_TYPEHID = 2;

        private const int RIDI_DEVICENAME = 0x20000007;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_INPUT = 0x00FF;
        private const int VK_OEM_CLEAR = 0xFE;
        private const int VK_LAST_KEY = VK_OEM_CLEAR; // this is a made up value used as a sentinal

        private const int MOUSE_ATTRIBUTES_CHANGED = 0x04;
        private const int MOUSE_MOVE_RELATIVE = 0;
        private const int MOUSE_MOVE_ABSOLUTE = 1;
        private const int MOUSE_VIRTUAL_DESKTOP = 0x02;


        private const int RI_MOUSE_LEFT_BUTTON_DOWN = 0x0001;
        private const int RI_MOUSE_LEFT_BUTTON_UP = 0x0002;
        private const int RI_MOUSE_MIDDLE_BUTTON_DOWN = 0x0010;
        private const int RI_MOUSE_MIDDLE_BUTTON_UP = 0x0020;
        private const int RI_MOUSE_RIGHT_BUTTON_DOWN = 0x0004;
        private const int RI_MOUSE_RIGHT_BUTTON_UP = 0x0008;

        private const int RI_MOUSE_WHEEL = 0x0400;
        #endregion const definitions

        #region structs & enums

        public class DeviceData
        {
            public DeviceInfo Info;
            public DeviceType Type;

            /// <summary>
            /// Contains mouse position relative to point where started captuiring
            /// Please note that this value WON'T be bounded to screen edges or somethign
            /// </summary>
            public int MouseX { get; set; }

            /// <summary>
            /// Contains mouse position relative to point where started captuiring
            /// Please note that this value WON'T be bounded to screen edges or somethign
            /// </summary>
            public int MouseY { get; set; }

            public bool MouseLeftPressed { get; set; }
            public bool MouseMiddlePressed { get; set; }
            public bool MouseRightPressed { get; set; }

            public int MouseWheel { get; set; }

            /// <summary>
            /// If device is keyboard array contains key states
            /// </summary>
            public bool[] KeyboardKeyPressed { get; set; }
        }

        public enum DeviceType
        {
            Keyboard,
            Mouse,
            OEM
        }

        #region Windows.h structure declarations
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICELIST
        {
            public IntPtr hDevice;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }

#if VISTA_64
        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;
            [FieldOffset(24)]
            public RAWMOUSE mouse;
            [FieldOffset(24)]
            public RAWKEYBOARD keyboard;
            [FieldOffset(24)]
            public RAWHID hid;
        }
 
        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            public IntPtr wParam;
        }
#else
        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;
            [FieldOffset(16)]
            public RAWMOUSE mouse;
            [FieldOffset(16)]
            public RAWKEYBOARD keyboard;
            [FieldOffset(16)]
            public RAWHID hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            [MarshalAs(UnmanagedType.U4)]
            public int wParam;
        }
#endif

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWHID
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwSizHid;
            [MarshalAs(UnmanagedType.U4)]
            public int dwCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BUTTONSSTR
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonFlags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonData;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWMOUSE
        {
            [MarshalAs(UnmanagedType.U2)]
            [FieldOffset(0)]
            public ushort usFlags;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public uint ulButtons;
            [FieldOffset(4)]
            public BUTTONSSTR buttonsStr;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(8)]
            public uint ulRawButtons;
            [FieldOffset(12)]
            public int lLastX;
            [FieldOffset(16)]
            public int lLastY;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(20)]
            public uint ulExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWKEYBOARD
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort MakeCode;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Flags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public ushort VKey;
            [MarshalAs(UnmanagedType.U4)]
            public uint Message;
            [MarshalAs(UnmanagedType.U4)]
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICE
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsage;
            [MarshalAs(UnmanagedType.U4)]
            public int dwFlags;
            public IntPtr hwndTarget;
        }
        #endregion Windows.h structure declarations

        /// <summary>
        /// Class encapsulating the information about a
        /// keyboard event, including the device it
        /// originated with and what key was pressed
        /// </summary>
        public class DeviceInfo
        {
            /// <summary>
            /// For instance \\??\\ACPI#PNP0303#3&13c0b0c5&0#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}
            /// \\??\\(Class code) # (SubClass code) # (Protocol code) # GUID
            /// </summary>
            public string deviceName;
            /// <summary>
            /// The type of raw input the message represents. The values can be RIM_TYPEHID (2), RIM_TYPEKEYBOARD (1), or RIM_TYPEMOUSE (0). 
            /// </summary>
            public string deviceType;
            public IntPtr deviceHandle;
            /// <summary>
            /// Device info from registry
            /// </summary>
            public string Name;
            public string source;
        }

        #endregion structs & enums

        #region DllImports

        [DllImport("User32.dll")]
        extern static uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        [DllImport("User32.dll")]
        extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        #endregion DllImports

        #region Variables and event handling

        /// <summary>
        /// List of devices
        /// Key: the device handle
        /// Value: the device info class
        /// </summary>
        public Dictionary<IntPtr, DeviceData> DeviceList = new Dictionary<IntPtr, DeviceData>();



        //Event and delegate
        public delegate void KeyboardEventHandler(object sender, KeyControlEventArgs e);
        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;

        public delegate void MouseEventHandler(object sender, MouseControlEventArgs e);
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;

        /// <summary>
        /// Arguments provided by the handler for the keyboard event.
        /// </summary>
        public class KeyControlEventArgs : EventArgs
        {
            public DeviceData DeviceData;

            public int vKey;
            public Keys Key;

            public KeyControlEventArgs(DeviceData dData, int VirtualKey, Keys WinFormKey)
            {
                DeviceData = dData;
                vKey = VirtualKey;
                Key = WinFormKey;
            }

            public KeyControlEventArgs()
            {
            }

        }

        /// <summary>
        /// Arguments provided by the handler for the mouse event.
        /// </summary>
        public class MouseControlEventArgs : EventArgs
        {
            public DeviceData DeviceData;

            public int DeltaX = 0;
            public int DeltaY = 0;

            public bool LeftButton = false;
            public bool MiddleButton = false;
            public bool RightButton = false;

            /// <summary>
            /// Positive value = away from user
            /// Negative value = towards user
            /// 1 - 1 mouse wheel tick
            /// </summary>
            public int Wheel = 0;

            public MouseControlEventArgs(DeviceData dData, int WheelChange)
            {
                Wheel = WheelChange;
                DeviceData = dData;
            }

            public MouseControlEventArgs(DeviceData dData, int ChangeX, int ChangeY)
            {
                DeltaX = ChangeX;
                DeltaY = ChangeY;
                DeviceData = dData;
            }

            public MouseControlEventArgs(DeviceData dData, bool Left, bool Middle, bool Right)
            {
                LeftButton = Left;
                RightButton = Right;
                MiddleButton = Middle;
                DeviceData = dData;
            }

            public MouseControlEventArgs(DeviceData dData)
            {
                DeviceData = dData;
            }
        }

        #endregion Variables and event handling

        #region InputDevice( IntPtr hwnd )

        /// <summary>
        /// InputDevice constructor; registers the raw input devices
        /// for the calling window.
        /// </summary>
        /// <param name="hwnd">Handle of the window listening for key presses</param>
        public InputDevice(IntPtr hwnd)
        {
            //Create an array of all the raw input devices we want to 
            //listen to. In this case, only keyboard devices.
            //RIDEV_INPUTSINK determines that the window will continue
            //to receive messages even when it doesn't have the focus.
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[2];

            rid[0].usUsagePage = 0x01;
            rid[0].usUsage = 0x06;
            rid[0].dwFlags = RIDEV_INPUTSINK;
            rid[0].hwndTarget = hwnd;

            rid[1].usUsagePage = 0x01;
            rid[1].usUsage = 0x02;
            rid[1].dwFlags = RIDEV_INPUTSINK;
            rid[1].hwndTarget = hwnd;

            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
            {
                throw new ApplicationException("Failed to register raw input device(s).");
            }
            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[1])))
            {
                throw new ApplicationException("Failed to register raw input device(s).");
            }
        }

        #endregion InputDevice( IntPtr hwnd )

        #region ReadReg( string item, ref bool isKeyboard )

        /// <summary>
        /// Reads the Registry to retrieve a friendly description
        /// of the device, and whether it is a keyboard.
        /// </summary>
        /// <param name="item">The device name to search for, as provided by GetRawInputDeviceInfo.</param>
        /// <param name="isKeyboard">Determines whether the device's class is "Keyboard". By reference.</param>
        /// <returns>The device description stored in the Registry entry's DeviceDesc value.</returns>
        private static string ReadReg(string item)
        {
            // Example Device Identification string
            // @"\??\ACPI#PNP0303#3&13c0b0c5&0#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}";

            // remove the \??\
            item = item.Substring(4);

            string[] split = item.Split('#');

            string id_01 = split[0];    // ACPI (Class code)
            string id_02 = split[1];    // PNP0303 (SubClass code)
            string id_03 = split[2];    // 3&13c0b0c5&0 (Protocol code)
            //The final part is the class GUID and is not needed here

            //Open the appropriate key as read-only so no permissions
            //are needed.
            RegistryKey OurKey = Registry.LocalMachine;

            string findme = string.Format(@"System\CurrentControlSet\Enum\{0}\{1}\{2}", id_01, id_02, id_03);

            OurKey = OurKey.OpenSubKey(findme, false);

            //Retrieve the desired information and set isKeyboard
            string deviceDesc = (string)OurKey.GetValue("DeviceDesc");
            string deviceClass = (string)OurKey.GetValue("Class");

            return deviceDesc;
        }

        #endregion ReadReg( string item, ref bool isKeyboard )

        #region int EnumerateDevices()

        /// <summary>
        /// Iterates through the list provided by GetRawInputDeviceList,
        /// counting keyboard devices and adding them to deviceList.
        /// </summary>
        /// <returns>The number of keyboard devices found.</returns>
        public int EnumerateDevices()
        {

            int NumberOfDevices = 0;
            uint deviceCount = 0;
            int dwSize = (Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));

            if (GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
            {
                IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

                for (int i = 0; i < deviceCount; i++)
                {
                    uint pcbSize = 0;

                    RAWINPUTDEVICELIST rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(
                                               new IntPtr((pRawInputDeviceList.ToInt32() + (dwSize * i))),
                                               typeof(RAWINPUTDEVICELIST));

                    GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                    if (pcbSize > 0)
                    {
                        IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                        GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                        string deviceName = Marshal.PtrToStringAnsi(pData);

                        //The list will include the "root" keyboard and mouse devices
                        //which appear to be the remote access devices used by Terminal
                        //Services or the Remote Desktop - we're not interested in these
                        //so the following code with drop into the next loop iteration
                        if (deviceName.ToUpper().Contains("ROOT"))
                        {
                            continue;
                        }

                        //If the device is identified as a keyboard or HID device,
                        //create a DeviceInfo object to store information about it
                        if (rid.dwType == RIM_TYPEKEYBOARD || rid.dwType == RIM_TYPEHID || rid.dwType == RIM_TYPEMOUSE)
                        {
                            DeviceData Data = new DeviceData();
                            DeviceInfo dInfo = new DeviceInfo();
                            Data.Info = dInfo;
                            Data.Type = GetDeviceTypeE(rid.dwType);
                            if (Data.Type == DeviceType.Keyboard)
                            {
                                Data.KeyboardKeyPressed = new bool[255];
                            }

                            dInfo.deviceName = Marshal.PtrToStringAnsi(pData);
                            dInfo.deviceHandle = rid.hDevice;
                            dInfo.deviceType = GetDeviceType(rid.dwType);


                            string DeviceDesc = ReadReg(deviceName);
                            dInfo.Name = DeviceDesc;

                            //If it is a keyboard and it isn't already in the list,
                            //add it to the deviceList hashtable and increase the
                            //NumberOfDevices count
                            if (!DeviceList.ContainsKey(rid.hDevice))
                            {
                                NumberOfDevices++;
                                DeviceList.Add(rid.hDevice, Data);
                            }
                        }
                        Marshal.FreeHGlobal(pData);
                    }
                }

                Marshal.FreeHGlobal(pRawInputDeviceList);
                return NumberOfDevices;
            }
            else
            {
                throw new ApplicationException("Error!\nGetRawInputDeviceList failed.");
            }
        }

        #endregion EnumerateDevices()

        #region ProcessInputCommand( Message message )

        /// <summary>
        /// Processes WM_INPUT messages to retrieve information about any
        /// keyboard events that occur.
        /// </summary>
        /// <param name="message">The WM_INPUT message to process.</param>
        public void ProcessInputCommand(Message message)
        {
            uint dwSize = 0;

            // First call to GetRawInputData sets the value of dwSize
            // dwSize can then be used to allocate the appropriate amount of memore,
            // storing the pointer in "buffer".
            GetRawInputData(message.LParam,
                             RID_INPUT, IntPtr.Zero,
                             ref dwSize,
                             (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
            try
            {
                // Check that buffer points to something, and if so,
                // call GetRawInputData again to fill the allocated memory
                // with information about the input
                if (buffer != IntPtr.Zero &&
                    GetRawInputData(message.LParam,
                                     RID_INPUT,
                                     buffer,
                                     ref dwSize,
                                     (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == dwSize)
                {
                    // Store the message information in "raw", then check
                    // that the input comes from a keyboard device before
                    // processing it to raise an appropriate KeyPressed event.

                    RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));

                    if (raw.header.dwType == RIM_TYPEMOUSE)
                    {
                        if (DeviceList.ContainsKey(raw.header.hDevice))
                        {

                            DeviceData dData = DeviceList[raw.header.hDevice];

                            if (raw.mouse.usFlags == MOUSE_MOVE_RELATIVE)
                            {
                                if (raw.mouse.lLastX != 0 && raw.mouse.lLastY != 0)
                                {
                                    dData.MouseX += raw.mouse.lLastX;
                                    dData.MouseY += raw.mouse.lLastY;
                                    if(MouseMove != null)
                                        MouseMove(this, new MouseControlEventArgs(dData, raw.mouse.lLastX, raw.mouse.lLastY));
                                }
                            }
                            else if (raw.mouse.usFlags == MOUSE_MOVE_ABSOLUTE)
                            {
                                if (raw.mouse.lLastX != dData.MouseX && raw.mouse.lLastY != dData.MouseY)
                                {
                                    dData.MouseX = raw.mouse.lLastX;
                                    dData.MouseY = raw.mouse.lLastY;
                                    if(MouseMove != null)
                                        MouseMove(this, new MouseControlEventArgs(dData));
                                }
                            }

                            // Handle mouse buttons
                            switch (raw.mouse.buttonsStr.usButtonFlags)
                            {
                                case RI_MOUSE_LEFT_BUTTON_DOWN:
                                    dData.MouseLeftPressed = true;
                                    if (MouseDown != null) MouseDown(this, new MouseControlEventArgs(dData, true, false, false));
                                    break;
                                case RI_MOUSE_MIDDLE_BUTTON_DOWN:
                                    dData.MouseMiddlePressed = true;
                                    if (MouseDown != null) MouseDown(this, new MouseControlEventArgs(dData, false, true, false));
                                    break;
                                case RI_MOUSE_RIGHT_BUTTON_DOWN:
                                    dData.MouseRightPressed = true;
                                    if (MouseDown != null) MouseDown(this, new MouseControlEventArgs(dData, false, false, true));
                                    break;
                                case RI_MOUSE_LEFT_BUTTON_UP:
                                    dData.MouseLeftPressed = false;
                                    if (MouseUp != null) MouseUp(this, new MouseControlEventArgs(dData, true, false, false));
                                    break;
                                case RI_MOUSE_MIDDLE_BUTTON_UP:
                                    dData.MouseMiddlePressed = false;
                                    if (MouseUp != null) MouseUp(this, new MouseControlEventArgs(dData, false, true, false));
                                    break;
                                case RI_MOUSE_RIGHT_BUTTON_UP:
                                    dData.MouseRightPressed = false;
                                    if (MouseUp != null) MouseUp(this, new MouseControlEventArgs(dData, false, false, true));
                                    break;
                                case RI_MOUSE_WHEEL:
                                    {
                                        short WheelTicks =  (short)raw.mouse.buttonsStr.usButtonData;
                                        WheelTicks /= 120;

                                        dData.MouseWheel += WheelTicks;
                                        if (MouseWheel != null) MouseWheel(this, new MouseControlEventArgs(dData, WheelTicks));
                                    }
                                    break;
                            }
                        }
                        else
                        {
#if _ID_DEBUG_OUTPUT_TO_CONSOLE
                            Console.WriteLine("Handle :{0} is unknown", raw.header.hDevice);
                            Console.WriteLine("Maybe this device supports more than one handle or usage page.");
                            Console.WriteLine("This is probably not a standard mouse.");
#endif
                        }
                    }

                    if (raw.header.dwType == RIM_TYPEKEYBOARD)
                    {

                        ushort key = raw.keyboard.VKey;

                        // On most keyboards, "extended" keys such as the arrow or page 
                        // keys return two codes - the key's own code, and an "extended key" flag, which
                        // translates to 255. This flag isn't useful to us, so it can be
                        // disregarded.
                        if (key > VK_LAST_KEY)
                        {
                            return;
                        }

                        int vkey = raw.keyboard.VKey;
                        Keys myKey = (Keys)Enum.Parse(typeof(Keys), Enum.GetName(typeof(Keys), vkey));

                        if (raw.keyboard.Message == WM_KEYDOWN || raw.keyboard.Message == WM_SYSKEYDOWN)
                        {
                            if (DeviceList.ContainsKey(raw.header.hDevice))
                            {
                                DeviceData Data = DeviceList[raw.header.hDevice];
                                Data.KeyboardKeyPressed[vkey] = true;

                                if (KeyDown != null)
                                {
                                    KeyDown(this, new KeyControlEventArgs(Data, vkey, myKey));
                                }
                                else
                                {
#if _ID_DEBUG_OUTPUT_TO_CONSOLE
                                    Console.WriteLine("Received Unknown Key: {0}", key);
                                    Console.WriteLine("Possibly an Unknown device");
#endif
                                }
                            }
                            else
                            {
#if _ID_DEBUG_OUTPUT_TO_CONSOLE
                                Console.WriteLine("Handle :{0} is unknown", raw.header.hDevice);
                                Console.WriteLine("Maybe this device supports more than one handle or usage page.");
                                Console.WriteLine("This is probably not a standard keyboard.");
#endif
                            }
                        }
                        else if (raw.keyboard.Message == WM_KEYUP || raw.keyboard.Message == WM_SYSKEYUP)
                        {
                            if (DeviceList.ContainsKey(raw.header.hDevice))
                            {
                                DeviceData Data = DeviceList[raw.header.hDevice];
                                Data.KeyboardKeyPressed[vkey] = false;

                                if (KeyUp != null)
                                {
                                    KeyUp(this, new KeyControlEventArgs(Data, vkey, myKey));
                                }
                                else
                                {
#if _ID_DEBUG_OUTPUT_TO_CONSOLE
                                    Console.WriteLine("Received Unknown Key: {0}", key);
                                    Console.WriteLine("Possibly an Unknown device");
#endif
                                }
                            }
                            else
                            {
#if _ID_DEBUG_OUTPUT_TO_CONSOLE
                                Console.WriteLine("Handle :{0} is unknown", raw.header.hDevice);
                                Console.WriteLine("Maybe this device supports more than one handle or usage page.");
                                Console.WriteLine("This is probably not a standard keyboard.");
#endif
                            }

                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        #endregion ProcessInputCommand( Message message )

        #region DeviceType GetDevice( int param )

        /// <summary>
        /// Determines what type of device triggered a WM_INPUT message.
        /// (Used in the ProcessInputCommand).
        /// </summary>
        /// <param name="param">The LParam from a WM_INPUT message.</param>
        /// <returns>A DeviceType enum value.</returns>
        private static DeviceType GetDevice(int param)
        {
            DeviceType deviceType;

            switch ((((ushort)(param >> 16)) & FAPPCOMMAND_MASK))
            {
                case FAPPCOMMAND_OEM:
                    deviceType = DeviceType.OEM;
                    break;
                case FAPPCOMMAND_MOUSE:
                    deviceType = DeviceType.Mouse;
                    break;
                default:
                    deviceType = DeviceType.Keyboard;
                    break;
            }
            return deviceType;
        }

        #endregion DeviceType GetDevice( int param )

        #region ProcessMessage(Message message)

        /// <summary>
        /// Filters Windows messages for WM_INPUT messages and calls
        /// ProcessInputCommand if necessary.
        /// </summary>
        /// <param name="message">The Windows message.</param>
        public void ProcessMessage(Message message)
        {
            switch (message.Msg)
            {
                case WM_INPUT:
                    {
                        ProcessInputCommand(message);
                    }
                    break;
            }
        }

        #endregion ProcessMessage( Message message )

        #region GetDeviceType( int device )

        /// <summary>
        /// Converts a RAWINPUTDEVICELIST dwType value to a string
        /// describing the device type.
        /// </summary>
        /// <param name="device">A dwType value (RIM_TYPEMOUSE, 
        /// RIM_TYPEKEYBOARD or RIM_TYPEHID).</param>
        /// <returns>A string representation of the input value.</returns>
        private static string GetDeviceType(int device)
        {
            string deviceType;
            switch (device)
            {
                case RIM_TYPEMOUSE: deviceType = "MOUSE"; break;
                case RIM_TYPEKEYBOARD: deviceType = "KEYBOARD"; break;
                case RIM_TYPEHID: deviceType = "HID"; break;
                default: deviceType = "UNKNOWN"; break;
            }
            return deviceType;
        }

        /// <summary>
        /// Converts a RAWINPUTDEVICELIST dwType value to a string
        /// describing the device type.
        /// </summary>
        /// <param name="device">A dwType value (RIM_TYPEMOUSE, 
        /// RIM_TYPEKEYBOARD or RIM_TYPEHID).</param>
        /// <returns>A string representation of the input value.</returns>
        private static DeviceType GetDeviceTypeE(int device)
        {
            switch (device)
            {
                case RIM_TYPEMOUSE: return DeviceType.Mouse;
                case RIM_TYPEKEYBOARD: return DeviceType.Keyboard;
                default: return DeviceType.OEM;
            }
        }
        #endregion GetDeviceType( int device )

    }
}