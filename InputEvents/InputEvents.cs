using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;


public class InputEvents
{

    [DllImport("user32")]
    public static extern short GetKeyState(int nVirtKey);
    public static bool GetKeyPressState(int vk)
    {
        int ks = GetKeyState(vk) & 0x8000;
        if (ks > 0)
        {
            return true;
        }
        return false;
    }

    [DllImport("User32")]
    public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

    [DllImport("User32")]
    public extern static void SetCursorPos(int x, int y);

    [DllImport("User32")]
    public extern static bool GetCursorPos(out POINT p);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    public enum MouseEventFlags
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        Wheel = 0x0800,
        Absolute = 0x8000
    }

    public static void click()
    {
        POINT p = new POINT();
        GetCursorPos(out p);
        try
        {
            SetCursorPos(p.X, p.Y);
            mouse_event((int)(MouseEventFlags.LeftDown | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftUp | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
        }
        finally
        {
            SetCursorPos(p.X, p.Y);
        }
    }
}
