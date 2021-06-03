using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public interface IBasicInfo
    {
        /// <summary>
        /// The name that was provided using the [Name(string)] attribute, <see cref="BlazorRunner.Attributes.NameAttribute"/>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The description that was provided using the [Description(string)] attribute, <see cref="BlazorRunner.Attributes.DescriptionAttribute"/>
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The unique <see cref="Guid"/> of this object
        /// </summary>
        Guid Id { get; }
    }
}
