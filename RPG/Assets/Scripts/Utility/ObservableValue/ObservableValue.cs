
using System;
using UnityEngine;

namespace Utility
{
    public abstract class ObservableValue<T> where T : struct
    {
        public event Action<T> OnValueChange;
        public T Value 
        { 
            get { return _value; }
            set { 
                _value = value;
                OnValueChange?.Invoke(_value);
                Debug.Log(_value);
            }
        }

        private T _value;

        public ObservableValue(T value)
        {
            Value = value;
        }
    
        public abstract void Add(T value);
        public abstract void Subtract(T value);
   
        public void Set(T newValue)
        {
            Value = newValue;
        }

    }
}
