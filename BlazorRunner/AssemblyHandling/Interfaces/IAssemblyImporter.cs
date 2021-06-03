using System.Reflection;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public interface IAssemblyImporter
    {
        LoadAssemblyResult Status { get; }

        Task<Assembly> LoadAsync();
        Assembly TryLoad();
    }
}