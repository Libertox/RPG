
using UnityEngine;

namespace Utility.Attribute
{
    public class HideIfAttribute : PropertyAttribute
    {
        public string conditionField;

        public HideIfAttribute(string conditionField)
        {
            this.conditionField = conditionField;
        }
    }
}
