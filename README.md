# Altinn.ApiClients.Dan

## About
This is a .NET5 client for [data.altinn.no](https://data.altinn.no) APIs that makes it easy to consume [published datasets](https://altinn.github.io/docs/utviklingsguider/data.altinn.no/beviskoder/) in your .NET5 based integrations.

## Installation

Get latest release from [nuget.org](https://www.nuget.org/packages/Altinn.ApiClients.Dan). Pre-release packages are available on [Github](https://github.com/Altinn/altinn-apiclient-dan/packages/)

## Usage

As of version 3.x, Altinn.ApiClients.Dan no longer depends on [Altinn.ApiClients.Maskinporten](https://github.com/Altinn/altinn-apiclient-maskinporten/) for authentication against data.altinn.no, but its use is still recommended for most cases. In order for the Dan client to authenticate, you must provide a HttpMessageHandler. Using the extension methods provided by Altinn.ApiClients.Maskinporten is a convenient way of getting a handler that gets a Maskinporten access token. See [Altinn.ApiClients.Maskinporten](https://github.com/Altinn/altinn-apiclient-maskinporten/) documentation on the different options available and how you can implement your custom provider if needed. 

### Add configuration

Configuration for DAN should be made available via the standard IConfiguration mechanism; the use of the IOptions pattern is not mandatoary. Add the section below to your appsettings.json (or other configuration store). You can name the sections what you want; you'll refer them in the next step.

```jsonc
{
    "MyDanSettings": {
      // "dev" is the development environment for bleeding edge, 
      // "staging" is the pre-prod environment and "prod" is production
      "Environment": "dev",
      "SubscriptionKey": "08f396cb9162478b9fd00daa3e161ef6"
  }
}
```

### Configure service

In Startup.cs, add the following to your `ConfigureServices`-method:

```csharp


public class Startup {

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // This registers an IDanClient for injection in your application code. Note that
        // this does not perform any authentication.
        services.AddDanClient(Configuration.GetSection("MyDanSettings"));


        // This does the same, but configures it to use Newtonsoft.Json instead of
        // the default System.Text.Json deserializer using custom serializer settings
        services.AddDanClient<Pkcs12ClientDefinition>(sp => new DanConfiguration
        {
            // Use Newtonsoft.Json instead of System.Text.Json
            Deserializer = new JsonNetDeserializer {
                SerializerSettings = new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy_MM_dd"
                }
            }
        });

        // Using DAN with Altinn.ApiClients.Maskinporten. Here we use the extension methods provided by
        // that library to first register a client definition instance and the attach it as a HttpMessageHandler
        // to DAN.
        services.RegisterMaskinportenClientDefinition<SettingsJwkClientDefinition>("my-client-definition-for-dan", 
          Configuration.GetSection("MaskinportenSettingsForDanClient"));

        services
            .AddDanClient(Configuration.GetSection("DanSettings"))
            .AddMaskinportenHttpMessageHandler<SettingsJwkClientDefinition>("my-client-definition-for-dan");
    }
}
```

### Use in application code

```csharp
public class MyDanClient 
{
    private readonly IDanClient _danClient;
    
    public MyDanClient(IDanClient danClient)
    {
        _danClient = danClient;
    }

    public void DoStuff()
    {
        var datasetname = "UnitBasicInformation";
        var subject = "991825827";
        
        // Gets a generic dataset which can be iterated
        DataSet dataset = await _danClient.GetDataSet(datasetname, subject);
        foreach (var dsv in dataset.Values)
        {
          // Do something with dsv.Name and dsv.Value
        }

        // If you have a model for the dataset you're getting you can map it directly
        UnitBasicInformation result = await _danClient.GetDataSet<UnitBasicInformation>(datasetname, subject);
        
    }
}
```
See `SampleWebApp` for more usage examples.
