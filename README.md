# AspNetCoreEventSourcing

AspNetCoreEventSourcing is a prototype project to experiment event sourcing covering a simple business flow. It's purpose is to increase my understand of its practical application within my work business domain.

### Features

* **EventFlow** This is the magical framework that I've used for CQRS and events
* **Everything in-memory** As everything is stored in-memory, it's quick to clone and run on your local environment
* **ASP.NET Core 2.2 RazorPages** An uncomplicated UI layer, as the focus is more on EventFlow.

### Business Scenario

The "business" is first line support for various `Sites` around the country. 

Assuming that `Operatives` have been hired and `Sites` acquired:

* Response will raise `Work` when they are contacted by customers within those `Sites` ([RaiseWorkCommand.cs](RaiseWorkCommand.cs))
* Control will queue that `Work` to an `Operative` ([AssignWorkCommand.cs](AssignWorkCommand.cs))
* `Operatives` will then dispatch, arrive and deal with the Work ([AssignWorkCommand.cs](AssignWorkCommand.cs) [AssignWorkCommand.cs](AssignWorkCommand.cs) [AssignWorkCommand.cs](AssignWorkCommand.cs))
  * When `Work` is completed it is marked as Completed ([AssignWorkCommand.cs](AssignWorkCommand.cs))
  * If the `Work` has remedied itself it can be marked as Closed ([AssignWorkCommand.cs](AssignWorkCommand.cs))
  * If the `Operative` has to leave the `Site`, they can stop. The `Work` is then back with Control to assign to another `Operative` ([AssignWorkCommand.cs](AssignWorkCommand.cs))
* Management will keep on their so called "Work Roadmap". This allows then to keep an eye on queue lengths and end-to-end times ([AssignWorkCommand.cs](AssignWorkCommand.cs))
