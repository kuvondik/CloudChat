using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CloudChat
{
    /// <summary>
    /// The IsBusy attached property for anything that wants to flag if the control is busy
    /// </summary>
    public class ClipFromBorderProperty : BaseAttachedProperties<ClipFromBorderProperty, bool>
    {
        private RoutedEventHandler mBorder_Loaded;
        private SizeChangedEventHandler mBorder_SizeChanged;
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get self
            var self = (sender as FrameworkElement);

            //Check we have parent border
            if (!(self.Parent is Border border))
            {
                Debugger.Break();
                return;
            }

            mBorder_SizeChanged = (s1, e1) => Border_OnChange(s1, e1, self.Parent as FrameworkElement);
            mBorder_Loaded = (s2, e2) => Border_OnChange(s2, e2, self.Parent as FrameworkElement);


            //if true hook into events
            if ((bool)e.NewValue)
            {
                border.Loaded += mBorder_Loaded;
                border.SizeChanged += mBorder_SizeChanged;
            }
            //Otherwise, unhook
            else
            {
                border.Loaded -= mBorder_Loaded;
                border.SizeChanged -= mBorder_SizeChanged;
            }
        }

        private void Border_OnChange(object sender, RoutedEventArgs e, FrameworkElement child)
        {
            var border = (Border)sender;

            //Check we have an actual size
            if (border.ActualWidth == 0 && border.ActualHeight == 0)
                return;
            var rect = new RectangleGeometry();
            rect.RadiusX = rect.RadiusY = Math.Max(0, border.CornerRadius.TopLeft - border.BorderThickness.Left * 0.5);

            rect.Rect = new Rect(child.RenderSize);
            child.Clip = rect;
        }
    }
}