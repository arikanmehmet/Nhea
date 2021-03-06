[![Build Status](https://dev.azure.com/serkanyazicioglu/serkanyazicioglu/_apis/build/status/serkanyazicioglu.Nhea?branchName=master)](https://dev.azure.com/serkanyazicioglu/serkanyazicioglu/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/Nhea.svg)](https://www.nuget.org/packages/Nhea/)

# Nhea Framework

Nhea Framework is a set of tools that helped me over years. This framework consists of following namespaces:
- Communication (Sending E-Mails, Notifications)
- Configuration (Settings for Nhea framework)
- Data (Basic namespace for repository libraries)
- Enumeration (Some helpers for enum usage)
- Globalization (Culture management)
- Localization (Multilanguage)
- Logging (Logging publishers)

Other Helper namespaces:
- Convertion Helpers
- Image Helpers
- Text Helpers


## Getting Started

Nhea is on NuGet. You may install Nhea via NuGet Package manager.

https://www.nuget.org/packages/Nhea/

```
Install-Package Nhea
```

### Prerequisites

Project is built with .NET Framework Standard. 

Dependencies:
- Newtonsoft.Json (>= 11.0.2)
- System.Configuration.ConfigurationManager (>= 4.5.0)
- System.Data.Common (>= 4.3.0)
- System.Data.SqlClient (>= 4.5.1)
- System.Drawing.Common (>= 4.5.1)

## Using Namespaces

This project also supports .net core configuration. Documentation will be provided soon.

### Configuration

First of all we will edit .config file to include Nhea configurations. Inside configSections we're gonna add Nhea's sectionGroup.

```
<configSections>
	<sectionGroup name="nhea" type="Nhea.Configuration.GenericConfigSection, Nhea">
		<section name="application" type="Nhea.Configuration.GenericConfigSection.ApplicationSection.ApplicationConfigSection, Nhea"/>
		<section name="data" type="Nhea.Configuration.GenericConfigSection.DataSection.DataConfigSection, Nhea"/>
		<section name="communication" type="Nhea.Configuration.GenericConfigSection.CommunicationSection.CommunicationConfigSection, Nhea" />
		<section name="log" type="Nhea.Configuration.GenericConfigSection.LogSection.LogConfigSection, Nhea"/>
	</sectionGroup>
</configSections>
```

And then we will embed our settings.

```
<nhea>
	<application environmentType="Development" />
	<data connectionName="MyDbConnectionName" />
	<communication />
	<log />
</nhea>
```

At the end .config file should look like this: 

```
...
<configSections>
	<sectionGroup name="nhea" type="Nhea.Configuration.GenericConfigSection, Nhea">
		<section name="application" type="Nhea.Configuration.GenericConfigSection.ApplicationSection.ApplicationConfigSection, Nhea" />
		<section name="data" type="Nhea.Configuration.GenericConfigSection.DataSection.DataConfigSection, Nhea" />
		<section name="communication" type="Nhea.Configuration.GenericConfigSection.CommunicationSection.CommunicationConfigSection, Nhea" />
		<section name="log" type="Nhea.Configuration.GenericConfigSection.LogSection.LogConfigSection, Nhea" />
	</sectionGroup>
</configSections>
<nhea>
	<application environmentType="Development" />
	<data />
	<communication />
	<log />
</nhea>
  ...
```

### Application segment

Inside the code we can get environment type by using:

```
if (Nhea.Configuration.Settings.Application.EnvironmentType == Nhea.Configuration.EnvironmentType.Production)
{
    //PROD
}
else
{
    //NOT PROD
}
```

Inside environmentType following options can be set:
- Development,
- Integration,
- Uat,
- Staging,
- Production

### Log segment

Nhea has three different kind of logging: File, Database (SQL) and E-Mail. By default Logging targets File.

```
Nhea.Logging.Logger.Log("Hello Nhea!");
```

By executing this code a log file will be created in the executing folder. If you want to change the path or the file name you can override these settings. Directory will be automatically created by Nhea.

```
<nhea>
	<application environmentType="Development" />
	<log directoryPath="C:\Projects\" fileName="MyLogFile.txt" />
</nhea>
```
You may also give file name a formatter for datetime.
```
<nhea>
	<application environmentType="Development" />
	<log directoryPath="C:\Projects\" fileName="NheaLog-{0:dd.MM.yyyy-HH:mm}.txt" />
</nhea>
```

Exceptions can easily be logged by the same method.
```
try
{
    throw new Exception("Test Exception");
}
catch (Exception ex)
{
    Nhea.Logging.Logger.Log(ex);
}
```

If you want to add additional details or parameters to exception logs you may use Exception.Data collection.
```
try
{
    throw new Exception("Test Exception");
}
catch (Exception ex)
{
    ex.Data.Add("Id", 1);
    Nhea.Logging.Logger.Log(ex);
}
```

If you have a SQL database you may want to target your DB for logs. It can easily be done by just a few configuration changes. In order to use SQL first you have to create Nhea's <a href=https://github.com/serkanyazicioglu/Nhea/blob/master/SQL/nhea_Log.sql>log table</a>. Execute the nhea_Log.sql which creates the schema of the table.

And then you should change the configuration. Add data to configuration list and set the connection name. Then change the defaultPublishType to 'Database'.

```
<nhea>
    <application environmentType="Development"></application>
    <data connectionName="DbConnectionStringName" />
    <log defaultPublishType="Database" />
  </nhea>
  <connectionStrings>
    <add name="DbConnectionStringName" connectionString="DB connection string" providerName="System.Data.SqlClient" />
  </connectionStrings>
```


### Mailing v2

As of 1.6.1 I've developed new version for mailing system. This will be supporting some more mailing functionalities by using the same table structure. What you have to do is before updating any of your projects you must update the mail sender process.

Please switch nhea_MailQueue Id column's default value to newsequentialid() when you fetch the latest version.

### Mailing Job Docker

You can use the sample project in the solution to create your mailing service or directly use the docker image. Click <b><a target='_blank' href='https://hub.docker.com/r/nhea/mailservice'>here</a></b> to visit repository docker page or run the following command to pull the image.

```
docker pull nhea/mailservice:latest
```
