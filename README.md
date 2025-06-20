# Weather MCP Server

A sample weather API that has been enhanced with Model Context Protocol (MCP) server capabilities, demonstrating how to convert an existing web API into an MCP server.

## Project Overview

This project serves as a practical example of integrating MCP (Model Context Protocol) functionality into an existing .NET web API. It started as a simple weather forecast API and has been enhanced to serve as an MCP server, allowing AI agents and other clients to interact with weather data through the standardized MCP protocol.

### What is MCP (Model Context Protocol)?

The Model Context Protocol (MCP) is an open standard that enables seamless integration between AI applications and external data sources. MCP allows AI models to securely connect to and interact with various tools, databases, and services through a standardized interface.

Key benefits of MCP:
- **Standardized Communication**: Provides a consistent way for AI agents to interact with tools and data sources
- **Security**: Ensures secure and controlled access to external resources
- **Flexibility**: Supports various transport mechanisms (HTTP, WebSocket, etc.)
- **Tool Integration**: Enables AI agents to call functions and access data from external services

## Converting a Web API to MCP Server

This project demonstrates the minimal changes needed to add MCP capabilities to an existing ASP.NET Core web API:

### 1. Package Dependencies
Added the MCP server package to `WeatherMcp.csproj`:
```xml
<PackageReference Include="ModelContextProtocol.AspNetCore" Version="0.2.0-preview.3" />
```

### 2. MCP Tools Implementation
Created `Tools/WeatherTools.cs` with MCP-decorated methods:
- `[McpServerToolType]` attribute on the class
- `[McpServerTool]` attribute on public methods
- `[Description]` attributes for documentation

### 3. Service Registration
In `Program.cs`, added MCP server services:
```csharp
// Add MCP server with HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

// Map MCP endpoints
app.MapMcp();
```

### 4. Tool Classes
The weather tools wrap the existing weather service, making it accessible via MCP:
- `GetWeatherForecast()` - Get forecast for default city
- `GetWeatherForecastForCity(string city)` - Get forecast for specific city

## Running the Application

### Prerequisites
- .NET 9 SDK
- Visual Studio Code (recommended) or Visual Studio

### Build and Run
```bash
# Build the solution
dotnet build

# Run the application
dotnet run --project WeatherMcp

# Or run with watch for development
dotnet watch --project WeatherMcp
```

The application will start on `http://localhost:5260` (or similar port).

### Available Endpoints

**REST API Endpoints:**
- `GET /weatherforecast` - Get 5-day forecast for default city
- `GET /weatherforecast/{city}` - Get 5-day forecast for specific city
- `GET /swagger` - Swagger UI documentation

You can test these endpoints using the [WeatherMcp.http](WeatherMcp/WeatherMcp.http) file. Example:
```http
### Get weather forecast for London
GET http://localhost:5260/weatherforecast/London
Accept: application/json
```

**MCP Endpoints:**
- `POST /` - MCP JSON-RPC endpoint for all MCP communication

Test MCP endpoints using [WeatherMcp.mcp.http](WeatherMcp/WeatherMcp.mcp.http). Example:
```http
### Call GetWeatherForecastForCity Tool
POST http://localhost:5260/
Content-Type: application/json

{
  "jsonrpc": "2.0",
  "id": 5,
  "method": "tools/call",
  "params": {
    "name": "GetWeatherForecastForCity",
    "arguments": {
      "city": "Amsterdam"
    }
  }
}
```

### MCP Inspector Integration

You can use the MCP Inspector to interact with your MCP server:

```bash
npx @modelcontextprotocol/inspector
```

This provides a web-based interface to test MCP tools and explore the server capabilities.

### Claude Desktop Integration

To connect this MCP server to Claude Desktop, add the following to your `claude_desktop_config.json` file:

```json
{
  "mcpServers": {
    "weather": {               
      "command": "npx",                      
      "args": [
        "-y",                                
        "mcp-remote",                        
        "http://localhost:5260"          
      ]
    }
  }
}
```

## Testing the Application

### Unit Tests
Run the comprehensive test suite:
```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run specific test project
dotnet test WeatherMcp.Tests
```

The project includes 22 tests covering:
- Weather service functionality
- REST API endpoints
- MCP server configuration
- Weather tools integration

### REST API Testing
Use the provided HTTP file to test REST endpoints:
```bash
# Using VS Code REST Client extension or similar tools
# Open and run requests from: WeatherMcp/WeatherMcp.http
```

Example requests:
- Get default weather forecast
- Get weather for London, New York, Tokyo
- Access Swagger documentation

### MCP Protocol Testing
Test MCP functionality using the MCP HTTP file:
```bash
# Open and run requests from: WeatherMcp/WeatherMcp.mcp.http
```

MCP test scenarios:
- Initialize MCP session
- List available tools
- Call `GetWeatherForecast` tool
- Call `GetWeatherForecastForCity` tool with parameters

## Development Environment

This repository includes a Dev Container configuration that provides a consistent development environment with .NET 9 pre-installed. This is particularly useful for GitHub Copilot coding agents and ensures a smooth development experience.

### Quick Start with Dev Container

1. Open this repository in VS Code
2. Install the "Dev Containers" extension
3. Press `Ctrl+Shift+P` and select "Dev Containers: Reopen in Container"
4. The container will automatically set up .NET 9 and all necessary tools

For more details about the dev container setup, see [.devcontainer/README.md](.devcontainer/README.md).

### Making Changes

#### Development Workflow
1. **Setup**: Use the dev container for consistent environment
2. **Build**: `dotnet build` to compile the solution
3. **Test**: `dotnet test` to run unit tests
4. **Run**: `dotnet watch --project WeatherMcp` for development with hot reload
5. **HTTP Testing**: Use the `.http` files to test both REST and MCP endpoints

#### Project Structure
```
WeatherMcp/
├── Models/              # Data models (WeatherForecast)
├── Services/            # Business logic (WeatherService)
├── Tools/               # MCP tools (WeatherTools)
├── Program.cs           # Application startup and configuration
├── WeatherMcp.http      # REST API test requests
└── WeatherMcp.mcp.http  # MCP protocol test requests

WeatherMcp.Tests/        # Unit tests for all components
```

This example demonstrates how straightforward it can be to add MCP capabilities to existing APIs, enabling them to be consumed by AI agents and other MCP-compatible clients.
