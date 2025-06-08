using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Aind.Ni.DigitalOutput.Configuration
{
    /// <summary>
    /// Configures multiple digital output channels by providing an array of channel configurations.
    /// This combinator is designed to work with ConfigurableDigitalOutput.
    /// </summary>
    [Combinator]
    [Description("Provides multiple digital output channel configurations")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ConfigureDigitalOutputChannels
    {
        /// <summary>
        /// Gets or sets the array of channel configurations.
        /// </summary>
        [Description("The array of channel configurations to provide.")]
        public DigitalOutputChannelConfig[] Channels { get; set; } = new DigitalOutputChannelConfig[0];

        /// <summary>
        /// Generates an observable sequence with the configured channel array.
        /// </summary>
        /// <returns>
        /// An observable sequence containing the configured channel array.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig[]> Process()
        {
            return Observable.Return(Channels);
        }

        /// <summary>
        /// Generates an observable sequence of channel configurations for each input value.
        /// </summary>
        /// <typeparam name="TSource">The type of the input sequence.</typeparam>
        /// <param name="source">The input sequence that triggers configuration generation.</param>
        /// <returns>
        /// An observable sequence where each input value triggers the emission of the 
        /// configured channel array.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig[]> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => Channels);
        }

        /// <summary>
        /// Merges input channel configurations with the configured channels.
        /// </summary>
        /// <param name="source">The input sequence of channel configurations.</param>
        /// <returns>
        /// An observable sequence where input channels are combined with configured channels.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig[]> Process(IObservable<DigitalOutputChannelConfig> source)
        {
            return source.Select(inputChannel => 
            {
                var allChannels = new DigitalOutputChannelConfig[Channels.Length + 1];
                Channels.CopyTo(allChannels, 0);
                allChannels[Channels.Length] = inputChannel;
                return allChannels;
            });
        }

        /// <summary>
        /// Merges input channel configuration arrays with the configured channels.
        /// </summary>
        /// <param name="source">The input sequence of channel configuration arrays.</param>
        /// <returns>
        /// An observable sequence where input channel arrays are combined with configured channels.
        /// </returns>
        public IObservable<DigitalOutputChannelConfig[]> Process(IObservable<DigitalOutputChannelConfig[]> source)
        {
            return source.Select(inputChannels => 
            {
                var allChannels = new DigitalOutputChannelConfig[Channels.Length + inputChannels.Length];
                Channels.CopyTo(allChannels, 0);
                inputChannels.CopyTo(allChannels, Channels.Length);
                return allChannels;
            });
        }
    }
}