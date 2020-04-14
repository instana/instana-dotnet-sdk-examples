# Instana SDK-Example for Orleans
This example is not specificaly about using Orleans or any other framework. The framework here only serves as an example of a technology Instana (currently) does not support out-of-the-box with automatic instrumentation, but can still be found being used in modern applications. No matter if you use Orleans, Akka.NET or any other non-standard framework for distributed systems, you can leverage our SDK to trace the workload throughout the entire application and get vluable insights.
Replace Orleans with whatever you are using and don't see being supported. Maybe drop us a feature-request for your feature, but start with the SDK to get the insight you need.

## What this example demonstrates
In this scenario we are using SDK-tracing only. The communication between Orleans components (Client, Silo, Grain basically) is based on lightweight socket-based message-passing.
There is no obvious way for correlation. So what we assume in this example is that you can very well model the messages you send. We use that fact to create a class which contains correlation data (headers).
This correlation information has to be written on the caller side and extracted on the callee-side to stitch the spans together, where they belong to the same trace.
So the demo here is all about manually correlating spans.

## How to get started
Clone the repo locally and restore the packages.
Once done, build the project.
Before running it, you should have an Instana agent installed and running. 
To enable automatic tracing for your application, set these environment-variables prior to starting the app:

Since we are not doing anything in regards to automatic tracing here, you do not need to set the env-vars.
If you change teh code however to do something that should be automatically traced (like database access, http-calls, logging, etc.) then set them.

```bash 
SET COR_ENABLE_PROFILING=1
SET COR_PROFILER={FA8F1DFF-0B62-4F84-887F-ECAC69A65DD3}
```

You will have to set these env-vars on both, your silo/host and your clients calling into grains.

Start the silo (`OrleansExampleHost`) first and then the client (`OrleansExampleClient`)
Also start your Instana agent, so that it can discover the processes and pick up the traces.

## Understanding correlation

The central part of this example is the correlation for the messages. You will have to deal with the following methods to fully understand

* OrleansExampleClient
    * Program.DoClientWork (the method starting the trace and doing the call)
* OrleansExample.Grains.Impl
    * HelloGrain.GreetMe (the method creating the span)
    * HelloGrain.CorrelateFromMessage (the function for correlation)

The overall workflow for correlation is an exit-span providing the correlation-data to a structure you define in your code (`GrainMessage<T>` in this example), and an entry-span extracting this info and transforming it to `DistributedTraceInformation`.

The `CustomSpan.CreateExit` API let's you define an `Action<string, string>` for that purpose.
On the other side `CustomSpan.CreateEntry` allows you to provide a `Func<DistributedTraceInformation>` which will extract the data we need for correlation (Trace-ID and Parent-Span-Id) from the custom data-source and transform it.


## What you will see

The example-code will create 10 traces of the same kind. The client will call the same grain (`HelloGrain`) in a loop and then terminate.
Those traces consist of a root-span starting when the client prepares it's call towards the grain and a call (exit/entry pair of spans) to that grain.
You will see the two services being created (`OrleansExampleClient` and `HelloGrain`) and carrying calls.

Have fun!
