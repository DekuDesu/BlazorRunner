using System.Reflection;

namespace BlazorRunner.Runner
{
    public interface IScriptSetting : IGrouped, IBasicInfo
    {
        /// <summary>
        /// The actual instance of the object that contains the setting. 
        /// <para>
        /// For example if you had the following script, BackingInstance would be an instance of Test
        /// <code>
        /// [Script]
        /// </code>
        /// <code>
        /// class Test {
        /// </code>
        /// <code>
        ///     [Setting]
        ///     </code>
        /// <code>
        ///     string Text = "test";
        ///     </code>
        /// <code>
        /// }
        /// </code>
        /// </para>
        /// </summary>
        object BackingInstance { get; init; }

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
    }
}