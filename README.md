# Yawat
Yet another WebApi tester

***

**This is currently a work in progress.**

If you find any bug or would like to implement a missing feature, pull requests are welcome.

***

# Introduction

When developing a WebApi you will eventually come to the point where you have to test your code.

You may use elaborated tools like Postman or JMeter for this task.
You get an UI where you are guided through the process of writing tests.
These tools allow you to test nearly any possible WebApi configuration.

But the debugging experience is printf-like, and 
versioning these tests may be hard, since they are stored in a proprietary format.

And sometimes, especially when you are a .NET developer, you may wish to write these tests with C#.

This is what **Yawat** was developed for.

**Yawat** is a .NET Standard2.1 library that can be used with NUnit (or other test frameworks) to run tests against a WebApi.
And you can use Visual Studio to debug Yawat tests.

# Sample

A simple test might look like this:

```
    using System.Collections.Generic;
    using NUnit.Framework;
    
    using Yawat;
    using Yawat.ResponseData;

    [TestFixture]
    public class ToDoRequests
    {
        private const string BaseRoute = "/api/TodoItems";

        [Test]
        public void ShouldGetJsonFromGetAll()
        {
            var options = new YawatOptions(typeof(JsonResponseData))
            {
                Protocol = "https",
                Host = "localhost",
                Port = "44328"
            };

            var httpClient = new YawatHttpClient(options);
            var json = httpClient.Get($"{BaseRoute}").AsString();

            Assert.AreNotEqual(json, string.Empty);
        }
    }
```

---

**Note**: `options` and `httpClient` should normally be created within the global setup-teardown method.<br/>
See https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimesetup.html for details or <br/>
https://github.com/helmuttheis/Yawat/blob/master/Yawat.Samples/Basic/SetupTeardown.cs for a sample.

---

# ToDos

There are a lot of unfinished or even missing features.<br/>
Especially all sections marked with **[*]** required more work.

For more details see [todos.md](todos.md)

# Build all

Clone the repository and run **buildAll.bat** from within the Yawat directory.<br/>
It is expected that the dotnet CLI is installed.

# Run the servers

## Visual Studio

You may run the servers from the solution Yawat.Servers within Visual Studio.<br/>
Make sure to run both projects.


## [*]Docker

Use **runDocker.bat** to run both servers with in Docker.

**Note**: Currently it is not possible to use HTTPS to access the docker containers.

**Note**: Currently it is not possible to use OKTA or IdentityServer authentication within the docker container.


# Solutions and projects

For a description of the solutions and projects see [solutions.md](solutions.md)
