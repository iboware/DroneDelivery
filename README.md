# DroneDelivery

Finds the closest distance between a drone, store and a customer using Djikstra's Algorithm and OpenStreetMap Nominatim API. 

Example API Request:
`https://localhost:5001/DroneDelivery?address=Katernberger Str. 107, 45327 Essen`

Example API Response:
```json
{
    "depot": "Ludenberger Str. 1, 40629 Düsseldorf",
    "store": "Lise-Meitner-Straße 1, 40878 Ratingen",
    "customer": "Katernberger Str. 107, 45327 Essen",
    "delivery_time": {
        "minutes": 33,
        "seconds": 47
    }
}
```
