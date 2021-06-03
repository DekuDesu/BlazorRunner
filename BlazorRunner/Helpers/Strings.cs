using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.Helpers
{
    public static class Strings
    {
        public static string GetCodeAcronym(string FullName)
        {
            // im sorry
            if (FullName?.Length is null or 0)
            {
                return null;
            }

            if (FullName.Contains('.'))
            {
                var split = FullName.Split('.').Select(x => x.FirstOrDefault());
                return string.Join("", split);
            }

            var capsLetters = FullName.Select(x =>
            {
                return char.IsUpper(x) ? x.ToString() : "";
            });

            string caps = string.Join("", capsLetters);

            if (caps.Length == 0)
            {
                return FullName[0..1];
            }
            if (caps.Length > 2)
            {
                return caps[0..1];
            }

            return caps;
        }

        /// <summary>
        /// Generates an integer that is always the same for the given string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetPersistentHash(string str)
        {
            byte[] nums = str.Select(x => (byte)x).ToArray();

            byte[] enc = MD5.HashData(nums);

            int sum = enc.Select(x => (int)x).Sum() << 8;

            for (int i = 0; i < enc.Length; i++)
            {
                sum ^= enc[i];
            }

            return sum;
        }
    }
}
