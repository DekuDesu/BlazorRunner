using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public abstract class RegisteredObject
    {
        public virtual Guid Id { get; } = GuidRegistrar.Create();
    }
}
