# Digital Output Configuration Documentation

## Overview

The `aind-ni-digitaloutput-configuration` package provides Bonsai nodes for configuring NI-DAQmx Digital Output Channels programmatically, allowing external configuration instead of relying solely on the Bonsai UI.

## API Reference

### DigitalOutputConfigurationSource

A source node that generates configuration objects for Digital Output Channels.

#### Properties

- **ChannelName** (string): The name to assign to the local created virtual channel. If not specified, the physical channel name will be used.
- **Lines** (string): The names of the digital lines or ports used to create the local virtual channel (e.g., "Dev1/port0", "Dev1/port0/line0:7").
- **Grouping** (ChannelLineGrouping): Specifies how to group digital lines into one or more virtual channels.

#### ChannelLineGrouping Enumeration

- **OneChannelForEachLine**: Create one virtual channel for each line
- **OneChannelForAllLines**: Create one virtual channel for all lines

#### Returns

An `IObservable<DigitalOutputChannelConfiguration>` containing the configuration object.

## Usage Examples

### Basic Configuration

```xml
<aind:DigitalOutputConfigurationSource>
  <aind:ChannelName>MyChannel</aind:ChannelName>
  <aind:Lines>Dev1/port0/line0:3</aind:Lines>
  <aind:Grouping>OneChannelForAllLines</aind:Grouping>
</aind:DigitalOutputConfigurationSource>
```

### Multiple Individual Lines

```xml
<aind:DigitalOutputConfigurationSource>
  <aind:ChannelName>IndividualLines</aind:ChannelName>
  <aind:Lines>Dev1/port0/line0,Dev1/port0/line1,Dev1/port0/line2</aind:Lines>
  <aind:Grouping>OneChannelForEachLine</aind:Grouping>
</aind:DigitalOutputConfigurationSource>
```

## Integration with Bonsai.DAQmx

The configuration objects produced by this package are compatible with standard Bonsai.DAQmx nodes and can be used wherever `DigitalOutputChannelConfiguration` objects are expected.
