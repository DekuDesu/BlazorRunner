using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using BlazorRunnerBench;

BenchmarkRunner.Run(
    typeof(BinaryConstantVsBinaryArray),
    DefaultConfig.Instance
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddExporter(RPlotExporter.Default)
    .AddExporter(CsvExporter.Default)
);