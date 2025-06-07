using Bonsai;
using Bonsai.DAQmx;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

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
        public string ChannelName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the names of the digital lines or ports used to create the local virtual channel.
        /// </summary>
        public string Lines { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value specifying how to group digital lines into one or more virtual channels.
        /// </summary>
        public DigitalLineGrouping Grouping { get; set; }
    }

    /// <summary>
    /// Provides configuration for NI-DAQmx Digital Output Channels that can be externalized from workflows.
    /// This source generates DigitalOutputConfig objects containing channel configuration data.
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
        /// Generates an observable sequence with a single <see cref="DigitalOutputConfig"/> 
        /// object containing the specified configuration parameters.
        /// </summary>
        /// <returns>
        /// An observable sequence containing a single <see cref="DigitalOutputConfig"/> 
        /// object with the current configuration values.
        /// </returns>
        public IObservable<DigitalOutputConfig> Process()
        {
            return Observable.Return(new DigitalOutputConfig
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines,
                Grouping = this.Grouping
            });
        }
    }
}
