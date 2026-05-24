## What are the differences between performance, load, and stress testing?

Performance testing is a general term that includes different techniques that help to check system stability, responsiveness, scalability. It can be used as checkup to ensure that system works as expected and identity performance issues over time.

Load testing is a specific type of performance test. It's more about how system behaves under the high (but expected) load. For example 1000 of the users are trying to get exam result at the same time. This type helps to identify performance bottlenecks.

Stress testing is similar to load test but the load is higher then expected (over SLA). It shows how system behaves under extreme pressure and how it fails and recovery time

## When would you prefer vertical scaling over horizontal?

There are a couple of cases when I'd prefer vertical scaling over horizontal:

 - in case of legacy monolithic app that wasn't designed to run on multiple servers

 - as fast or temp solution when performance boost is required right now and there is no time to accommodate an existing app for horizontal scaling

 - when server is designed for low workload

 - if complex infrastructure architecture is not an option due to infra limitations

## Does ASP.NET Core API support horizontal scaling? Explain your answer.

Technically yes, but in this case an app should be written in proper manner and it's better to decide whether horizontal scaling will be used from the very beginning.

Things that should be taken into account when using horizontal scaling:

 - each request should be stateless and sequential requests can be sent to different servers. e.g. REST

 - need to configure session affinity when data is stored inside session and it's important to process all requests on the same server

 - appropriate caching strategy to avoid irrelevant cached data on the servers

 - infra setup. need to balance the load between the servers and manage them (deployment, increasing/decreasing instances number). also in case of high load, infra dependencies might also require scaling (e.g. DB, distributed cache) as it might become a bottleneck

 - data access. need to ensure that data is consitent between the instances