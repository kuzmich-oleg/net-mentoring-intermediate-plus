## How ASP.NET API handles multiple requests?

Multiple requests in ASP.NET API are handled in isolation. The concurrency is managed through multithreading and asynchronous programming.

On high level it works the next way:

- Kestrel accepts a new connection (request) and uses ThreadPool to queue it's execution. Once the work item is queued, Kestrel continues listening to process new incomming connections

- Worker thread from the ThreadPool picks up queued work item (request) and starts execution using configured middleware pipeline

In combination with async programming this approach allows to use resources efficiently and handle multiple requests simultaneously.

## What are the benefits and downsides of caching? When should we consider applying caching?

Caching allows to decrease server load by reusing saved data to minimaze expensive operation (e.g. calls to DB, external systems, etc.). But from the other side it brings additional complexity as cached data must be properly managed. Also it might require additional infrastructure setup (e.g. Redis for distributed cache).

Benefits:
 - reduced latency
 - better user experience
 - less server load

Downsides
 - outdated data
 - invalidation complexity
 - increased system complexity

It's recommended to cache frequent read operations, data from external systems with high latency or rate limits, data aggregated by multiple queries. 

## What are the differences between In-memory, Distributed or Request caching options?

In case of In-memory cache data is stored in server RAM, read/write operations are fast. In case of horizontal scaling each server has it's own cached data.

In case Distributed cache data is saved in separate storage (Redis, PostgreSql). It allows to reuse the same data among numerous servers, but latency is higher. Also In-memory and Distributed caches can be used together (Hybrid approach).

Request caching allows to store whole responses for specific requests. Storage location depends on the settings and might be client, server, proxy. It allows to reuse the same response for the resources that are rarely updated.

## What does ‘session affinity’ and ‘thread affinity’ mean? When do we have to consider session affinity?

Session affinity is a technique that allows to route all the requests from the concrete client to the same server within a specific session. It allows to share session data between the requests without saving it inside DB.

Thread affinity is a technique that allows to bind a thread or process to a specific CPU core/cores. This binding allows to maximize core cache usage during processing.

## What are the race conditions and deadlocks? Do they possible in a single threaded application?

Race condition it's a type of concurrency error when multiple threads access shared resource simultaneously and the outcome depends on the order of execution or timing. This error leads to data corruption and system inconsistency as result is unpredictable.
Race conditions are possible in single threaded application in case of async operations (2+ tasks used inside Task.WhenAll).

Deadlock it's an application state when 2 or more threads are waiting for resource occupied by each other. As a result they are waiting infinitely.
Deadlock can occur in single thread app, for example in case of recursive call within the lock. 

## Why is it not safe to use static constructors/fields when your code is running in a multithreaded application?

Static constructors are thread safe (single execution is guaranteed by CLR), but can lead to sevelar issues:

 - deadlock risk. if there is a complex logic inside constructor which requires waiting for other task/thread completion, there is a risk that the other task/thread will try access the same class (with static ctor), but access will be blocked as constructor execution is not completed. as a result ctor waits for thread and threads can't access the blocked ctor, which causes deadlock 
 
 - in case constructor failure class will never be initialized and cause TypeInitializationException when app is trying to access it

Static fields represent kind of shared state and have common issues in case of multithreaded application:

 - race conditions, 2 or more thread are trying to read/write the same variable in the same time in randon order affection each other

 - when updating complex object as static field, operation might fail if shared access is not locked

## What  objects and features .NET proposes to solve race conditions and deadlocks?
 - lock
 - interlocked
 - mutex
 - monitor
 - semaphore, semaphoreSlim
 - eventWaitHandle
 - manualResetEvent, autoResetEvent
 - spinLock
 - thread safe types