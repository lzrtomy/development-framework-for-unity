using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Tools
{
    public class BindableProperty<T>
    {
        public delegate void ValueChangedHandler(T oldValue, T newValue);

        public ValueChangedHandler OnValueChanged;

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!object.Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    ValueChanged(old, _value);
                }
            }
        }

        public BindableProperty() { }
        public BindableProperty(T oldValue) 
        {
            _value = oldValue;
        }

        private void ValueChanged(T oldValue, T newValue)
        {
            if (OnValueChanged != null)
            {
                OnValueChanged(oldValue, newValue);
            }
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "null");
        }
    }
}