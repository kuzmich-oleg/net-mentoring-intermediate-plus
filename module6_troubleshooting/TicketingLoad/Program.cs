using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Text;

var httpClient = Http.CreateDefaultClient();

var scenario = Scenario.Create("book_same_seat", async context =>
{
    var cartId = Guid.NewGuid();

    var createCartRequest = Http
        .CreateRequest("POST", $"https://localhost:7239/orders/carts/{cartId}")
        .WithHeader("Accept", "application/json")
        .WithBody(new StringContent("{ \"offerId\": \"77777777-8888-9999-0000-019dc8d2b0d0\" }", Encoding.UTF8, "application/json"));

    var createCartResponse = await Http.Send(httpClient, createCartRequest);

    if (createCartResponse.IsError)
        return createCartResponse;

    var bookCartRequest = Http
        .CreateRequest("PUT", $"https://localhost:7239/orders/carts/{cartId}/book")
        .WithHeader("Accept", "application/json");

    var bookCartResponse = await Http.Send(httpClient, bookCartRequest);

    return bookCartResponse;
})
.WithoutWarmUp()
.WithLoadSimulations(
    Simulation.Inject(
        rate: 200,
        interval: TimeSpan.FromSeconds(1),
        during: TimeSpan.FromSeconds(2))
);

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();
