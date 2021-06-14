using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace BlazorRunner.Runner.RuntimeHandling
{
    public static class SettingsDirector
    {
        public const string Path = "settings.json";

        public static LocalDictionary<string, object> Settings { get; private set; } = new LocalDictionary<string, object>(Path);

        public static void Set<T>(string Key, T Value) => Settings.Set(Key, Value);

        public static T Get<T>(string Key) => (T)Settings.Get(Key);

        public static void Save() => Settings.Save();

        public static Task SaveAsync() => Settings.SaveAsync();

        public static bool Load() => Settings.Load();

        public static Task<bool> LoadAsync() => Settings.LoadAsync();

        public static async Task Initialize()
        {
            if (await LoadAsync() is false)
            {
                await SaveAsync();
            }
        }
    }
}
