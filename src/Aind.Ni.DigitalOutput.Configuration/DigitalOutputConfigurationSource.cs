using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Provides configuration for NI-DAQmx Digital Output Channels that can be externalized from workflows.
    /// This source generates DigitalOutputChannelConfig objects containing channel configuration data.
    /// </summary>
    [Combinator]
    [Description("Provides configuration for Digital Output Channel")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class DigitalOutputConfigurationSource
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
        /// Generates an observable sequence with a single <see cref="DigitalOutputChannelConfig"/> 
        /// object containing the specified configuration parameters.
        /// </summary>
        /// <returns>
        /// An observable sequence containing a single <see cref="DigitalOutputChannelConfig"/> 
        /// object with the current configuration values.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig> Process()
        {
            return Observable.Return(new DigitalOutputChannelConfig
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines,
                Grouping = this.Grouping
            });
        }

        /// <summary>
        /// Generates an observable sequence of channel configurations for each input value.
        /// </summary>
        /// <typeparam name="TSource">The type of the input sequence.</typeparam>
        /// <param name="source">The input sequence that triggers configuration generation.</param>
        /// <returns>
        /// An observable sequence where each input value triggers the emission of a 
        /// <see cref="DigitalOutputChannelConfig"/> object with the current configuration values.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => new DigitalOutputChannelConfig
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines,
                Grouping = this.Grouping
            });
        }
    }
}