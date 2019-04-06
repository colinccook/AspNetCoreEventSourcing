# AspNetCoreEventSourcing

AspNetCoreEventSourcing is a prototype project to experiment event sourcing covering a simple business flow. It's purpose is a learning exercise for myself, with an ultimate aim of determining if it's of any use within my work domain.



![RazorPages End to End](docs/aspnet2.gif)


### Features

* **EventFlow** This is the magical framework used for CQRS and storing the entire state of the app as events
* **Everything in-memory** As everything is stored in-memory, it's quick to clone and run on your local environment
* **ASP.NET Core 2.2 RazorPages** An uncomplicated, WASM/JavaScript free UI layer, as the focus is on EventFlow.

### Business Scenario

The "business" is first line support for various `Sites` around the country. 

Assuming that `Operatives` have been hired and `Sites` acquired:

* Response will raise `Work` when they are contacted by customers within those `Sites` ([WorkRaisedCommand.cs](WorkRaisedCommand.cs))
* Control will queue that `Work` to an `Operative` ([AssignWorkCommand.cs](AssignWorkCommand.cs))
* `Operatives` will then dispatch, arrive and deal with the Work ([AssignWorkCommand.cs](AssignWorkCommand.cs), [AssignWorkCommand.cs](AssignWorkCommand.cs), [AssignWorkCommand.cs](AssignWorkCommand.cs))
  * When `Work` is completed it is marked as Completed ([AssignWorkCommand.cs](AssignWorkCommand.cs))
  * If the `Operative` cannot complete work they abandon it. The `Work` is then back with Control to assign to another `Operative` ([AssignWorkCommand.cs](AssignWorkCommand.cs))

### How it Works

The state of this prototype is not stored in a series of relational database tables. It is stored as a series of events in an event store. 

As you interact with the prototype, the aggregate roots will emit events. If a business rule fales (such as not providing a forename when hiring a new operative, the event is not emitted. All events are persisted in an in-memory event store.

Meanwhile, read models are listening for domain events and persisting important information in a read model store. Queries can then refer to these read model stores. (rather than requerying the entire event store)

### FAQs

#### I'm getting a "WARNING: The target process exited without raising a CoreCLR started event" message
Ensure you have .NET Core 2.2 SDK installed on your machine

### Disclaimer

This prototype should not be used as an architectural reference, a good example of using EventFlow or DDD. I may refine it and grow it as I continue to learn about DDD.
