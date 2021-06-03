using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public class ScriptSetting : RegisteredObject, IScriptSetting
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public object BackingInstance { get; init; }

        public bool IsField => BackingField is not null && BackingProperty is null;

        public bool IsProperty => !IsField;

        public PropertyInfo BackingProperty { get; init; }

        public FieldInfo BackingField { get; init; }

        public object Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Group { get; set; }

        private void SetValue(object value)
        {
            BackingProperty?.SetValue(BackingInstance, value);
            BackingField?.SetValue(BackingInstance, value);
        }

        private object GetValue()
        {
            return BackingField?.GetValue(BackingInstance) ?? BackingProperty.GetValue(BackingInstance);
        }

        public override string ToString()
        {
            return $"{Name ?? Id.ToString()} ({(BackingField is null ? "Property" : "Field")})";
        }
    }
}
