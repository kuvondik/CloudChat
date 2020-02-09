using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CloudChat
{
    /// <summary>
    /// The IsBusy attached property for anything that wants to flag if the control is busy
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperties<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is System.Windows.Controls.Control control))
                return;
            control.Loaded += (ss, ee) => { control.Focus(); };
        }
    }

    public class FocusProperty : BaseAttachedProperties<FocusProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is System.Windows.Controls.Control control))
                return;
            if ((bool)e.NewValue)
                control.Focus();
        }
    }

    public class SelectAllTextProperty : BaseAttachedProperties<SelectAllTextProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxBase control)
            {
                if ((bool)e.NewValue)
                {
                    control.Focus();
                    control.SelectAll();
                }
            }
            if (sender is PasswordBox password)
            {
                if ((bool)e.NewValue)
                {
                    password.Focus();
                    password.SelectAll();
                }
            }

        }
    }
}