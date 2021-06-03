using System.Reflection;

namespace BlazorRunner.Runner
{
    public interface IScriptSetting : IGrouped, IBasicInfo, IInstanced
    {
        /// <summary>
        /// The value of this setting
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// The actual field and type information about the original field contained within the instance
        /// </summary>
        FieldInfo BackingField { get; init; }

        /// <summary>
        /// The actual property and type information about the original property contained within the instance
        /// </summary>
        PropertyInfo BackingProperty { get; init; }

        /// <summary>
        /// Whether or not this setting is for a field
        /// </summary>
        bool IsField { get; }

        /// <summary>
        /// Whether or not this setting is for a property
        /// </summary>
        bool IsProperty { get; }

        /// <summary>
        /// Gets the appropriate field or property information for this object along with it's backing instance
        /// </summary>
        /// <returns></returns>
        (MemberInfo Member, object Instance) GetBackingInformation();
    }
}