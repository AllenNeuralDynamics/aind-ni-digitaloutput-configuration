# aind-ni-digitaloutput-configuration

[![License](https://img.shields.io/badge/license-MIT-brightgreen)](LICENSE)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)
[![Bonsai](https://img.shields.io/badge/bonsai-v2.7.0-purple)](https://bonsai-rx.org)

A [Bonsai](https://bonsai-rx.org/) library that provides configurable NI-DAQmx Digital Output functionality with external channel configuration support.

## Overview

This package provides a flexible alternative to the standard Bonsai.DAQmx `DigitalOutput` node by allowing channel configuration to be provided externally at runtime. This enables more dynamic and reusable digital output workflows.

## Features

- **Runtime Channel Configuration**: Configure digital output channels through external configuration sources
- **External Configuration Support**: Channel settings can come from files, other nodes, or external systems
- **CI Build Compatibility**: Supports builds without DAQmx runtime for continuous integration
- **Type-Safe Configuration**: Uses strongly-typed configuration objects with validation
- **Multiple Data Types**: Supports boolean, Mat, and array inputs like the standard DigitalOutput

## Installation

To install the aind-ni-digitaloutput-configuration package:

1. Open Bonsai
2. Click on the "Tools" menu and select "Manage Packages"
3. Click on "Settings" and add the NuGet package source where this package is hosted
4. Search for "Aind.Ni.DigitalOutput.Configuration" and install

## Prerequisites

- **Windows Operating System** - Bonsai and NI-DAQmx are Windows-only
- **NI-DAQmx Runtime** - Install the National Instruments DAQmx drivers for full functionality
- **Bonsai** - Version 2.7.0 or later

## Components

This package provides four main components:

### ConfigurableDigitalOutput
The main digital output operator that accepts external channel configuration. This is a more flexible alternative to the standard `DigitalOutput` node.

**Properties:**
- `TaskName`: Optional name for the DAQmx task
- `SignalSource`: Optional source terminal for the clock
- `SampleRate`: Sampling rate in samples per second (default: 1000.0)
- `BufferSize`: Buffer size for continuous samples (default: 1000)
- `ActiveEdge`: Clock edge for sampling (Rising/Falling)
- `SampleMode`: Finite or continuous sample generation

### DigitalOutputConfigurationSource
Generates configuration objects for digital output channels that can be connected to `ConfigurableDigitalOutput`.

**Properties:**
- `ChannelName`: Name for the virtual channel
- `Lines`: Physical lines to use (e.g., "Dev1/port0/line0:7")
- `Grouping`: How to group digital lines (OneChannelForEachLine/OneChannelForAllLines)

### DigitalOutputChannelConfig
Configuration data structure containing channel setup parameters:
- `ChannelName`: The virtual channel name
- `Lines`: Physical DAQmx lines specification
- `Grouping`: Line grouping method

### ConfigureDigitalOutputChannels
Combines multiple channel configurations into arrays for multi-channel scenarios.

## Usage Examples

### Basic Single Channel Configuration

```bonsai
DigitalOutputConfigurationSource -> ConfigurableDigitalOutput
BooleanSource                   /
```

1. Add a `DigitalOutputConfigurationSource` and configure:
   - `ChannelName`: "MyChannel"
   - `Lines`: "Dev1/port0/line0"
   - `Grouping`: OneChannelForEachLine

2. Add a `ConfigurableDigitalOutput` node
3. Connect both the configuration and your boolean data stream to the `ConfigurableDigitalOutput`

### Multi-Channel Configuration

```bonsai
ConfigureDigitalOutputChannels -> ConfigurableDigitalOutput
MatSource                     /
```

1. Configure multiple channels in `ConfigureDigitalOutputChannels`
2. Connect to `ConfigurableDigitalOutput` along with your data source

### External Configuration from File

You can load configuration from external sources and feed it to `ConfigurableDigitalOutput`, enabling dynamic reconfiguration without rebuilding workflows.

## Supported Data Types

`ConfigurableDigitalOutput` supports the same data types as the standard `DigitalOutput`:

- `IObservable<bool>` - Single boolean values
- `IObservable<bool[]>` - Boolean arrays  
- `IObservable<Mat>` - OpenCV Mat objects with integer depth types (U8, S8, U16, S16, S32)

## Architecture

### Design Principles

This package solves the limitation of the standard `DigitalOutput` node which requires design-time channel configuration. By using a combinator pattern with external configuration, `ConfigurableDigitalOutput` enables:

1. **Runtime Configuration**: Channels can be configured from external sources
2. **Reusable Workflows**: The same workflow can work with different hardware setups
3. **Dynamic Reconfiguration**: Channel settings can change during workflow execution

## Testing

The package includes comprehensive tests in the `test/` directory:

```bash
# Run tests (requires DAQmx runtime)
cd test
dotnet run
```

Tests include:
- Configuration object creation and validation
- CI build compatibility
- Basic functionality verification (without hardware)

## Deployment

This package can be deployed to NuGet.org via GitHub Actions workflow. To trigger a deployment:

1. Update the version in the project file
2. Commit and push changes to the `main` branch
3. Create a new tag in the format `v<version>` 
4. The automated workflow will build and publish the package

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
