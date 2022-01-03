using System.Linq;
using NUnit.Framework;
using ServiceStack.IO;
using ServiceStack.Testing;

namespace ServiceStack.WebHost.Endpoints.Tests;

public class ModuleTests
{
    private ServiceStackHost appHost;
    private IVirtualPathProvider ssResources;
    public ModuleTests()
    {
        appHost = new BasicAppHost().Init();
        ssResources = appHost.GetVirtualFileSources()
            .FirstOrDefault(x => x is ResourceVirtualFiles rvfs && rvfs.RootNamespace == nameof(ServiceStack));
    }

    [OneTimeTearDown] public void OneTimeTearDown() => appHost.Dispose();

    [Test]
    public void Can_search_modules_resources_folder()
    {
        var uiIndexFile = ssResources.GetFile("/modules/ui/index.html");
        Assert.That(uiIndexFile, Is.Not.Null);

        var componentFiles = ssResources.GetAllMatchingFiles("/modules/ui/components/*.html").ToList();
        Assert.That(componentFiles.Count, Is.GreaterThanOrEqualTo(7));
    }

    [Test]
    public void Tailwind_did_gen_properly()
    {
        var uiCss = ssResources.GetFile("/modules/ui/assets/ui.css");
        Assert.That(uiCss, Is.Not.Null);

        var uiCssContents = uiCss.ReadAllText();
        Assert.That(uiCssContents, Does.Contain("col-span-3"));
    }
}