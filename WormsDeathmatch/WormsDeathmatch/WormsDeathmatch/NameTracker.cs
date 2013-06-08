using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

public class Hooker
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern uint RegisterWindowMessage(string lpString);

    private IntPtr _hWnd; // APS-50 class reference
    private List<IntPtr> _windowsMessages = new List<IntPtr>(); // APS-50 messages

    private const string _className = "www.AuPix.com/SHOCK/MessageWindowClass";

    // Windows Messages events
    private const string _messageBroadcast = "www.AuPix.com/SHOCK/BROADCAST";
    private const string _messageCallEvents = "www.AuPix.com/SHOCK/CallEvents";
    private const string _messageRegistrationEvents = "www.AuPix.com/SHOCK/RegistrationEvents";
    private const string _messageActions = "www.AuPix.com/SHOCK/Actions";

    private void DemoProblem()
    {
        // Find hidden window handle
        _hWnd = FindWindow(_className, null);

        // Register for events
        _windowsMessages.Add(new IntPtr(RegisterWindowMessage(_messageActions)));
        _windowsMessages.Add(new IntPtr(RegisterWindowMessage(_messageBroadcast)));
        _windowsMessages.Add(new IntPtr(RegisterWindowMessage(_messageCallEvents)));
        _windowsMessages.Add(new IntPtr(RegisterWindowMessage(_messageRegistrationEvents)));
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        // Are they registered Windows Messages for the APS-50 application?
        foreach (IntPtr message in _windowsMessages)
        {
            if ((IntPtr)m.Msg == message)
            {
                System.Windows.Forms.MessageBox.Show("Message from specified application found!");
            }
        }

        // Are they coming from the APS-50 application?
        if (m.HWnd == shock.WindowsHandle)
        {
            System.Windows.Forms.MessageBox.Show("Message from specified application found!");
        }

    }
}