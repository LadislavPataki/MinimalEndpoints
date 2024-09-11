- POST /api/v1/admin/products
``` json multipart/form-data
{
    "name": string,
    "description": string,
    "productCategory": enum { general, wearable }, //general???
    "price": decimal,
    "enabled": bool,
    "stock": [
        {
            "size": nullable enum { XS, S, M, L, XL, ...}
            "quantity": nullable int,
            "isUnlimited": bool
        },
    ]
    "buyingRules": [
        {
            "buyingLimit": nullable int, //null = unlimited
            "buyingLimitPeriod": nullable enum { monthly, yearly, }, //null = not aplicable
        },
        ...
    ],
    "images": ... // part of multipart/form-data
}
```

- GET /api/v1/products
``` json
[
    {
        "name": string,
        "description": string,
        "productCategory": wearable, //polymorfic json discriminator
        "price": decimal,
        "buyingLimit": int,
        "availability": enum { inStock, outOfStock, limitReached },
        "imageUrls": [ "https://....", ... ],
        "items": [ //depends on discriminator
            {
                "productId": guid,
                "size": enum { XS, S, M, L, XL, ...}
            },

        ]
    },
    {
        "name": string,
        "description": string,
        "productCategory": general, //polymorfic json discriminator
        "price": decimal,
        "buyingLimit": int,
        "availability": enum { inStock, outOfStock, limitReached },
        "imageUrls": [ "https://....", ... ],
        "productId": guid //depends on discriminator
    },
]
```

- POST /api/v1/cart
``` json
[
    {
        "productId": guid,
        "amount": int
    },
]
```

- GET /api/v1/cart
``` json
[
    {
        "itemId": guid,
        "productId": guid,
        "name": string,
        "size": enum { XS, S, M, L, XL, ...}
        "description": string,
        "amount": int
        "imageUrls": [ "https://....", ... ],
    },
]
```

- PUT /api/v1/cart/{itemId}
``` json
{
    "amount": int
}
```

- DELETE /api/v1/cart/{itemId}

- POST /api/v1/cart/place-order
``` json Response
{
    "orderId": guid
}
```

- POST /api/v1/kudos
``` json
{
    "recipients": [ entraId, ... ],
    "subject": string,
    "body": string,
    "kudosCategoryId": guid
}

- GET /api/v1/kudos/categories
``` json
[
    {
        "kudosCategoryId": guid,
        "name": string,
        "description": string
    }
]

- GET /api/v1/profile
``` json
{
    "avatar": string, //probably url
    "name": string,
    "kudosCount": int,
    "accountBalance": decimal,
    "stats": [
        {
            "kudosCategoryId": guid,
            "name": string,
            "description": string,
            "level": int,
            "progressPercentage": int
        },
    ]
}
```

- GET /api/v1/history/received-kudos
``` json
[
    {
        "sender": string,
        "date": date,
        "subject": string,
        "body": string,
        "kudosCategoryName": string
    },
    ...
]
```

- GET /api/v1/history/sent-kudos
``` json
[
    {
        "recipients": [ string, ... ],
        "date": date,
        "subject": string,
        "body": string,
        "kudosCategoryName": string
    },
    ...
]
```

- GET /api/v1/history/orders
``` json
[
    {
        "orderId": int,
        "status": enum { toBecollected, expired, collected },
        "date": dateTime,
        "totalPrice": decimal,
        "items": [
            {
                "productId": guid,
                "name": string,
                "size": enum { XS, S, M, L, XL, ...}
                "description": string,
                "amount": int,
                "price": decimal,
                "imageUrls": [ "https://....", ... ],
            },
        ]
]
```

TODO:
- notification
- dashboard
- admin screens

GET /api/v1/admin/products
``` json
{
    "id": guid,
    "name": string,
    "description": string,
    "category": enum { general, wearable }, //general???
    "price": decimal,
    "avilableSizes": [ enum { XS, S, M, L, XL, ...} ],
    "buyingLimit": int,
    "availability": enum { inStock, outOfStock, limitReached },
    "imageUrls": [ "https://....", ... ]
}
```