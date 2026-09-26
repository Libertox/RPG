using System;

namespace Entity
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class DefaultControllerAttribute : Attribute
    {
        public Type ControllerType { get; }

        public DefaultControllerAttribute(Type controllerType)
        {
            ControllerType = controllerType;
        }
    }
}
