// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace MicroBenchmarks
{
    class Program
    {
        static void Main(string[] args)
            => BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, GetConfig());

        private static IConfig GetConfig()
            => DefaultConfig.Instance
                .With(Job.Default
                    .WithWarmupCount(1) // 1 warmup is enough for our purpose
                    .WithIterationTime(TimeInterval.FromMilliseconds(250)) // the default is 0.5s per iteration, which is slighlty too much for us
                    .WithMinIterationCount(15)
                    .WithMaxIterationCount(20) // we don't want to run more that 20 iterations
                    .AsDefault()) // tell BDN that this are our default settings
                .With(MemoryDiagnoser.Default) // MemoryDiagnoser is enabled by default
                .With(new OperatingSystemFilter())
                .With(JsonExporter.Full) // make sure we export to Json (for BenchView integration purpose)
                .With(StatisticColumn.Median, StatisticColumn.Min, StatisticColumn.Max)
                .With(TooManyTestCasesValidator.FailOnError);
    }
}