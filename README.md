# Azure Functions in-process model code testing sample

Unit and integration tests examples for **Azure Functions in-process model**.

This repo is a sample .NET 6 "Create Notes" API written using Azure Functions template and contains examples on how to properly write unit and integration tests for the Azure Functions classes.

## Features

This project framework provides the following features:

* **Unit tests** for Azure Function classes
* **Integration tests** for Azure Function classes

## Getting Started

### Prerequisites

The code in this repo requires knowledge of the following concepts and frameworks:

- **Unit testing**
- **Integration testing**
- [**xUnit**](https://github.com/xunit/xunit)
  - Testing framework
- [**NSubstitute**](https://nsubstitute.github.io/)
  - Mocking framework
- [**Bogus**](https://github.com/bchavez/Bogus)
  - Fake data generator for C#
- [**FluentAssertions**](https://fluentassertions.com/)
- [**Test Containers**](https://testcontainers.com/)
- [**WireMock.Net**](https://github.com/WireMock-Net/WireMock.Net)

You also need to have **Docker Desktop** installed in your local machine.

### Installation

The API uses **Postgresql** database management system to store the created Notes. 

To run an instance locally with docker you must run your API against a local instance of **Postgresql**, by simply typing the following command:

```
docker run --name postgresql -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v /data:/var/lib/postgresql/data -d postgres
```

To access the Postgresql instance through a management interface you can also run the following docker container:

```
docker run --name my-pgadmin -p 82:80 -e 'PGADMIN_DEFAULT_EMAIL=<your-email-placeholder>' -e 'PGADMIN_DEFAULT_PASSWORD=root'-d dpage/pgadmin4
```

And then access it via http://localhost:82 from a browser in your local machine.

You must also create a DB inside Postgresql called **notesdb**.

### Quickstart

To be able to run this demo on your own in your local machine you need to create a **local.settings.json** file inside the **Fta.DemoFunc.Api** project.

As it’s not advisable to store keys and secrets inside a git repository, for local development you can use a local.settings.json file to store configuration.

Sample **local.settings.json** file:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "NotificationApiUrl": "https://63cacb2d4f53a004202b1df7.mockapi.io/api/v1/",
  "ConnectionStrings": {
    "Database": "Server=localhost;Port=5432;Database=notesdb;User ID=postgres;Password=postgres;"
  }
}
```

The "NotificationApiUrl" setting is a mock 3rd party API (created with [mockapi.io](https://mockapi.io/)) which we call to send notification events that a new note has been created into our system.

You can use the "NotificationApiUrl" setting as is to run the demo API locally. It is here to demonstrate how to handle integration tests against 3rd party APIs that you do not control.