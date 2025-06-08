using System.ComponentModel;
#if !CI_BUILD
using NationalInstruments.DAQmx;
#endif

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
    /// Represents the configuration data for a digital output channel.
    /// This class provides channel configuration that can be used externally.
    /// </summary>
    public class DigitalOutputChannelConfig
    {
        /// <summary>
        /// Gets or sets the name to assign to the local created virtual channel.
        /// If not specified, the physical channel name will be used.
        /// </summary>
        [Description("The name to assign to the local created virtual channel. If not specified, the physical channel name will be used.")]
        public string ChannelName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the names of the digital lines or ports used to create the local virtual channel.
        /// </summary>
        [Description("The names of the digital lines or ports used to create the local virtual channel.")]
        public string Lines { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value specifying how to group digital lines into one or more virtual channels.
        /// </summary>
        [Description("Specifies how to group digital lines into one or more virtual channels.")]
        public DigitalLineGrouping Grouping { get; set; } = DigitalLineGrouping.OneChannelForEachLine;

#if !CI_BUILD
        /// <summary>
        /// Converts this configuration to a Bonsai.DAQmx.DigitalOutputChannelConfiguration.
        /// </summary>
        /// <returns>A DigitalOutputChannelConfiguration instance.</returns>
        public Bonsai.DAQmx.DigitalOutputChannelConfiguration ToDAQmxConfiguration()
        {
            return new Bonsai.DAQmx.DigitalOutputChannelConfiguration
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines,
                Grouping = (ChannelLineGrouping)this.Grouping
            };
        }
#endif
    }
}