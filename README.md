# PluploadMvc

`PluploadMvc` contains server side components to make it easy to integrate [Plupload](http://plupload.com) into your ASP.NET MVC application. It can be used to deeply integrate [Plupload](http://plupload.com) to make it feel like you're using a normal file input field, but you can also just use it for the included `PluploadHandler` (also supporting [chunked file uploads](http://plupload.com/docs/Chunking)) + `PluploadContext` to easily retrieve the uploaded files. 

[![Build status](http://img.shields.io/appveyor/ci/mwijnands/pluploadmvc.svg?style=flat)](https://ci.appveyor.com/project/mwijnands/pluploadmvc) [![NuGet version](http://img.shields.io/nuget/v/XperiCode.PluploadMvc.svg?style=flat)](https://www.nuget.org/packages/XperiCode.PluploadMvc)

## Installation

The `PluploadMvc` package is available at [NuGet](https://www.nuget.org/packages/XperiCode.PluploadMvc). To install `PluploadMvc`, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console):

> ### Install-Package XperiCode.PluploadMvc

## Documentation

I will start with some examples of using the various server side components separately, to eventually get to a full integration example. All examples are built off of the [Plupload Core API sample](http://plupload.com/examples/core) from the Plupload website at [http://plupload.com/examples/core](http://plupload.com/examples/core).

####PluploadHandler

`PluploadMvc` includes an `HttpHandler` that can be used to handle uploaded files that are uploaded by [Plupload](http://plupload.com). It is added to the `Web.config` as `Plupload.axd` automatically when using [NuGet]([NuGet](https://www.nuget.org/packages/XperiCode.PluploadMvc)) to install the package.

    <system.webServer>
        <handlers>
            <add name="PluploadHandler" path="Plupload.axd" verb="POST" type="XperiCode.PluploadMvc.PluploadHandler, XperiCode.PluploadMvc" />
        </handlers>
    </system.webServer>

This `HttpHandler` can be used to handle file uploads from [Plupload](http://plupload.com). To be able to retrieve the uploaded files later, **you need to pass the HttpHandler a reference in the form of a Guid** (this will soon be a string in v0.3.0 for more flexibility). So in javascript, the url option passed to the plupload.Uploader constructor should be something like this:

    var uploader = new plupload.Uploader({
        // ...
        url : "/plupload.axd?reference=13095a38-6841-4204-a2cc-61135d812436",
        // ...
    });

The `PluploadHandler` will now handle the file uploads (it handles [chunked uploads](http://plupload.com/docs/Chunking) as well as normal uploads), and saves the files to a temporary folder at `~/App_Data/PluploadMvc/13095a38-6841-4204-a2cc-61135d812436/`.

#### PluploadContext

To easily retrieve the uploaded files, you can use `PluploadContext`. You should not create an instance of this class yourself. Instead, get it off `HttpContextBase` by using the provided extension method `GetPluploadContext()`. For example, within a `Controller` you could use:

    using XperiCode.PluploadMvc;
	// ...
    var pluploadContext = HttpContext.GetPluploadContext();

After acquiring the `PluploadContext`, you can use the `GetFiles()` method to retrieve the files that were uploaded using the specific `reference`:

	var reference = Guid.Parse("13095a38-6841-4204-a2cc-61135d812436");
    var uploadedFiles = pluploadContext.GetFiles(reference);

`uploadedFiles` will now contain a list of `PluploadFile` objects (inheriting `HttpPostedFileBase`) for you to work with. When you're done with the files (for example, you saved them to a database), you can use the `DeleteFiles()` method to delete them from the temporary folder. If you don't, they will keep being returned when calling `GetFiles()` using the same reference.

	pluploadContext.DeleteFiles(reference);

`PluploadContext` also has a static method `CleanupFiles()` you could use on application startup, to cleanup the temporary `~/App_Data/PluploadMvc/` folder. When using the [NuGet package](https://www.nuget.org/packages/XperiCode.PluploadMvc) to install `PluploadMvc`, this is added to your application automatically by adding `PluploadMvcConfig.cs` to the `App_Start` folder and using [WebActivatorEx](https://www.nuget.org/packages/WebActivatorEx/2.0.0) to execute it on application startup.

    using XperiCode.PluploadMvc;
	// ...
    PluploadContext.CleanupFiles();

#### PluploadModelBinder

Examples of a full integration using a ViewModel and ModelBinder are coming, and this can already be found in the [sample project on GitHub](https://github.com/mwijnands/PluploadMvc/tree/master/PluploadMvc.Sample).

## (Unit) testing

When using `PluploadMvc`, you can still test your controllers. The `HttpContextBase` extension method `GetPluploadContext()` returns an `IPluploadContext`. To make this return a `Mock`, you can use the extension method `SetPluploadContext()` first to set your `Mock`, so that when `GetPluploadContext()` is called in the code under test, it will return your `Mock`. Examples of this can be found in the tests of this project on [GitHub](https://github.com/mwijnands/PluploadMvc).

## Release notes

#### v0.2.0

- Added support for [chunked uploads](http://plupload.com/docs/Chunking)

#### v0.1.1
- Changed Microsoft.AspNet.Mvc dependencies to 5.0.0 (5.2.0 was unnecessary)

#### v0.1.0

- Initial release

## Collaboration

Please report issues if you find any. Pull requests are welcome for documentation and code.
