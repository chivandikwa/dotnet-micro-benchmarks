using BenchmarkDotNet.Running;
using micro;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
