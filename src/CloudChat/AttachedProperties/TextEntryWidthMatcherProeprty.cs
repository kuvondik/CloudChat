using System;
using System.Windows;
using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// The NoFrameHistory attached property for creating a <see cref="Frame"/> that never shows navigation
    /// and keeps the navigation history empty
    /// </summary>
    public class TextEntryWidthMatcherProeprty : BaseAttachedProperties<TextEntryWidthMatcherProeprty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get the panel (grid typically)
            var panel = (sender as Panel);

            SetWidths(panel);
            RoutedEventHandler onLoaded = null;

            onLoaded = (ss, ee) =>
            {
                panel.Loaded -= onLoaded;

                SetWidths(panel);

                // Loop each child
                foreach (var child in panel.Children)
                {
                    if (!(child is TextEntryControl control) && !(child is PasswordEntryControl))
                        continue;
                    var label = child is TextEntryControl ? (child as TextEntryControl).Label : (child as PasswordEntryControl).Label;
                    // Set it's margin to the given value
                    label.SizeChanged += (sss, eee) =>
                    {
                        SetWidths(panel);
                    };
                }

            };
            panel.Loaded += onLoaded;

        }

        private void SetWidths(Panel panel)
        {
            var maxSize = 0d;


            foreach (var child in panel.Children)
            {
                if (!(child is TextEntryControl control) && !(child is PasswordEntryControl))
                    continue;
                var label = child is TextEntryControl ? (child as TextEntryControl).Label : (child as PasswordEntryControl).Label;
                maxSize = Math.Max(maxSize, label.RenderSize.Width + label.Margin.Left + label.Margin.Right);
            }


            var gridLength = (GridLength)new GridLengthConverter().ConvertFromString(maxSize.ToString());


            foreach (var child in panel.Children)
            {
                if (child is TextEntryControl text)
                    text.LabelWidth = gridLength;
                else if (child is PasswordEntryControl pass)
                    pass.LabelWidth = gridLength;
            }
        }
    }
}