# Introduction

This project is a web application retrieves data from this API.

Users can view repositories sorted by updatedAt date, number of stars, or number of forks. Additionally, users can filter repositories by topic or language and sort them in ascending or descending order.

It is developed using Blazor WebAsembly.

# Installation process

1. Clone the repository using git clone repository-url
2. Navigate to the project folder and open the solution file in Visual Studio.
3. Create a json file inside \GitHubRepoTrackerFE_Blazor\wwwroot folder and name it appsettings.json

## Configuration settings(appsettings.json file)

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

"ApiBaseUrl": "your api base url",
  "ApiEndpoints": {
    "GetAllReposEndpoint": "The endpoint that returns all the repositories",
    "GetAllTopicsEndpoint": "The endpoint that returns all the topics",
    "GetAllLanguagesEndpoint": "The endpoint that returns all the languages",
    "GetAccessToken": "The endpoint that returns the access token"
  },
  "AllowedHosts": "*",
  
  "TokenUserName": "The app username",
  "TokenPassword": "The password used to get the access token"
  }
  ```

  # Build
  To build the code:
  + Open GitHubRepoTrackerFE_Blazor.sln with Visual Studio
  + Update the configuration file mentioned above.
  + From the Build menu, select "Build solution"


