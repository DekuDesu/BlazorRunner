using System;

namespace BlazorRunner.Attributes
{
    /// <summary>
    /// Tells the <see cref="BlazorRunner"/>.Runner library that this class should be considered a script and be displayed as it's own executable portion of this assembly.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ScriptAttribute : System.Attribute
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
