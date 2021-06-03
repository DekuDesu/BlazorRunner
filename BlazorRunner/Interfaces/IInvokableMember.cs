using System;
using System.Reflection;

namespace BlazorRunner.Runner
{
    public interface IInvokableMember : IBasicInfo, IGrouped
    {
        MethodInfo BackingMethod { get; }

        object BackingInstance { get; init; }

        object[] DefaultParameters { get; set; }

        object Invoke();

        object Invoke(params object[] parameters);
    }
}