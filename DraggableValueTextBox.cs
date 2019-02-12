using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SomeProjectNamespace
{
    public class DraggableValueTextBox : TextBox
    {
        private bool IsDragging;
        protected Point ClickPoint;
        private bool? IsValueParsed = null;
        private int Value;
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsValueParsed == null)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    string var = Text.Replace(',', '.').Split('.')[0];
                    IsValueParsed = int.TryParse(var, out Value);
                    if (IsValueParsed == true)
                    {
                        Focusable = false;
                        Cursor = Cursors.SizeWE;
                    }
                }
            }
            base.OnMouseEnter(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (IsDragging && IsValueParsed == true)
            {
                Point p = e.GetPosition(this);
                if (p.X > ClickPoint.X)
                {
                    Text = (Value + (int)(Math.Abs(ClickPoint.X - p.X) / 2)).ToString();
                }
                else if (p.X < ClickPoint.X)
                {
                    Text = (Value - (int)(Math.Abs(ClickPoint.X - p.X) / 2)).ToString();
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (!IsFocused && IsValueParsed == true)
            {
                IsDragging = true;
                ClickPoint = e.GetPosition(this);
                Mouse.Capture(this);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsDragging && IsValueParsed == true)
            {
                IsDragging = false;
                Point p = e.GetPosition(this);
                int.TryParse(Text, out Value);
                if (Math.Abs(p.X - ClickPoint.X) < 2)
                {
                    Focusable = true;
                    Focus();
                    Cursor = Cursors.IBeam;
                    CaretIndex = Text.Length;
                }
            }
            else
            {
                Focusable = true;
                Focus();
            }
            ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (IsValueParsed == true)
            {
                Cursor = Cursors.SizeWE;
                Focusable = false;
            }
        }
    }
}
