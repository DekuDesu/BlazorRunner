using System;
using System.Threading;
using BlazorRunner.Attributes;
namespace ServerTestAssembly
{
    [Script]
    [Name("Test Script")]
    [Description("My script description")]
    public class MyExampleScript : IDisposable
    {
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

        [Setup]
        public void Before()
        {

        }

        [EntryPoint]
        public void Main()
        {

        }

        [Cleanup]
        public void After()
        {

        }

        [MiniScript(Group = "Mini Scripts")]
        [Name("Alternate Message")]
        [Description("Displays a different message than hello world")]
        public void ExtraStuff()
        {
            Console.WriteLine($"{Crocodile}");
        }

        [MiniScript]
        [Description("Uses " + nameof(Cat) + " For length of sleep")]
        public int WaitAround()
        {
            Console.WriteLine("Started Cat");
            Thread.Sleep(Cat);
            Console.WriteLine("Ended Cat");
            return Cat;
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
