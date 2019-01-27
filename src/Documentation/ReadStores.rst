.. _read-stores:

Read model stores
=================

In order to create query handlers that perform and enable them search
across multiple fields, read models or projections are used.

To get started you can use the built-in in-memory read model store, but
EventFlow supports a few others as well.

- :ref:`In-memory <read-store-inmemory>`
- :ref:`Microsoft SQL Server <read-store-mssql>`
- :ref:`Elasticsearch <read-store-elasticsearch>`


Creating read models
--------------------

Read models are a flattened view of a subset or all aggregate domain
events created specifically for efficient queries.

Here's a simple example of how a read model for doing searches for
usernames could look. The read model handles the ``UserCreated`` domain
event to get the username and user ID.

.. code-block:: c#

    public class UserReadModel : IReadModel,
      IAmReadModelFor<UserAggregate, UserId, UserCreated>
    {
      public string UserId { get; set; }
      public string Username { get; set; }

      public void Apply(
        IReadModelContext context,
        IDomainEvent<UserAggregate, UserId, UserCreated> domainEvent)
      {
        UserId = domainEvent.AggregateIdentity.Value;
        Username = domainEvent.AggregateEvent.Username.Value;
      }
    }

The read model applies all ``UserCreated`` events and thereby merely saves
the latest value instead of the entire history, which makes it much easier to
store in an efficient manner.


Read model locators
-------------------

Typically the ID of read models are the aggregate identity, but
sometimes this isn't the case. Here are some examples.

-  Items from a collection on the aggregate root
-  Deterministic ID created from event data
-  Entity within the aggregate

To create read models in these cases, use the EventFlow concept of read
model locators, which is basically a mapping from a domain event to a
read model ID.

As an example, consider if we could add several nicknames to a user. We
might have a domain event called ``UserNicknameAdded`` similar to this.

.. code-block:: c#

    public class UserNicknameAdded : AggregateEvent<UserAggregate, UserId>
    {
      public Nickname Nickname { get; set; }
    }

We could then create a read model locator that would return the ID for
each nickname we add via the event like this.

.. code-block:: c#

    public class UserNicknameReadModelLocator : IReadModelLocator
    {
      public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
      {
        var userNicknameAdded = domainEvent as
          IDomainEvent<UserAggregate, UserId, UserNicknameAdded>;
        if (userNicknameAdded == null)
        {
          yield break;
        }

        yield return userNicknameAdded.Nickname.Id;
      }
    }

And then use a read model similar to this that represent each nickname.

.. code-block:: c#

    public class UserNicknameReadModel : IReadModel,
      IAmReadModelFor<UserAggregate, UserId, UserNicknameAdded>
    {
      public string UserId { get; set; }
      public string Nickname { get; set; }

      public void Apply(
        IReadModelContext context,
        IDomainEvent<UserAggregate, UserId, UserCreated> domainEvent)
      {
        UserId = domainEvent.AggregateIdentity.Value;
        Nickname = domainEvent.AggregateEvent.Nickname.Value;
      }
    }

We could then use this nickname read model to query all the nicknames
for a given user by search for read models that have a specific
``UserId``.


Read store implementations
--------------------------

EventFlow has built-in support for several different read model stores.


.. _read-store-inmemory:

In-memory
~~~~~~~~~

The in-memory read store is easy to use and easy to configure. All read
models are stored in-memory, so if EventFlow is restarted all read
models are lost.

To configure the in-memory read model store, simply call
``UseInMemoryReadStoreFor<>`` or ``UseInMemoryReadStoreFor<,>`` with
your read model as the generic argument.

.. code-block:: c#

    var resolver = EventFlowOptions.New
      ...
      .UseInMemoryReadStoreFor<UserReadModel>()
      .UseInMemoryReadStoreFor<UserNicknameReadModel,UserNicknameReadModelLocator>()
      ...
      .CreateResolver();


.. _read-store-mssql:

Microsoft SQL Server
~~~~~~~~~~~~~~~~~~~~

To configure the MSSQL read model store, simply call
``UseMssqlReadModel<>`` or ``UseMssqlReadModel<,>`` with your read model
as the generic argument.

.. code-block:: c#

    var resolver = EventFlowOptions.New
      ...
      .UseMssqlReadModel<UserReadModel>()
      .UseMssqlReadModel<UserNicknameReadModel,UserNicknameReadModelLocator>()
      ...
      .CreateResolver();

By convention, EventFlow uses the table named ``ReadModel-[CLASS NAME]``
as the table to store the read models rows in. If you need to change
this, use the ``Table`` from the
``System.ComponentModel.DataAnnotations.Schema`` namespace. So in the
above example, the read model ``UserReadModel`` would be stored in a
table called ``ReadModel-UserReadModel`` unless stated otherwise.

To allow EventFlow to find the read models stored, a single column is
required to have the ``MsSqlReadModelIdentityColumn`` attribute. This
will be used to store the read model ID.

You should also create a ``int`` column that has the
``MsSqlReadModelVersionColumn`` attribute to tell EventFlow which column
is used to store the read model version in.

.. IMPORTANT::

    EventFlow expect the read model to exist, and thus any
    maintenance of the database schema for the read models must be handled
    before EventFlow is initialized. Or, at least before the read models are
    used in EventFlow.


.. _read-store-elasticsearch:

Elasticsearch
~~~~~~~~~~~~~

To configure the
`Elasticsearch <https://www.elastic.co/products/elasticsearch>`__ read
model store, simply call ``UseElasticsearchReadModel<>`` or
``UseElasticsearchReadModel<,>`` with your read model as the generic
argument.

.. code-block:: c#

    var resolver = EventFlowOptions.New
      ...
      .ConfigureElasticsearch(new Uri("http://localhost:9200/"))
      ...
      .UseElasticsearchReadModel<UserReadModel>()
      .UseElasticsearchReadModel<UserNicknameReadModel,UserNicknameReadModelLocator>()
      ...
      .CreateResolver();

Overloads of ``ConfigureElasticsearch(...)`` is available for
alternative Elasticsearch configurations.

.. IMPORTANT::

    Make sure to create any mapping the read model requires in Elasticsearch
    *before* using the read model in EventFlow.


If EventFlow is requested to *purge* a specific read model, it does it
by deleting the index. Thus make sure to create one separate index per
read model.

If you want to control the index a specific read model is stored in,
create an implementation of ``IReadModelDescriptionProvider`` and
register it in the `EventFlow IoC <./Customize.md>`__.
