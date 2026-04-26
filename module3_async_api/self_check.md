## What are the benefits and drawbacks of async programming?

Benefits:

 - increased performance due to efficient resources usage
 - better scalability
 - makes UI apps more responsive

Drawbacks:

 - error handling is more complex
 - race conditions have a place when updating the same resource in different tasks

## How to make APS.NET controller action support async flow?

Use async (in method signature) and await (when calling async methods) keywords along with an appropriate response type. It might be one of the folowing options:

 - Task or specific type wrapped into Task<>
 - IAsyncEnumerable<>
 - types with GetAwaiter method (e.g. ValueTask<>)

Also it's important to have async operations along the whole request execution path to avoid blocking operations.

As an addition need to pass CancelletionToken as a param for async operation.

## How does async flow influences APS.NET request executions (life cycle)?

### Sync
Lets start from sync request lifecycle to understand the diff.

When client sends a request to the API, to process it server takes an available thread from the pool. The allocated thread will be used till the moment when respone is returned. And in case of calls to the external resources (DB, other APIs) the thead will wait for the response form this resource.

The problem is that Thread Pool has limited number of threads. And when it's exhausted (a lot of requests at the same time) incoming requests have to wait until the Pool will give them available thread. This "queue" causes delays.

### Async

The request lifecycle is similar with one key difference. In case of async call to external resource, thread is not waiting for the completion (e.g. result from DB), and it's returned back to the pool. And only when the async call completed, it requests a thread from pool (might be a different one) to continue execution. This allows server to use initial thread while waiting for the long running operation as it was returned to the pool.

Such approach allows to use available threads in more efficient way. And it's also "unblocks" UI for specific types of application.

## List at least 5 tips on ASP.NET API performance best practices.

- Async API to avoid blocking operations
- Efficient I/O operations (min amount of data, number of requests to external resource, async calls, optimal sql)
- Cache frequently used resources
- Response compression
- Paging for large lists
- Minimaze exceptions

## Vertical vs Horizontal scalability. Where to use each?

### Horizintal
Horizontal scalability at servel level means adding more servers. In this case the load will balanced between the servers.
But this technique requires certain application architecture, each request should be stateless and sequential requests can be sent to different servers.

Also DB or cache may be a bottleneck in this case. 

### Vertical
Vertical scalability refers to improving some of the characteristics of the specific server, for example adding RAM or CPU capacity. This approach has its limitation (hardware) and might be expensive.

The alternative way to scale up server verticaly is to utilize existing resources in more efficient way. And Async API can help with it.

## Explain why the PUT method was suggested for the book action on the order.

PUT is a good candidate as "/book" action changes the status of the existing seats and also updates order with PaymentId ( which is create operation under the hood).

In case of specific fields update PATCH is a better choice. But for more complex scenario like this PUT fits better.
