using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BlazorRunner.Runner.Helpers
{
    public static class Attributes
    {
        public static bool TryAssignProperties<T, U>(object instance) where T : System.Attribute
        {
            var instanceType = instance.GetType();

            var att = instanceType.GetCustomAttribute<T>();

            if (att is null)
            {
                return false;
            }

            // get all the properties of the attribute and see if they match anything on the instance
            // if they do assign the instances value to the attribute value of the same name and typed property

            var properties = att.GetType().GetProperties();

            foreach (var item in properties)
            {
                // check to see if the destination object has each property
                var prop = instanceType.GetProperty(item.Name);

                if (prop != null)
                {
                    // get the instanced value of the property on the attribute
                    var value = item.GetValue(att);

                    // since they have the same property check their types to make sure we are allowed to set it
                    // without running into a runtime InvalidCastException
                    if (TypeValidator.TryGetCompatibility(value, prop.PropertyType, out var compatibility))
                    {
                        // if its not implicit we have to cast
                        if (compatibility is not CastingCompatibility.Implicit or CastingCompatibility.SameType)
                        {
                            // cast then set the value on the instance
                            object castedValue = TypeValidator.Cast(value, prop.PropertyType, compatibility);

                            prop.SetValue(instance, castedValue);

                            continue;
                        }

                        // no casting needed set the value off the bat
                        prop.SetValue(instance, value);
                    }
                    else
                    {
                        // if we failed to coerce the type from the attribute property type to the same named
                        // property type on the destination instance we should give up becuase something is wrong
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
