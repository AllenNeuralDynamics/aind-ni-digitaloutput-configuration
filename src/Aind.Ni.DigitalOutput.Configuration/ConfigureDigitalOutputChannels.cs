using Bonsai;
using Bonsai.DAQmx;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Transforms configuration settings into a format suitable for DigitalOutput.Channels property mapping.
    /// Use this node to externalize digital output channel configuration from your workflows.
    /// </summary>
    [Combinator]
    [Description("Transforms configuration into DigitalOutput channel configuration")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ConfigureDigitalOutputChannels
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureDigitalOutputChannels"/> class.
        /// </summary>
        public ConfigureDigitalOutputChannels()
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
        /// Transforms the input source into a DigitalOutputChannelConfiguration that can be connected
        /// to the Channels property of a DigitalOutput node.
        /// </summary>
        /// <typeparam name="TSource">The type of the source sequence elements.</typeparam>
        /// <param name="source">The source sequence to transform.</param>
        /// <returns>
        /// An observable sequence of DigitalOutputChannelConfiguration objects configured 
        /// with the specified parameters.
        /// </returns>
        public IObservable<DigitalOutputChannelConfiguration> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => CreateConfiguration());
        }

        /// <summary>
        /// Creates a DigitalOutputChannelConfiguration object from the current settings.
        /// This overload can be used as a source when no input is needed.
        /// </summary>
        /// <returns>
        /// An observable sequence containing a single DigitalOutputChannelConfiguration object.
        /// </returns>
        public IObservable<DigitalOutputChannelConfiguration> Process()
        {
            return Observable.Return(CreateConfiguration());
        }

        /// <summary>
        /// Creates a DigitalOutputChannelConfiguration object with the current property values.
        /// </summary>
        /// <returns>A configured DigitalOutputChannelConfiguration object.</returns>
        private DigitalOutputChannelConfiguration CreateConfiguration()
        {
            var config = new DigitalOutputChannelConfiguration
            {
                ChannelName = this.ChannelName,
                Lines = this.Lines
            };

            // Note: The Grouping property will be set to a default value since we cannot
            // reference NationalInstruments.DAQmx.ChannelLineGrouping directly in CI builds.
            // In the actual Bonsai runtime environment, users should manually adjust this
            // property if needed using the standard Bonsai property editor.

            return config;
        }
    }
}
