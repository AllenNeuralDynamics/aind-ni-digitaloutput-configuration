#if CI_BUILD
// Stubs for CI builds without DAQmx or Bonsai.DAQmx
namespace NationalInstruments.DAQmx
{
    public enum SampleClockActiveEdge { Rising, Falling }
    public enum SampleQuantityMode { FiniteSamples, ContinuousSamples }
    public class Task { public DOChannelCollection DOChannels = new DOChannelCollection(); public Timing Timing = new Timing(); public Stream Stream = new Stream(); public void WaitUntilDone() {} public void Stop() {} public void Dispose() {} }
    public class DOChannelCollection { public void CreateChannel(string lines, string name, ChannelLineGrouping grouping) {} }
    public class Timing { public void ConfigureSampleClock(string src, double rate, SampleClockActiveEdge edge, SampleQuantityMode mode, int buf) {} }
    public class Stream { }
    public class DigitalMultiChannelWriter { public DigitalMultiChannelWriter(Stream s) {} public void WriteMultiSamplePort(bool a, byte[,] b) {} }
    public enum ChannelLineGrouping { OneChannelForEachLine = 0, OneChannelForAllLines = 1 }
}
namespace Bonsai.DAQmx { }
#endif
