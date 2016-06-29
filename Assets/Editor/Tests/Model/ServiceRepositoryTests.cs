using UnityEngine;
using NUnit.Framework;
using Service;
using System.Collections.Generic;

public class ServiceRepositoryTests
{
    private interface TestService : SharedService
    {
        string Value { get; }
    }

    private class FooService : TestService
    {
        public string Value { get { return "Foo"; } }
    }

    private class BarService : TestService
    {
        public string Value { get { return "Bar"; } }
    }

    [Test]
    public void RegisteredTypesAreReturned()
    {
        var repository = new ServiceRepository();

        repository.Register<TestService>(typeof(FooService));
        var service = repository.Get<TestService>();

        Assert.AreEqual("Foo", service.Value);
    }

    [Test]
    public void OnlyOneInstanceIsCreated()
    {
        var repository = new ServiceRepository();

        repository.Register<TestService>(typeof(FooService));

        var first = repository.Get<TestService>();
        var second = repository.Get<TestService>();

        Assert.AreEqual(first, second);
    }

    [Test]
    public void DeserializedRegistrationOfServices()
    {
        var json = string.Format(
            "{{\"registry\": [{{\"interfaceName\": \"{0}\", \"className\": \"{1}\"}}]}}",
            typeof(TestService).AssemblyQualifiedName,
            typeof(BarService).AssemblyQualifiedName
        );

        var repository = new ServiceRepository();
        repository.RegisterFromJson(json);

        var service = repository.Get<TestService>();

        Assert.AreEqual("Bar", service.Value);
    }
}
