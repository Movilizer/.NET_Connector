Movilizer .NET Connector
=============

### Importing the .NET Connector to your project

* Download the project from this repository and unzip.
* Import the project into your solution.
* Reference the .NET Connector project in the project you wish to use it.

### Making your first Cloud call
Connecting to the Movilizer Cloud requires you to implement the abstract Manager class.
Below you will find a simple implementation which uses registry entries to set the Movilizer specific variables, for example systemId, systemPassword, Web Service Host, Web Service Protocol etc.
These registry entries are loaded by the Connector in a RegistryConfigurator.
We also add a Windows Log where we can log possible exceptions the Connector generates. This is advisable.

```c#
public class MovilizerManager : Manager
{
	public MovilizerManager()
            : base()
        {
            LogFactory.RegisterLog(new MWS.Log.WindowsLog()); //For logging certain events and errors.
        }

        public override void RunServiceCycle()
        {
            base.PostMovilizerRequest();
        }
}
```

The RunServiceCycle will be called by the MovilizerWindowsService on a specific interval set in the registry configuration.
Within this method we send a Request to the Cloud and a Response will be send back.
We have now made our first call to the Movilizer Cloud.

To avoid having to use the registry configuration you can e.g. use the app.config to set your Movilizer specific variables.
This can be done using the following code:

```c#
        public MovilizerManager()
            : base(new AppConfigConfigurator())
        {
            LogFactory.RegisterLog(new MWS.Log.WindowsLog());
        }
```

and app.config

```xml
<configuration>
  <appSettings>
    <add key="System ID" value="YOUR_SYSTEMID"/>
    <add key="System Password" value="YOUR_SYSTEMPASS"/>
    <add key="Web Service Host" value="(demo.)movilizer.com"/>
    <add key="Web Service Protocol" value="http(s)"/>
    <add key="Service Time Interval" value="5000"/>
  </appSettings>
</configuration>
```
