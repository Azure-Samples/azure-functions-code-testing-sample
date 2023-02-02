# Project Name

Unit and integration tests examples for Azure Functions

This repo is a sample .NET 6 "Create Notes" API written using Azure Functions template and contains examples on how to properly write unit and integration tests for the Azure Functions classes.

## Features

This project framework provides the following features:

* Unit tests for Azure Function classes
* Integration tests for Azure Function classes

## Getting Started

### Prerequisites

The code in this repo requires knowledge of the following concepts and frameworks:

- Unit testing
- Integration testing
- [xUnit](https://github.com/xunit/xunit)
  - Testing framework
- [NSubstitute](https://nsubstitute.github.io/)
  - Mocking framework
- [FluentAssertions](https://fluentassertions.com/)

### Installation

The API uses Azure Cosmos DB to store the created Notes and to run it locally you must run your API against a local instance of Azure CosmosDB. 

It’s availalble for download [here](https://aka.ms/cosmosdb-emulator)

### Quickstart

Also to be able to run this demo on your own in your local machine you need to create a local.settings.json file inside the Fta.DemoFunc.Api project.

As it’s not advisable to store keys and secrets inside a git repository, for local development you can use a local.settings.json file to store configuration.

Sample local.settings.json file:

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "NotificationApiUrl": "https://63cacb2d4f53a004202b1df7.mockapi.io/api/v1/",
  "CosmosDb": {
    "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=..."
  }
}

The "NotificationApiUrl" setting is a mock 3rd party API (created with [mockapi.io](https://mockapi.io/)) which we call to send notification events that a new note has been created into our system.

You can use the "NotificationApiUrl" setting as is to run the demo API locally. It is here to demonstrate how to handle integration tests against 3rd party APIs that you do not control.
