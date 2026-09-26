
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Entity
{
    public class EntityController : MonoBehaviour
    {
        private readonly Dictionary<Type, IController> controllers = new();
        private bool isDead;
        public bool IsDead => isDead;

        protected virtual void Awake()
        {
            GatherControllers();
        }

        private void GatherControllers()
        {
            var components = GetComponents<IController>();

            foreach (var component in components)
            {
                RegisterController(component);
            }
        }

        public void RegisterController(IController controller)
        {
            if (controller == null)
                return;

            var type = controller.GetType();

            controllers[type] = controller;

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (typeof(IController).IsAssignableFrom(interfaceType))
                {
                    controllers[interfaceType] = controller;
                }
            }
        }

        public void UnregisterController(IController controller)
        {
            var controllerType = controller.GetType();

            if (!controllers.ContainsKey(controllerType))
                return;

            controllers.Remove(controllerType);
        }

        public T GetController<T>() where T : class, IController
        {
            if (controllers.TryGetValue(typeof(T), out var controller))
                return controller as T;

            var attribute = typeof(T).GetCustomAttribute<DefaultControllerAttribute>();

            if (attribute == null)
            {
                Debug.LogWarning($"Controller {typeof(T).Name} does not exist and has no default implementation.");
                return null;
            }

            if (!typeof(T).IsAssignableFrom(attribute.ControllerType))
            {
                Debug.LogError($"Default controller {attribute.ControllerType.Name} does not implement {typeof(T).Name}.");
                return null;
            }

            var instance = Activator.CreateInstance(attribute.ControllerType) as T;

            if (instance == null)
            {
                Debug.LogError($"Could not create default controller {attribute.ControllerType.Name}.");

                return null;
            }

            Debug.LogError($"Created default controller {typeof(T).Name}.");

            controllers.Add(typeof(T), instance);

            return instance;
        }

        public void SetIsDead(bool isDead)
        {
            this.isDead = isDead;
        }

        private void OnDestroy()
        {
            foreach (var controller in controllers)
            {
                if (controller.Value is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}
