using QTranser;
using QTranser.Properties;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotKeyEditor
{
    public class HotKeyHelper
    {
        private static HashSet<Key> _ignoredKey = new HashSet<Key>() { Key.LeftAlt, Key.RightAlt, Key.LeftCtrl,
            Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.RWin, Key.LWin};

        public static readonly DependencyProperty IsHotKeyEditorProperty =
            DependencyProperty.RegisterAttached(
                "IsHotKeyEditor",
                typeof(Boolean),
                typeof(TextBox),
                new UIPropertyMetadata(false, OnIsHotKeyEditorChanged));

        public static void SetIsHotKeyEditor(UIElement element, Boolean value = true)
        {
            element.SetValue(IsHotKeyEditorProperty, value);
        }

        public static Boolean GetIsHotKeyEditor(UIElement element)
        {
            return (Boolean)element.GetValue(IsHotKeyEditorProperty);
        }

        static void OnIsHotKeyEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;

            if ((bool)e.NewValue)
            {
                textBox.PreviewKeyDown += textBox_PreviewKeyDown;
            }
            else
            {
                textBox.PreviewKeyDown -= textBox_PreviewKeyDown;
            }
        }

        private static void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!_ignoredKey.Contains(e.Key) && (e.Key != Key.System || (e.Key == Key.System && !_ignoredKey.Contains(e.SystemKey))))
            {
                var keys = (e.Key == Key.System && !_ignoredKey.Contains(e.SystemKey)) ? e.SystemKey : e.Key;
                var hotKey = new HotKey()
                {
                    Ctrl = ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control),
                    Alt = ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt),
                    Shift = ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift),
                    Key = keys
                };
                if(hotKey.Ctrl || hotKey.Alt || hotKey.Shift == true)
                {
                    var kc = new KeyConverter();
                    var mkc = new ModifierKeysConverter();
                    if (textBox.Name == "HotKeyQ")
                    {
                        if (e.Key == Key.C ) return;

                        Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyQ);
                        ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModQ);

                        QTranse.HotKeyManage.Unregister(key, mod);
                        QTranse.HotKeyManage.Register(e.Key, Keyboard.Modifiers);
                        Settings.Default.hotKeyQ = kc.ConvertToString(e.Key);
                        Settings.Default.hotKeyModQ = mkc.ConvertToString(Keyboard.Modifiers);
                        Settings.Default.Save();
                        QTranse.Mvvm.HotKeyQ = string.Format($"{hotKey}");
                    }
                    if (textBox.Name == "HotKeyW")
                    {
                        if (e.Key == Key.C) return;

                        Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyW);
                        ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModW);

                        QTranse.HotKeyManage.Unregister(key, mod);
                        QTranse.HotKeyManage.Register(e.Key, Keyboard.Modifiers);
                        Settings.Default.hotKeyW = kc.ConvertToString(e.Key);
                        Settings.Default.hotKeyModW = mkc.ConvertToString(Keyboard.Modifiers);
                        Settings.Default.Save();
                        QTranse.Mvvm.HotKeyW = string.Format($"{hotKey}");
                    }
                    if (textBox.Name == "HotKeyB")
                    {
                        if (e.Key == Key.C ) return;

                        Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyB);
                        ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModB);

                        QTranse.HotKeyManage.Unregister(key, mod);
                        QTranse.HotKeyManage.Register(e.Key, Keyboard.Modifiers);
                        Settings.Default.hotKeyB = kc.ConvertToString(e.Key);
                        Settings.Default.hotKeyModB = mkc.ConvertToString(Keyboard.Modifiers);
                        Settings.Default.Save();
                        QTranse.Mvvm.HotKeyB = string.Format($"{hotKey}");
                    }
                    if (textBox.Name == "HotKeyG")
                    {
                        if (e.Key == Key.C) return;

                        Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyG);
                        ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModG);

                        QTranse.HotKeyManage.Unregister(key, mod);
                        QTranse.HotKeyManage.Register(e.Key, Keyboard.Modifiers);
                        Settings.Default.hotKeyG = kc.ConvertToString(e.Key);
                        Settings.Default.hotKeyModG = mkc.ConvertToString(Keyboard.Modifiers);
                        Settings.Default.Save();
                        QTranse.Mvvm.HotKeyG = string.Format($"{hotKey}");
                    }

                }
                    
            }
            e.Handled = true;
        }
    }
}



