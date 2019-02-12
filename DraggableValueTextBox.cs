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
        private bool IsValueParsed;
        private int Value;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Focusable = false;
            Cursor = Cursors.SizeWE;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (IsDragging && IsValueParsed)
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
            if (!IsFocused)
            {
                IsDragging = true;
                ClickPoint = e.GetPosition(this);
                IsValueParsed = int.TryParse(Text, out Value);
                Mouse.Capture(this);
                Console.WriteLine(Text + "  " + Value);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsDragging && IsValueParsed)
            {
                IsDragging = false;
                Point p = e.GetPosition(this);
                if (Math.Abs(p.X - ClickPoint.X) < 2)
                {
                    Focusable = true;
                    Focus();
                    Cursor = Cursors.IBeam;
                    CaretIndex = Text.Length;
                }
                ReleaseMouseCapture();
            }
            base.OnMouseUp(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            Cursor = Cursors.SizeWE;
            Focusable = false;
        }
    }
}
