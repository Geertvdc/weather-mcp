# Development Container Configuration

This directory contains the Dev Container configuration for the Weather MCP Server project, specifically designed to provide a consistent development environment for GitHub Copilot coding agents.

## Features

- **Pre-installed .NET 9**: The container comes with .NET 9 SDK already installed
- **VS Code Extensions**: Includes essential extensions for .NET development and GitHub Copilot
- **Optimized Settings**: Configured with helpful IntelliSense and development settings
- **Port Forwarding**: Automatically forwards common .NET development ports (5000, 5001)

## Usage

### For GitHub Copilot Agents

When GitHub Copilot agents work on this repository, they will automatically use this dev container configuration, providing:

1. Immediate access to .NET 9 SDK without installation delays
2. Consistent development environment across different agent sessions
3. Pre-configured VS Code settings optimized for .NET development

### For Manual Development

To use this dev container manually:

1. Open the repository in VS Code
2. Install the "Dev Containers" extension if not already installed
3. Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on macOS)
4. Select "Dev Containers: Reopen in Container"
5. VS Code will build and start the container with .NET 9 ready to use

## Container Details

- **Base Image**: `mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm`
- **Features**: Common utilities, Git, Zsh with Oh My Zsh
- **User**: `vscode` (non-root user for security)
- **Ports**: 5000, 5001 (common .NET development ports)

## Verification

After the container starts, you can verify the .NET installation with:

```bash
dotnet --version
```

This should show .NET 9.x.x version information.