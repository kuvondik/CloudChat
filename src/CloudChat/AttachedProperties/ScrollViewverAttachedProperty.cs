using System.Windows;
using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Scroll an items control to the bottom when the data context changes
    /// </summary>
    public class ScrollToBottomOnLoadProperty : BaseAttachedProperties<ScrollToBottomOnLoadProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is ScrollViewer control))
                return;

            // Scroll content to bottom when context changes
            control.DataContextChanged -= Control_DataContextChanged;
            control.DataContextChanged += Control_DataContextChanged;
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Scroll to bottom
            (sender as ScrollViewer).ScrollToBottom();
        }
    }

    /// <summary>
    /// Automatically keep the scroll at the bottom of the screen when we are already close to the bottom
    /// </summary>
    public class AutoScrollToBottomProperty : BaseAttachedProperties<AutoScrollToBottomProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is ScrollViewer control))
                return;

            // Scroll content to bottom when context changes
            control.ScrollChanged -= Control_ScrollChanged;
            control.ScrollChanged += Control_ScrollChanged;
        }

        private void Control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;

            // If we are close enough to the bottom...
            if (scroll.ScrollableHeight - scroll.VerticalOffset < 35)
                // Scroll to the bottom
                scroll.ScrollToEnd();
        }
    }
}