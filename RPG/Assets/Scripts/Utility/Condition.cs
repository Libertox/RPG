
using UnityEngine;

namespace Utility
{
    public abstract class Condition : ScriptableObject
    {
        [SerializeField] protected bool isNegate;

        public abstract bool Evaluate();
 
    }

    public abstract class Condition<T> : ScriptableObject
    {
        [SerializeField] protected bool isNegate;

        public abstract bool Evaluate(T targetObject);
    }

    public static class ConditionExtension
    {
        public static bool Evaluate(this Condition[] conditions)
        {
            foreach(var condition in conditions)
            {
                if (!condition.Evaluate())
                    return false;
            }

            return true;
        }

        public static bool Evaluate<T>(this Condition<T>[] conditions, T target)
        {
            foreach (var condition in conditions)
            {
                if (!condition.Evaluate(target))
                    return false;
            }

            return true;
        }
    }
}
