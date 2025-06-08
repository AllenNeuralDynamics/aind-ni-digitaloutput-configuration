# CI/CD Build Strategy for aind-ni-digitaloutput-configuration

## Overview

This project uses a **simplified and robust approach** by leveraging the Bonsai.DAQmx NuGet package to provide all necessary DAQmx assemblies. This eliminates the need for conditional compilation and provides a professional, maintainable solution.

## Build Strategy

### Simple Dependency-Based Build ✅
- **Purpose**: Create working assemblies using DAQmx assemblies provided by Bonsai.DAQmx
- **Platform**: Windows (GitHub Actions) 
- **Method**: Standard .NET build with package dependencies
- **Command**: `dotnet build --configuration Release`
- **Result**: ✅ Always builds successfully using assemblies from Bonsai.DAQmx package
- **Status**: ✅ Professional, reliable CI/CD pipeline

## How It Works

**Key Insight**: The Bonsai.DAQmx NuGet package includes the actual DAQmx assemblies and provides them through MSBuild props files. Our project simply depends on this package and uses the assemblies it provides.

### Project Configuration

```xml
<ItemGroup>
  <PackageReference Include="Bonsai.DAQmx" Version="0.4.0" />
</ItemGroup>
```

The Bonsai.DAQmx package automatically:
- ✅ Provides `NationalInstruments.DAQmx.dll` for x64 and x86 platforms
- ✅ Includes `NationalInstruments.Common.dll` in the lib folder
- ✅ Sets up proper assembly references through build props
- ✅ Handles platform-specific assembly resolution

### Target Framework Alignment

```xml
<TargetFramework>net462</TargetFramework>
```

We align our target framework with Bonsai.DAQmx (net462) to ensure full compatibility and proper assembly resolution.

## Package Distribution Strategy

The final NuGet package contains:
- ✅ **Pre-compiled assemblies** built against Bonsai.DAQmx assemblies
- ✅ **Dependency on Bonsai.DAQmx** ensuring all required assemblies are available
- ✅ **Full DAQmx functionality** without conditional compilation complexity
- ✅ **Professional packaging** following standard .NET practices

## Workflow Files

### `.github/workflows/build.yml`
- **Build**: ✅ Always passes using Bonsai.DAQmx assemblies
- **Package**: ✅ Creates standard NuGet packages
- **Verification**: ✅ Confirms assemblies and packages are created correctly

### `.github/workflows/tag_and_publish.yml`
- **Build**: ✅ Reliable builds using dependency assemblies
- **Package**: ✅ Professional NuGet package creation
- **Publish**: ✅ Standard package publishing to NuGet.org

## Why This Strategy Works

1. **Leverage Existing Infrastructure**: Uses Bonsai.DAQmx's proven assembly distribution
2. **No Conditional Compilation**: Clean, maintainable code without preprocessor directives
3. **Standard .NET Practices**: Follows conventional dependency management patterns
4. **Reliable CI/CD**: No "expected failures" or complex workarounds
5. **Professional Distribution**: Creates standard packages that work like any other .NET library

## Local Development

- **Any Platform**: Standard `dotnet build` works everywhere Bonsai.DAQmx can be installed
- **No Special Flags**: No conditional compilation or build flags needed
- **Standard Debugging**: Full IntelliSense and debugging support for DAQmx types
- **Cross-Platform**: Works on Windows, and builds work on CI systems

## End User Installation

When users install the package:

```bash
Install-Package Aind.Ni.DigitalOutput.Configuration
```

The package manager will:
1. ✅ Install our package with pre-compiled assemblies
2. ✅ Automatically install Bonsai.DAQmx as a dependency
3. ✅ Provide full DAQmx functionality when drivers are available
4. ✅ Work seamlessly in Bonsai workflows

## Expected CI Results

| Job | Expected Result | Meaning |
|-----|----------------|---------|
| Build | ✅ Pass | Standard build using dependency assemblies |
| Package | ✅ Pass | Professional NuGet package created |
| Publish | ✅ Pass | Package successfully published |

## Runtime Behavior

The packaged assemblies provide:
- **Full DAQmx functionality** when NI-DAQmx drivers are installed
- **Clear error messages** when drivers are missing (standard DAQmx behavior)
- **Professional user experience** matching other DAQmx-based packages

## Summary

This strategy represents a **mature, professional approach** that:
- ✅ **Eliminates complexity** by using existing infrastructure
- ✅ **Follows best practices** for .NET package development
- ✅ **Provides reliable CI/CD** with no expected failures
- ✅ **Maintains clean code** without conditional compilation
- ✅ **Delivers professional results** that work like any other .NET package

The result is a robust, maintainable package that integrates seamlessly with the Bonsai ecosystem while following industry-standard development practices.
