using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Text_Swap.Model;
using WindowsInput;

namespace Text_Swap.Services;

public class KeyboardListenerService
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static Dictionary<string, string> _shortcuts;
    private static string _currentInput = "";

    public static void StartListening()
    {
        LoadShortcuts();
        _hookID = SetHook(_proc);
        Application.Run(); // Garde l'écoute active
        UnhookWindowsHookEx(_hookID);
    }

    public static void StopListening()
    {
        UnhookWindowsHookEx(_hookID);
    }

    private static void LoadShortcuts()
    {
        string filePath = "C:\\shortcuts\\shortcuts.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _shortcuts = JsonSerializer.Deserialize<List<ShortcutModel>>(json).ToDictionary(s => s.Trigger, s => s.Replacement);
        }
        else
        {
            _shortcuts = new Dictionary<string, string>();
        }
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
   {
       if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            char key = (char)vkCode;

            if (char.IsLetterOrDigit(key) || key == ' ')
            {
                 _currentInput += key;
                CheckForShortcut();
            }
            else if (key == '\b' && _currentInput.Length > 0) // Backspace
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private static void CheckForShortcut()
    {
        foreach (var shortcut in _shortcuts)
        {
            if (_currentInput.EndsWith(shortcut.Key.ToUpper()))
            {
                SendKeys.SendWait(shortcut.Value);
                _currentInput = ""; // Réinitialiser
            }
        }
    }

    //private static void CheckForShortcut()
    //{
    //    foreach (var shortcut in _shortcuts)
    //    {
    //        if (_currentInput.EndsWith(shortcut.Key.ToUpper()))
    //        {
    //            InputSimulator sim = new InputSimulator();
    //            sim.Keyboard.TextEntry(shortcut.Value);

    //            _currentInput = ""; // Réinitialiser
    //        }
    //    }
    //}

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

}
