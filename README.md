# aind-ni-digitaloutput-configuration

[![License](https://img.shields.io/badge/license-MIT-brightgreen)](LICENSE)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)
[![Bonsai](https://img.shields.io/badge/bonsai-v2.7.0-purple)](https://bonsai-rx.org)

A [Bonsai](https://bonsai-rx.org/) extension for configurable NI-DAQmx Digital Output, using runtime channel configuration.

## Overview

This package provides Bonsai combinators and sinks for NI-DAQmx digital output, allowing channel configuration to be provided externally at runtime. It is a drop-in replacement for the standard Bonsai.DAQmx DigitalOutput node, but with more flexible configuration.

## Features

- **Runtime Channel Configuration**: Configure digital output channels at runtime
- **Type-Safe Configuration**: Uses strongly-typed configuration objects
- **Multiple Data Types**: Supports boolean, array, and matrix inputs
- **Bonsai 2.7+ Compatible**

## Installation

1. Open Bonsai
2. Go to "Tools" > "Manage Packages"
3. Add the NuGet source for this package
4. Search for `Aind.Ni.DigitalOutput.Configuration` and install

## Prerequisites

- **Windows**
- **NI-DAQmx Runtime**
- **Bonsai 2.7.0+**

## Components

### DigitalOutputConfigurationSource
Generates configuration objects for digital output channels.

- `ChannelName`: Name for the virtual channel
- `Lines`: Physical lines to use (e.g., "Dev1/port0/line0:7")
- `Grouping`: How to group digital lines (OneChannelForEachLine/OneChannelForAllLines)

### DigitalOutputWriter
Writes digital values to NI-DAQmx digital output lines using the specified configuration.

- `Channels`: Collection of DigitalOutputConfig
- `SignalSource`, `SampleRate`, `ActiveEdge`, `SampleMode`, `BufferSize`: Standard DAQmx timing properties

### ToDigitalOutputConfigCollection
Wraps a single DigitalOutputConfig into a collection for use with DigitalOutputWriter.

## Usage Example

```bonsai
DigitalOutputConfigurationSource -> ToDigitalOutputConfigCollection -> DigitalOutputWriter
BooleanSource                  /
```

## Supported Data Types

- `IObservable<bool>`
- `IObservable<bool[]>`
- `IObservable<bool[,]>`
- `IObservable<byte[,]>`


### Node Descriptions

- **DigitalOutputConfigurationSource**: Use this node to define the digital output channel parameters (name, lines, grouping) in your workflow. Outputs a configuration object.
- **ToDigitalOutputConfigCollection**: Use this node to convert a single configuration into a collection, which is required by the writer node.
- **DigitalOutputWriter**: Use this node to send digital data (bool, bool[], bool[,], or byte[,]) to the hardware, using the configuration(s) provided.

## License

MIT License. See [LICENSE](LICENSE).
