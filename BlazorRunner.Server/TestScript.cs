using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorRunner.Attributes;
using Microsoft.Extensions.Logging;

[assembly: Name("Test Assembly")]
[assembly: Description("This is a test assembly")]
namespace ServerTestAssembly
{
    [Script]
    [Name("Test Script")]
    [Description("My script description")]
    public class MyExampleScript : IDisposable
    {
        [Setting(Group = "Small Integers")]
        public char SeaSlug { get; set; }

        [Setting(Group = "Small Integers")]
        public sbyte Canary { get; set; }

        [Setting(Group = "Small Integers")]
        public byte Mole { get; set; }

        [Setting(Group = "Small Integers")]
        public short GuineaPig { get; set; }

        [Setting(Group = "Small Integers")]
        public ushort Chamois { get; set; }

        [Setting(Group = "Large Integers")]
        public int Cat { get; set; } = 10_000;

        [Setting(Group = "Large Integers")]
        public uint Porpoise { get; set; }

        [Setting(Group = "Large Integers")]
        public long Mustang { get; set; }

        [Setting(Group = "Large Integers")]
        public ulong Jackal { get; set; }

        [Setting(Group = "Floating Points")]
        [Range(Min = 16)]
        public float Colt { get; set; }

        [Setting(Group = "Floating Points")]
        public double Gopher { get; set; }

        [Setting(Group = "Floating Points")]
        [Range(Max = 20)]
        public decimal Parakeet { get; set; } = 17.9m;

        [Setting(Group = "Bools")]
        public bool Porcupine { get; set; }

        [Setting(Group = "Enums")]
        [Description("This is a pupper")]
        public TypeCode Puppy { get; set; }

        [Setting(Group = "Guids")]
        public Guid Muskrat { get; set; }

        [Setting(Group = "Strings")]
        public string Crocodile { get; set; }

        [Logger]
        ILogger Logger { get; set; }

        [Setup]
        public void Before(CancellationToken token)
        {
            Logger.LogInformation("{Method} ran", nameof(Before));
            Task.Delay(500, token).Wait(token);
        }

        [EntryPoint]
        public void Main(CancellationToken token)
        {
            Logger.LogInformation("{Method} ran", nameof(Main));
            Task.Delay(500, token).Wait(token);
        }

        [Cleanup]
        public void After(CancellationToken token)
        {
            Logger.LogInformation("{Method} ran", nameof(After));
            Task.Delay(500, token).Wait(token);
            throw new TimeZoneNotFoundException();
        }

        [MiniScript(Group = "Mini Scripts")]
        [Name("Alternate Message")]
        [Description("Displays a different message than hello world")]
        public void ExtraStuff()
        {
            Logger.LogInformation("Wrote {Length} to console", Crocodile);
            Console.WriteLine($"{Crocodile}");
        }

        [MiniScript]
        [Description("Uses " + nameof(Cat) + " For length of sleep")]
        public int WaitAround()
        {
            Console.WriteLine("Started Cat");
            Logger.LogInformation("Started Waiting for {Milliseconds}", Cat);
            Thread.Sleep(Cat);
            Logger.LogInformation("Finished Waiting");
            Console.WriteLine("Ended Cat");
            return Cat;
        }

        [MiniScript]
        [Description("Uses " + nameof(Cat) + " For length of sleep")]
        public int WaitAroundWithCancellationToken(CancellationToken token)
        {
            Console.WriteLine("Started Cat");
            Logger.LogInformation("Started Waiting for {Milliseconds}", Cat);
            Task.Delay(Cat, token).Wait(token);
            Logger.LogInformation("Finished Waiting");
            Console.WriteLine("Ended Cat");
            return Cat;
        }

        [MiniScript]
        [Description("Will display a generic exception, the message will be the property Crocodile")]
        public void ThrowsException()
        {
            Console.Error.WriteLine("Throwing Error");
            throw new InvalidTimeZoneException(Crocodile);
        }

        public void HiddenStuff()
        {
            // should not be found and displayed
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
