# CI/CD Build Strategy for aind-ni-digitaloutput-configuration

## Overview

This project requires a sophisticated build strategy because it depends on NI-DAQmx, which is only available on Windows with specific drivers installed. Our CI/CD pipeline handles this using a dual-build approach.

## Build Strategy

### 1. CI Build (Conditional Compilation)
- **Purpose**: Verify that the code compiles without DAQmx dependencies
- **Platform**: Any (Windows, macOS, Linux)
- **Method**: Uses `CI_BUILD` preprocessor directive
- **Command**: `dotnet build -p:DefineConstants="CI_BUILD"`
- **Result**: Code compiles with all DAQmx-specific code excluded via `#if !CI_BUILD`

### 2. Full Build (Production)
- **Purpose**: Create the actual NuGet package with full DAQmx support
- **Platform**: Windows with Bonsai.DAQmx package
- **Method**: Standard build including all DAQmx references
- **Command**: `dotnet build` (no CI_BUILD flag)
- **Result**: Complete functionality for end users

### 3. Package Build
- **Purpose**: Create distributable NuGet package
- **Platform**: Windows (GitHub Actions)
- **Dependencies**: Bonsai.DAQmx NuGet package provides DAQmx assemblies
- **Result**: Package works on any Windows machine with NI-DAQmx drivers

## Conditional Compilation Implementation

The source code uses conditional compilation to exclude DAQmx-dependent code during CI builds:

```csharp
#if !CI_BUILD
using NationalInstruments.DAQmx;
#endif

// DAQmx-dependent code is wrapped in:
#if !CI_BUILD
    // DAQmx code here
#endif
```

## Package Distribution

The final NuGet package:
- ✅ Includes full DAQmx functionality
- ✅ References Bonsai.DAQmx NuGet package for assemblies
- ✅ Works on Windows with NI-DAQmx drivers installed
- ✅ Compiles correctly when installed via NuGet

## Workflow Files

### `.github/workflows/build.yml`
- Runs CI build to verify conditional compilation
- Runs full build on Windows to verify complete functionality
- Creates NuGet package with full DAQmx support

### `.github/workflows/tag_and_publish.yml`
- Creates versioned releases
- Publishes to NuGet.org with full DAQmx support
- Creates GitHub releases with release notes

## Why This Strategy Works

1. **CI Verification**: Ensures code quality and compilation without requiring DAQmx
2. **Production Quality**: Final package includes all functionality
3. **Cross-Platform Development**: Developers can work on any platform
4. **Windows Deployment**: End users get full functionality on target platform

## Local Development

- **On macOS/Linux**: Use CI build for development/testing
- **On Windows**: Can use full build if DAQmx drivers are installed
- **Testing**: Use conditional compilation to mock DAQmx functionality

## Error Handling

If builds fail:
- **CI Build Failures**: Usually indicate syntax errors or conditional compilation issues
- **Full Build Failures**: May indicate DAQmx assembly issues or missing dependencies
- **Package Build Failures**: Usually NuGet configuration or dependency issues

This strategy ensures that:
1. Code quality is maintained across platforms
2. End users receive fully functional packages
3. CI/CD can run reliably on GitHub Actions
4. Development can happen on any platform
