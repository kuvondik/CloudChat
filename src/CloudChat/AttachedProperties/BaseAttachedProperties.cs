using System;
using System.Windows;

namespace CloudChat
{
    public abstract class BaseAttachedProperties<Parent, Property>
        where Parent : new()
    {
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        public static Parent Instance { get; } = new Parent();

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached
        (
            "Value",
            typeof(Property),
            typeof(BaseAttachedProperties<Parent, Property>),
            new UIPropertyMetadata(default(Property), new PropertyChangedCallback(OnValuePropertyChanged),
            new CoerceValueCallback(OnValuePropertyUpdated))
        );

        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            (Instance as BaseAttachedProperties<Parent, Property>)?.OnValueUpdated(d, value);
            (Instance as BaseAttachedProperties<Parent, Property>)?.ValueUpdated(d, value);

            return value;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Instance as BaseAttachedProperties<Parent, Property>)?.OnValueChanged(d, e);
            (Instance as BaseAttachedProperties<Parent, Property>)?.ValueChanged(d, e);
        }

        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);


        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        public virtual void OnValueUpdated(DependencyObject sender, object value)
        {
        }
    }
}