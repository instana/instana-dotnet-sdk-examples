# Instana SDK-Example for Windows Forms
If you happen to have a Windows-Forms based client app that you want to be traced by Instana, you will likely see that Windows Forms is not a technology we support out of the box.
But there is still a way of getting these applications traced in a meaningful way - by leveraging the SDK.

This application demonstrates in an easy to understand scenario how you can create the entry-, exit- and intermediate-spans that you need in order to make your traces meaningful for your monitoring needs.

We provide a Visual Studio Solution here (VS2019). Clone the repo and open the solution or the folder in the IDE of your choice. NuGet packages are not provided as part of the repository, you will have to restore them.

## What this example demonstrates
It is rather easy to combine both, automatic and codified tracing. This example shows how you can create spans on events which Instana does not support ootb and still get automatic tracing for supported technology.

## How to get started
Clone the repo locally and restore the packages.
Once done, build the project.
Before running it, you should have an Instana agent installed and running. 
To enable automatic tracing for your application, set these environment-variables prior to starting the app:

```bash 
SET COR_ENABLE_PROFILING=1
SET COR_PROFILER={FA8F1DFF-0B62-4F84-887F-ECAC69A65DD3}
InstanaSDKExampleApp.exe
```

## What you will see
This simple example creates traces for three transactions. 

The first transaction called `Main Window Load` is rather simple and does not really contain anything. But it displays how you can create traces for any event happening on your application.

The second transaction is called `Get Country List` which contains several spans which show you what happens inside the app when you click the refresh-button on the UI.
This transaction contains both, codified spans for things happening inside the control-flow, as well as an automatically traced call to a http-resource.

The third trace can be created by clicking the "Open Popup" button. This will create a trace starting at the click-handler and spanning over the load-handler for a popup-form.

Feel free to extend this example to your needs and take it as a starting point for instrumenting your Windows Forms based applications.

Have fun!
