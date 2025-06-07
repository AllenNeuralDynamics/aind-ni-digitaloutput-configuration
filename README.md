# aind-ni-digitaloutput-configuration

[![License](https://img.shields.io/badge/license-MIT-brightgreen)](LICENSE)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)
[![Bonsai](https://img.shields.io/badge/bonsai-v2.7.0-purple)](https://bonsai-rx.org)

A [Bonsai](https://bonsai-rx.org/) library for configuring NI-DAQmx Digital Output Channels externally from workflows.

## Overview

This package provides a node for exposing NI-DAQmx Digital Output Channel configuration properties that can be set externally from workflows, instead of only through the Bonsai UI. This enables programmatic configuration and better workflow automation for digital output tasks.

**Version 1.1.0** includes compatibility fixes for CI/CD builds with published Bonsai.DAQmx NuGet packages.

## Installation

To install the aind-ni-digitaloutput-configuration package:

1. Open Bonsai
2. Click on the "Tools" menu and select "Manage Packages"
3. Click on "Settings" and add the NuGet package source where this package is hosted
4. Search for "Aind.Ni.DigitalOutput.Configuration" and install

## Prerequisites

- **Windows Operating System** - Bonsai and NI-DAQmx are Windows-only
- **NI-DAQmx Runtime** - Install the National Instruments DAQmx drivers
- **Bonsai** - Version 2.9.0 or later

### Key Components

- **DigitalOutputConfigurationSource**: Generates a configuration object for Digital Output Channels that can be used with other DAQmx nodes
- **DigitalOutputConfig**: Configuration object containing channel setup parameters
- **DigitalLineGrouping**: Enum specifying how to group digital lines into virtual channels

### Example

See the [examples directory](examples/) for sample workflows:
- [DigitalOutputExample.bonsai](examples/DigitalOutputExample.bonsai) - Basic digital output configuration

1. Add a `DigitalOutputConfigurationSource` node to your Bonsai workflow
2. Configure the properties as needed:
   - `ChannelName`: Name for the virtual channel
   - `Lines`: Physical lines to use (e.g., "Dev1/port0/line0:7")
   - `Grouping`: How to group the lines
3. Connect the output to a `DigitalOutput` node or other DAQmx nodes

## Configuration

### DigitalOutputConfigurationSource Properties

- `ChannelName`: The name to assign to the local created virtual channel. If not specified, the physical channel name will be used.
- `Lines`: The names of the digital lines or ports used to create the local virtual channel (e.g., "Dev1/port0", "Dev1/port0/line0:7").
- `Grouping`: Specifies how to group digital lines into one or more virtual channels:
  - `OneChannelForEachLine`: Create one virtual channel for each line
  - `OneChannelForAllLines`: Create one virtual channel for all lines

## Output

The `DigitalOutputConfigurationSource` produces a `DigitalOutputConfig` object that contains:
- `ChannelName`: The configured channel name
- `Lines`: The configured physical lines
- `Grouping`: The configured line grouping

This configuration object can be consumed by other Bonsai.DAQmx nodes or used to programmatically configure digital output channels.

## Deployment

This package is automatically deployed to NuGet.org via a GitHub Actions workflow when a new tag is created on the `main` branch.

To trigger a deployment:

1. Ensure the `Version` property in `src/Aind.Ni.DigitalOutput.Configuration/Aind.Ni.DigitalOutput.Configuration.csproj` is updated to the desired new version.
2. Commit and push any changes to the `main` branch.
3. Manually trigger the "Tag and Publish" workflow in the Actions tab of the GitHub repository.
   - Set the "Publish to NuGet" input to `true`.
   - The workflow will:
     - Read the version from the `.csproj` file.
     - Create a new Git tag in the format `v<version>`.
     - Build the project.
     - Pack the NuGet package.
     - Push the package to NuGet.org using the `NUGET_API_KEY` repository secret.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
