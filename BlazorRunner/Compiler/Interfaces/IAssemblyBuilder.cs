using System.Reflection;

namespace BlazorRunner.Runner
{
    public interface IAssemblyBuilder
    {
        IScriptAssembly Parse(Assembly assembly);
    }
}