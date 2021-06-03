using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public static class GuidRegistrar
    {
        private static readonly HashSet<Guid> Registry = new();

        private const int MaxAttempts = 10_000;

        public static Guid Create()
        {
            var newId = Guid.NewGuid();

            if (Registry.Add(newId))
            {
                return newId;
            }

            // with such a low collision chance it is very likely we never reach this
            // this is solely for people who decide they want to load hundreds of assemblies with hundres of scripts

            int currentTry = MaxAttempts;

            do
            {
                newId = Guid.NewGuid();

                if (Registry.Add(newId))
                {
                    return newId;
                }
            }
            while (currentTry-- > 0);

            Helpers.Warnings.TooManyGuidsGenerated(newId, Registry.Count, MaxAttempts);

            return newId;
        }
    }
}
