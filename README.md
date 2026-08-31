[![](https://img.shields.io/nuget/v/soenneker.namecom.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.namecom.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.namecom.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.namecom.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.namecom.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.namecom.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.namecom.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.namecom.client/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.NameCom.Client

Provides cached, Basic-authenticated `HttpClient` instances for Name.com's production and test APIs.

## Installation

```bash
dotnet add package Soenneker.NameCom.Client
```

## Configuration

```json
{
  "NameCom": {
    "Username": "your-username",
    "Token": "your-api-token"
  }
}
```

## Usage

```csharp
using Soenneker.NameCom.Client.Abstract;
using Soenneker.NameCom.Client.Registrars;

services.AddNameComClientUtilAsSingleton();

INameComClientUtil nameCom = serviceProvider
    .GetRequiredService<INameComClientUtil>();

HttpClient client = await nameCom.Get(cancellationToken: cancellationToken);
```

Pass `test: true` to use `https://api.dev.name.com/v4/`; the provider applies Name.com's required `-test` username suffix automatically.

Do not dispose a returned `HttpClient`; the registered provider owns both production and test clients and removes them from the cache when disposed.
