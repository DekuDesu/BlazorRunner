using System;
using System.Reflection;
using System.Threading;

namespace BlazorRunner.Runner
{
    public interface IInvokableMember : IBasicInfo, IGrouped, IInstanced
    {
        Guid Parent { get; set; }
        MethodInfo BackingMethod { get; }

        object[] DefaultParameters { get; set; }
        bool AcceptsCancellationToken { get; set; }

        object Invoke();

        object Invoke(params object[] parameters);
        object Invoke(CancellationToken token);
        Action<CancellationToken> ToAction();
    }
}