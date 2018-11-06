## `fn`

Web application backend used as authentication and proxy layer for [Azure Search](https://docs.microsoft.com/en-us/azure/search/). Private keys are kept server-side for security reasons.

Built with [Azure Functions 2.0](https://docs.microsoft.com/en-us/azure/azure-functions/) targeting [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

### Usage

Functions in `fn`:
- Search: `api/search/{page}/{term}`

### Deployment

```bash
dotnet publish --configuration Release
func azure functionapp publish fn --publish-local-settings 
```