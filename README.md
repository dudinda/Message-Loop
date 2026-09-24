# Message-Loop

1. [Overview](#overview)  
   - [Custom Topology On Ports](#custom-topology-on-ports)
   - [Message-Loop](#message-loop)
2. [Managing Long-Running Operations](#managing-long-running-operations)
   - [Polling](#polling)
3. [Managing Messages](#managing-messages)
   - [Unicast](#unicast)
   - [Broadcast](#broadcast)
4. [Created With](#created-with)
5. [NuGet](#nuget)

## Overview

<p align="center">
   <img width="400" height="400" alt="hierarchy" src="https://github.com/user-attachments/assets/5cc5fbda-fa2c-415d-90f4-88d6932a13f1" />
   <p align="center">Fig. 1 - </p>
</p>

### Custom Topology On Ports

<p align="center">
   <img width="400" height="400" alt="topology" src="https://github.com/user-attachments/assets/af261481-2df5-490d-a0a7-8e7c69ac7d53" />
   <p align="center">Fig. 2 - </p>
</p>


```json
{
  "status": 5,
  "exception": null,
  "data": "Operation completed on localhost:5000-->Operation completed on localhost:5001-->Operation completed on localhost:5002-->Operation completed on localhost:5003\r\n"
          "Operation completed on localhost:5000-->Operation completed on localhost:5001-->Operation completed on localhost:5003\r\n"
          "Operation completed on localhost:5000-->Operation completed on localhost:5002-->Operation completed on localhost:5003\r\n"
          "Operation completed on localhost:5000-->Operation completed on localhost:5003\r\n"
}
```

<p align="center">Fig. 3 - Example of the output produced by the <code>onNode</code> action. The status value represents the <code>TaskStatus.RanToCompletion</code>  value, and the data field contains the actual graph shown in Fig. 2. The output has been formatted to improve readability.</p>

### Message-Loop

## Managing Long-Running Operations

### Polling

## Managing Messages

### Unicast

### Broadcast

## Created With

[.NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

[ASP.NET Core](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/2.3.9)

[Refit](https://www.nuget.org/packages/Refit/9.0.2)

[PowerShell 7](https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell?view=powershell-7.6)

## Nuget
