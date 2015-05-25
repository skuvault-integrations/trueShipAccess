# trueShipAccess
TrueShip Wrapper
Version .1.0.0
Author: Timothy Jannace

Notes:
Supports getting orders from TrueShip Readycloud and updating locations to order items on Readycloud.
Needs to have asyncronization added in, currently everything is run synchronously.
Location POST is done on an item level per TrueShip API.  May be able to update an order at a time, as opposed to an order item at a time if it is more efficient.
