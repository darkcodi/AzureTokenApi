# AzureTokenApi
It's an HTTP API that provides a convenient way to obtain access tokens for Azure services using the simplest flow - [device code auth](https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-device-code). This repository aims to simplify the process of acquiring Azure access tokens, allowing developers to focus on integrating Azure services into their applications without worrying about the intricacies of authentication.

## Demo
Go to https://azure-token-api.fly.dev/swagger

## Endpoints
1. `GET /az/get-code` - returns a code that a user should enter on a login page  
   Request:
   ```bash
   curl http://localhost:8080/az/get-code
   ```
   Response:
   ```json
   {
     "device_code":"RAQABAAEAAAD--DLA3VO7QrddgJg7Wevr4aDBYqABv9DyI_7TzZxeH2X_0BkuEW9RLmU9NjbvIe_UD037rC4gWMS4V0swxHfbmFY_TQIxXsYfEK-rCFY2GOT4cN4C8MAVkkYoKaNV3J8tt_GWunNqq5n-luUxZAXVCL0DjGL8PZC74vUzu7bWqUvETzZIMr60Fry4PQXmSv3STF7Q8ufOUneUkrs3hz2ZIAA",
     "user_code":"R7ZSEAY7J",
     "verification_uri":"https://microsoft.com/devicelogin",
     "expires_in":900,
     "interval":5,
     "message":"To sign in, use a web browser to open the page https://microsoft.com/devicelogin and enter the code R7ZSEAY7J to authenticate."
   }
   ```
2. `GET /az/get-token` - redeem a device code for an access token  
   | :warning: WARNING          |
   |:---------------------------|
   | Follow the instructions from the previous step before calling this endpoint (navigate to a page and enter a code). |

   Request:
   ```bash
   curl http://localhost:8080/az/get-token?device_code=RAQABAAEAAAD--DLA3VO7QrddgJg7Wevr4aDB...
   ```
   Response:
   ```json
   {
     "token_type":"Bearer",
     "scope":"https://management.core.windows.net//user_impersonation https://management.core.windows.net//.default",
     "expires_in":4432,
     "ext_expires_in":4432,
     "access_token":"<ommited>",
     "refresh_token":"<ommited>",
     "foci":"1",
     "id_token":"<ommited>",
     "client_info":"eyJ1aWQiOiI1ODYyODBmZi05ODI5LTQzMmEtYWRjMC1iZTY2ZTYxODUwMjAiLCJ1dGlkIjoiNmM1MWM2NTktOWQ1Mi00MWFmLTgxZjctZGRlMTYzODBlODEzIn0"
   }
   ```
3. `GET /az/refresh-token` - redeem a refresh token for an access token
   Request:
   ```bash
   curl http://localhost:8080/az/refresh-token?refresh_token=0.AQkAWcZRbFKdr0GB...
   ```
   Response:
   ```json
   {
    "token_type": "Bearer",
    "scope": "https://management.core.windows.net//user_impersonation https://management.core.windows.net//.default",
    "expires_in": 3611,
    "ext_expires_in": 3611,
    "access_token": "<ommited>",
    "refresh_token": "<ommited>",
    "foci": "1",
    "id_token": "<ommited>",
    "client_info": "eyJ1aWQiOiI1ODYyODBmZi05ODI5LTQzMmEtYWRjMC1iZTY2ZTYxODUwMjAiLCJ1dGlkIjoiNmM1MWM2NTktOWQ1Mi00MWFmLTgxZjctZGRlMTYzODBlODEzIn0"
   }
   ```

## Getting started
### Run from sources
1. Clone the repository:
   ```bash
   git clone https://github.com/darkcodi/AzureTokenApi.git
   ```
2. Start the API server:
   ```bash
   dotnet run
   ```
3. Navigate to Swagger - http://localhost:8080/swagger/
### Docker
TODO
