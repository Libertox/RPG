
using System;

namespace Utility
{
    public abstract class ObservableValue<T> where T : struct
    {
        public event Action<T> OnValueChange;
        public T Value 
        { 
            get { return _value; }
            protected set { 
                _value = value;
                OnValueChange?.Invoke(_value);
            }
        }

        private T _value;

        public ObservableValue(T value)
        {
            Value = value;
        }
    
        public abstract void Add(T value);
        public abstract void Subtract(T value);
   

    }
}
