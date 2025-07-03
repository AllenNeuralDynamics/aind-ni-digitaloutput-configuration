// filepath: c:\Users\jeromel\Documents\Projects\PredictiveProcessingCommunity\Daq_config.cs
using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Bonsai.DAQmx;
using NationalInstruments.DAQmx;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Specifies how to group digital lines into one or more virtual channels.
    /// This enum mirrors NationalInstruments.DAQmx.ChannelLineGrouping for compatibility.
    /// </summary>
    public enum DigitalLineGrouping
    {
        /// <summary>
        /// Create one virtual channel for each line.
        /// </summary>
        OneChannelForEachLine = 0,
        
        /// <summary>
        /// Create one virtual channel for all lines.
        /// </summary>
        OneChannelForAllLines = 1
    }

    /// <summary>
    /// Represents the configuration of a digital output channel for use with Bonsai.DAQmx nodes.
    /// This class provides the same interface as Bonsai.DAQmx.DigitalOutputChannelConfiguration.
    /// </summary>
    public class DigitalOutputConfig
    {
        /// <summary>
        /// Gets or sets the name to assign to the local created virtual channel.
        /// If not specified, the physical channel name will be used.
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the names of the digital lines or ports used to create the local virtual channel.
        /// </summary>
        public string Lines { get; set; }

        /// <summary>
        /// Gets or sets a value specifying how to group digital lines into one or more virtual channels.
        /// </summary>
        public DigitalLineGrouping Grouping { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputConfig"/> class with default values.
        /// </summary>
        public DigitalOutputConfig()
        {
            ChannelName = string.Empty;
            Lines = string.Empty;
            Grouping = DigitalLineGrouping.OneChannelForEachLine;
        }
    }

    /// <summary>
    /// Provides configuration for NI-DAQmx Digital Output Channels that can be externalized from workflows.
    /// </summary>
    [Description("Provides configuration for Digital Output Channel")]
    public class DigitalOutputConfigurationSource : Source<DigitalOutputConfig>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputConfigurationSource"/> class.
        /// </summary>
        public DigitalOutputConfigurationSource()
        {
            ChannelName = string.Empty;
            Lines = "Dev1/port0";
            Grouping = DigitalLineGrouping.OneChannelForEachLine;
        }

        /// <summary>
        /// Gets or sets the name to assign to the local created virtual channel.
        /// If not specified, the physical channel name will be used.
        /// </summary>
        [Description("The name to assign to the local created virtual channel. If not specified, the physical channel name will be used.")]
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the names of the digital lines or ports used to create the local virtual channel.
        /// </summary>
        [Description("The names of the digital lines or ports used to create the local virtual channel.")]
        public string Lines { get; set; }

        /// <summary>
        /// Gets or sets a value specifying how to group digital lines into one or more virtual channels.
        /// </summary>
        [Description("Specifies how to group digital lines into one or more virtual channels.")]
        public DigitalLineGrouping Grouping { get; set; }

        /// <summary>
        /// Generates an observable sequence with a single <see cref="DigitalOutputConfig"/> 
        /// object containing the specified configuration parameters.
        /// </summary>
        /// <returns>
        /// An observable sequence containing a single <see cref="DigitalOutputConfig"/> 
        /// object with the current configuration values.
        /// </returns>
        public override IObservable<DigitalOutputConfig> Generate()
        {
            return Observable.Return(new DigitalOutputConfig
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines,
                Grouping = this.Grouping
            });
        }
    }

    /// <summary>
    /// Writes digital values to the NI-DAQmx digital output lines using the specified configuration.
    /// </summary>
    [DefaultProperty("Channels")]
    [Description("Writes logical values to one or more DAQmx digital output lines from a sequence of sample buffers.")]
    public class DigitalOutputWriter : Sink<byte[,]>
    {
        private Collection<DigitalOutputConfig> channels = new Collection<DigitalOutputConfig>();

        /// <summary>
        /// Gets or sets the collection of digital output channel configurations.
        /// </summary>
        [Editor("Bonsai.Design.DescriptiveCollectionEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        [Description("The collection of digital output channel configurations.")]
        public Collection<DigitalOutputConfig> Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        /// <summary>
        /// Gets or sets the optional source terminal of the clock. If not specified, the internal clock of the device will be used.
        /// </summary>
        public string SignalSource { get; set; }

        /// <summary>
        /// Gets or sets the output sample rate in samples per second.
        /// </summary>
        public double SampleRate { get; set; }

        /// <summary>
        /// Gets or sets which edge of a clock pulse sampling takes place.
        /// </summary>
        public SampleClockActiveEdge ActiveEdge { get; set; }

        /// <summary>
        /// Gets or sets whether the writer task will generate a finite number of samples or if it continuously generates samples.
        /// </summary>
        public SampleQuantityMode SampleMode { get; set; }

        /// <summary>
        /// Gets or sets the number of samples to generate, for finite samples, or the size of the buffer for continuous samples.
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputWriter"/> class.
        /// </summary>
        public DigitalOutputWriter()
        {
            SignalSource = string.Empty;
            SampleRate = 1000;
            ActiveEdge = SampleClockActiveEdge.Rising;
            SampleMode = SampleQuantityMode.ContinuousSamples;
            BufferSize = 1000;
        }

        /// <summary>
        /// Writes digital values to the NI-DAQmx digital output lines using the specified configuration.
        /// </summary>
        /// <param name="source">The observable sequence of byte arrays to write.</param>
        /// <returns>An observable sequence that signals when writing is complete.</returns>
        public override IObservable<byte[,]> Process(IObservable<byte[,]> source)
        {
            return Observable.Defer(() =>
            {
                var task = new NationalInstruments.DAQmx.Task();
                foreach (var config in Channels)
                {
                    task.DOChannels.CreateChannel(
                        config.Lines,
                        config.ChannelName,
                        (NationalInstruments.DAQmx.ChannelLineGrouping)config.Grouping);
                }
                // Only configure sample clock for buffered output
                if (SampleMode == SampleQuantityMode.ContinuousSamples || SampleMode == SampleQuantityMode.FiniteSamples)
                {
                    task.Timing.ConfigureSampleClock(
                        SignalSource,
                        SampleRate,
                        ActiveEdge,
                        SampleMode,
                        BufferSize);
                }
                var writer = new DigitalMultiChannelWriter(task.Stream);
                return Observable.Using(
                    () => Disposable.Create(() =>
                    {
                        task.WaitUntilDone();
                        task.Stop();
                        task.Dispose();
                    }),
                    _ => source.Do(data =>
                    {
                        try { writer.WriteMultiSamplePort(true, data); }
                        catch { task.Stop(); throw; }
                    })
                );
            });
        }

        // Add: ProcessSingleSample logic for static (on-demand) output
        IObservable<TSource> ProcessSingleSample<TSource>(IObservable<TSource> source, Action<DigitalMultiChannelWriter, TSource> onNext)
        {
            return Observable.Defer(() =>
            {
                var task = new NationalInstruments.DAQmx.Task();
                foreach (var config in Channels)
                {
                    task.DOChannels.CreateChannel(
                        config.Lines,
                        config.ChannelName,
                        (NationalInstruments.DAQmx.ChannelLineGrouping)config.Grouping);
                }
                var writer = new DigitalMultiChannelWriter(task.Stream);
                return Observable.Using(
                    () => Disposable.Create(() =>
                    {
                        task.WaitUntilDone();
                        task.Stop();
                        task.Dispose();
                    }),
                    _ => source.Do(input =>
                    {
                        try { onNext(writer, input); }
                        catch { task.Stop(); throw; }
                    })
                );
            });
        }

        /// <summary>
        /// Processes an observable sequence of single boolean values and writes each value as a static (on-demand) digital output sample.
        /// </summary>
        /// <param name="source">The observable sequence of boolean values to write.</param>
        /// <returns>An observable sequence that signals when writing is complete.</returns>
        public IObservable<byte[,]> Process(IObservable<bool> source)
        {
            return ProcessSingleSample(source, (writer, value) =>
            {
                writer.WriteSingleSampleSingleLine(true, new[] { value });
            }).Select(_ => new byte[1, 1]); // Output shape for compatibility
        }

        /// <summary>
        /// Processes an observable sequence of boolean arrays and writes each array as a static (on-demand) digital output sample.
        /// </summary>
        /// <param name="source">The observable sequence of boolean arrays to write.</param>
        /// <returns>An observable sequence that signals when writing is complete.</returns>
        public IObservable<byte[,]> Process(IObservable<bool[]> source)
        {
            return ProcessSingleSample(source, (writer, data) =>
            {
                writer.WriteSingleSampleSingleLine(true, data);
            }).Select(_ => new byte[1, 1]);
        }

        /// <summary>
        /// Processes an observable sequence of byte arrays and writes each array as a static (on-demand) digital output sample.
        /// </summary>
        /// <param name="source">The observable sequence of byte arrays to write.</param>
        /// <returns>An observable sequence that signals when writing is complete.</returns>
        public IObservable<byte[,]> Process(IObservable<byte[]> source)
        {
            return ProcessSingleSample(source, (writer, data) =>
            {
                writer.WriteSingleSamplePort(true, data);
            }).Select(_ => new byte[1, 1]);
        }

        /// <summary>
        /// Converts a sequence of 2D boolean arrays to byte arrays and writes them to the output.
        /// </summary>
        /// <param name="source">The observable sequence of 2D boolean arrays to write.</param>
        /// <returns>An observable sequence that signals when writing is complete.</returns>
        public IObservable<byte[,]> Process(IObservable<bool[,]> source)
        {
            return Process(source.Select(bools =>
            {
                int rows = bools.GetLength(0);
                int cols = bools.GetLength(1);
                var data = new byte[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        data[i, j] = (byte)(bools[i, j] ? 1 : 0);
                return data;
            }));
        }

        /// <summary>
        /// Releases all resources used by the DigitalOutputWriter.
        /// </summary>
        public void Dispose()
        {
            // No-op: resources are managed per subscription
        }
    }

    /// <summary>
    /// Wraps a single DigitalOutputConfig into a collection.
    /// </summary>
    [Description("Wraps a single DigitalOutputConfig into a collection.")]
    public class ToDigitalOutputConfigCollection : Transform<DigitalOutputConfig, Collection<DigitalOutputConfig>>
    {
        /// <summary>
        /// Processes an observable sequence of DigitalOutputConfig and wraps each item in a Collection.
        /// </summary>
        /// <param name="source">The observable sequence of DigitalOutputConfig to wrap.</param>
        /// <returns>
        /// An observable sequence containing a Collection&lt;DigitalOutputConfig&gt; for each 
        /// DigitalOutputConfig in the source sequence.
        /// </returns>
        public override IObservable<Collection<DigitalOutputConfig>> Process(IObservable<DigitalOutputConfig> source)
        {
            return source.Select(config =>
            {
                var collection = new Collection<DigitalOutputConfig>();
                collection.Add(config);
                return collection;
            });
        }
    }
}