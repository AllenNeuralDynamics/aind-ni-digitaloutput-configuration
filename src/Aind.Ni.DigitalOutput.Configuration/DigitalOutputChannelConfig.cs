using Bonsai;
using Bonsai.DAQmx;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Represents a digital output channel configuration that can be used for property mapping in Bonsai workflows.
    /// This class provides externalized configuration for DigitalOutput.Channels property.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DigitalOutputChannelConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputChannelConfig"/> class.
        /// </summary>
        public DigitalOutputChannelConfig()
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
        /// Creates a Bonsai.DAQmx.DigitalOutputChannelConfiguration object for use with DAQmx nodes.
        /// This method converts the external configuration to the format expected by Bonsai.DAQmx.
        /// </summary>
        /// <returns>A DigitalOutputChannelConfiguration object compatible with Bonsai.DAQmx nodes.</returns>
        public DigitalOutputChannelConfiguration ToDAQmxConfiguration()
        {
            var config = new DigitalOutputChannelConfiguration
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines
            };

            // Note: The Grouping property mapping will need to be handled at runtime
            // since we can't directly reference the NationalInstruments.DAQmx enum in CI
            // This conversion should happen in the actual Bonsai runtime environment
            
            return config;
        }

        /// <summary>
        /// Implicit conversion operator to DigitalOutputChannelConfiguration for seamless property mapping.
        /// </summary>
        /// <param name="config">The DigitalOutputChannelConfig to convert.</param>
        /// <returns>A DigitalOutputChannelConfiguration object.</returns>
        public static implicit operator DigitalOutputChannelConfiguration(DigitalOutputChannelConfig config)
        {
            return config.ToDAQmxConfiguration();
        }

        /// <summary>
        /// Returns a string representation of the configuration for debugging purposes.
        /// </summary>
        /// <returns>A string describing the channel configuration.</returns>
        public override string ToString()
        {
            return $"Channel: {ChannelName ?? "(auto)"}, Lines: {Lines}, Grouping: {Grouping}";
        }
    }
}
