# Digital Output Configuration Documentation

## Overview

The `aind-ni-digitaloutput-configuration` package provides Bonsai nodes for configuring NI-DAQmx Digital Output Channels programmatically, allowing external configuration instead of relying solely on the Bonsai UI.

**Key Features:**
- Cross-platform development support (build on macOS/Linux, run on Windows)
- Compatibility with published Bonsai.DAQmx NuGet packages
- Self-contained configuration types for maximum compatibility
- CI/CD ready with automated NuGet publishing

## API Reference

### DigitalOutputConfigurationSource

A source node that generates configuration objects for Digital Output Channels.

#### Properties

- **ChannelName** (string): The name to assign to the local created virtual channel. If not specified, the physical channel name will be used.
- **Lines** (string): The names of the digital lines or ports used to create the local virtual channel (e.g., "Dev1/port0", "Dev1/port0/line0:7").
- **Grouping** (DigitalLineGrouping): Specifies how to group digital lines into one or more virtual channels.

#### DigitalLineGrouping Enumeration

- **OneChannelForEachLine**: Create one virtual channel for each line
- **OneChannelForAllLines**: Create one virtual channel for all lines

#### Returns

An `IObservable<DigitalOutputConfig>` containing the configuration object.

### DigitalOutputConfig

Configuration object for digital output channels.

#### Properties

- **ChannelName** (string): The configured channel name
- **Lines** (string): The configured physical lines
- **Grouping** (DigitalLineGrouping): The configured line grouping

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

The configuration objects produced by this package provide a standardized way to configure digital output channels programmatically. While the objects use a custom structure for compatibility across different versions of Bonsai.DAQmx, the configuration parameters directly correspond to the standard DAQmx channel configuration options and can be easily integrated into workflows that use NI-DAQmx digital output functionality.
