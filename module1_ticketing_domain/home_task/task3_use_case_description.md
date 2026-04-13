Task 3
Draw a sequence diagram for buying a seat of a lowest price (includes finding available seats logic)

Customer open ticketing system (auth optional) and requests events list,  then clicks on a concrete one.
Under selected event he/she views a map of available seats and their prices. Applies sorting by Price to find the cheapest one.

After choosing a seat, customer adds it into a cart. After reviewing details purchase proccess starts.
On this step order is formed. Order details have the next details: event id, seat, total amount.

After filling order details (e.g. customer info) payment process starts. TicketsService creates payment request to Payment service.

Customer can choose payment method (GooglePay, Card, etc.).
In case of card, card details validation required.

Payment service starts payment process and return start confirmation.

Payment service receives response from payment system through the web hook and pushes data into Tickets service.

When payment is completed and confirmed, Tickets service, changes order status and calls sents command to Notification system.participants