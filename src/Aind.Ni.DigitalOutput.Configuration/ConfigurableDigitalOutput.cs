using System;
using System.Reactive.Linq;
using NationalInstruments.DAQmx;
using System.Reactive.Disposables;
using System.ComponentModel;
using OpenCV.Net;
using System.Runtime.InteropServices;
using Bonsai;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Represents an operator that writes logical values to one or more DAQmx digital
    /// output lines from a sequence of sample buffers, with external channel configuration support.
    /// This is a fork of Bonsai.DAQmx.DigitalOutput that allows runtime channel configuration.
    /// </summary>
    [Combinator]
    [Description("Writes logical values to one or more DAQmx digital output lines with external channel configuration.")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public class ConfigurableDigitalOutput
    {
        /// <summary>
        /// Gets or sets the name of the DAQmx task.
        /// </summary>
        [Description("The name of the DAQmx task.")]
        public string TaskName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional source terminal of the clock. If not specified,
        /// the internal clock of the device will be used.
        /// </summary>
        [Description("The optional source terminal of the clock. If not specified, the internal clock of the device will be used.")]
        public string SignalSource { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sampling rate for writing logical values, in samples
        /// per second.
        /// </summary>
        [Description("The sampling rate for writing logical values, in samples per second.")]
        public double SampleRate { get; set; } = 1000.0;

        /// <summary>
        /// Gets or sets a value specifying on which edge of a clock pulse sampling takes place.
        /// </summary>
        [Description("Specifies on which edge of a clock pulse sampling takes place.")]
        public SampleClockActiveEdge ActiveEdge { get; set; } = SampleClockActiveEdge.Rising;

        /// <summary>
        /// Gets or sets a value specifying whether the writer task will generate
        /// a finite number of samples or if it continuously generates samples.
        /// </summary>
        [Description("Specifies whether the writer task will generate a finite number of samples or if it continuously generates samples.")]
        public SampleQuantityMode SampleMode { get; set; } = SampleQuantityMode.ContinuousSamples;

        /// <summary>
        /// Gets or sets the number of samples to generate, for finite samples, or the
        /// size of the buffer for continuous samples.
        /// </summary>
        [Description("The number of samples to generate, for finite samples, or the size of the buffer for continuous samples.")]
        public int BufferSize { get; set; } = 1000;

        Task CreateTask(DigitalOutputChannelConfig[] channels)
        {
            var task = string.IsNullOrEmpty(TaskName) ? new Task() : new Task(TaskName);
            foreach (var channel in channels)
            {
                task.DOChannels.CreateChannel(
                    channel.Lines, 
                    channel.ChannelName, 
                    (ChannelLineGrouping)channel.Grouping);
            }

            task.Control(TaskAction.Verify);
            return task;
        }

        IObservable<TSource> ProcessSingleSample<TSource>(
            IObservable<TSource> source, 
            DigitalOutputChannelConfig[] channels,
            Action<DigitalMultiChannelWriter, TSource> onNext)
        {
            return Observable.Defer(() =>
            {
                var task = CreateTask(channels);
                var digitalOutWriter = new DigitalMultiChannelWriter(task.Stream);
                return Observable.Using(
                    () => Disposable.Create(() =>
                    {
                        task.WaitUntilDone();
                        task.Stop();
                        task.Dispose();
                    }),
                    resource => source.Do(input =>
                    {
                        try { onNext(digitalOutWriter, input); }
                        catch { task.Stop(); throw; }
                    }));
            });
        }

        IObservable<TSource> ProcessMultiSample<TSource>(
            IObservable<TSource> source, 
            DigitalOutputChannelConfig[] channels,
            Action<DigitalMultiChannelWriter, TSource> onNext)
        {
            return Observable.Defer(() =>
            {
                var task = CreateTask(channels);
                task.Timing.ConfigureSampleClock(SignalSource, SampleRate, ActiveEdge, SampleMode, BufferSize);
                var digitalOutWriter = new DigitalMultiChannelWriter(task.Stream);
                return Observable.Using(
                    () => Disposable.Create(() =>
                    {
                        if (task.Timing.SampleQuantityMode == SampleQuantityMode.FiniteSamples)
                        {
                            task.WaitUntilDone();
                        }
                        task.Stop();
                        task.Dispose();
                    }),
                    resource => source.Do(input =>
                    {
                        try { onNext(digitalOutWriter, input); }
                        catch { task.Stop(); throw; }
                    }));
            });
        }

        /// <summary>
        /// Writes an observable sequence of logical values to one or more DAQmx
        /// digital output lines using external channel configuration.
        /// </summary>
        /// <param name="source">
        /// A sequence of boolean values representing the logical levels to write
        /// to one or more DAQmx digital output lines.
        /// </param>
        /// <param name="channelConfig">
        /// A sequence containing the channel configuration. Only the first configuration
        /// will be used to set up the DAQmx task.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing logical
        /// values to one or more DAQmx digital output lines.
        /// </returns>
        public IObservable<bool> Process(
            IObservable<bool> source, 
            IObservable<DigitalOutputChannelConfig> channelConfig)
        {
            return channelConfig.Take(1).SelectMany(config =>
                ProcessSingleSample(source, new[] { config }, (writer, value) =>
                {
                    writer.WriteSingleSampleSingleLine(autoStart: true, new[] { value });
                })
            );
        }

        /// <summary>
        /// Writes an observable sequence of boolean samples to one or more DAQmx
        /// digital output lines using external channel configuration.
        /// </summary>
        /// <param name="source">
        /// A sequence of 1D arrays of boolean samples representing the state of a
        /// digital output channel in a local virtual port.
        /// </param>
        /// <param name="channelConfig">
        /// A sequence containing the channel configuration. Only the first configuration
        /// will be used to set up the DAQmx task.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing logical
        /// values to one or more DAQmx digital output lines.
        /// </returns>
        public IObservable<bool[]> Process(
            IObservable<bool[]> source, 
            IObservable<DigitalOutputChannelConfig> channelConfig)
        {
            return channelConfig.Take(1).SelectMany(config =>
                ProcessSingleSample(source, new[] { config }, (writer, data) =>
                {
                    writer.WriteSingleSampleSingleLine(autoStart: true, data);
                })
            );
        }

        /// <summary>
        /// Writes logical values to one or more DAQmx digital output lines from
        /// an observable sequence of sample buffers using external channel configuration.
        /// </summary>
        /// <param name="source">
        /// A sequence of 2D <see cref="Mat"/> objects storing the logical values.
        /// Each row corresponds to a channel in the signal generation task, and each
        /// column to a sample from each of the channels.
        /// </param>
        /// <param name="channelConfig">
        /// A sequence containing the channel configuration. Only the first configuration
        /// will be used to set up the DAQmx task.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing logical
        /// values to one or more DAQmx digital output lines.
        /// </returns>
        public IObservable<Mat> Process(
            IObservable<Mat> source, 
            IObservable<DigitalOutputChannelConfig> channelConfig)
        {
            return channelConfig.Take(1).SelectMany(config =>
                ProcessMultiSample(source, new[] { config }, (writer, input) =>
                {
                    switch (input.Depth)
                    {
                        case Depth.U8:
                        case Depth.S8:
                            writer.WriteMultiSamplePort(autoStart: true, GetMultiSampleArray<byte>(input));
                            break;
                        case Depth.U16:
                        case Depth.S16:
                            writer.WriteMultiSamplePort(autoStart: true, GetMultiSampleArray<ushort>(input));
                            break;
                        case Depth.S32:
                            writer.WriteMultiSamplePort(autoStart: true, GetMultiSampleArray<int>(input));
                            break;
                        default:
                            throw new InvalidOperationException("The elements in the input buffer must have an integer depth type.");
                    }
                })
            );
        }

        static TArray[,] GetMultiSampleArray<TArray>(Mat input) where TArray : unmanaged
        {
            var data = new TArray[input.Rows, input.Cols];
            var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var dataHeader = new Mat(input.Rows, input.Cols, input.Depth, 1, dataHandle.AddrOfPinnedObject());
                CV.Copy(input, dataHeader);
                return data;
            }
            finally { dataHandle.Free(); }
        }
    }
}