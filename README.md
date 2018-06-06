# SeparateStartup

This repo contains the code that supports the blog post https://stevetalkscode.co.uk/separating-aspnetcore-startup

There are three ASP.Net Core 2.0 Solutions in this repository.

### ConfigureAppIo.Demos.SayHello.AllInStartup.sln

This is the initial project which sets the scene before being refactored into the other solutions.

The basic concept of the project is to say the word 'Hello' in multiple languages, each supplied by different implementations of interfaces. This is somewhat contrived example, but is serves the purpose of demonstrating the following:

* Registering classes with multiple interfaces in the DI container
* Registering multiple classes with the same interface
* Using Autofac as the DI service provider instead of the out-of-the-box Microsoft implementation
* Using FluentValidation to validate domain classes instead of using attribute validation in MVC
* Using FluentAssertions in unit tests
* Using TestServer for integration testing of Web API


### ConfigureAppIo.Demos.SayHello.FullySeparateStartupClean.sln

This project is for learning purposes only and is not recommended for use. 

The purpose of this project is to demonstrate how to completely move the Startup.cs class from the main project into another library. Read the blog post for details of the thinking behind this, but in short it is a thought experiment to hide 'plumbing' from the main project. It has limitations and therefore, you should look at the next project for 'best practice'.


### ConfigureAppIo.Demos.SayHello.SeparateStartupClean.sln

This project is a middle road between the above two in trying to provide a clean architecture that hides 'plumbing' code from the main project so that developers are forced to obtain instances of dependencies via the DI container.

In this project, the Startup.cs file remains in the main project, but the registration of classes from outside the main project is delegated to the infrastructure project which has knowledge of all the other dependencies.
