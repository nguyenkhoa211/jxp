using System;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for CustomUmbracoApplication
/// </summary>
public class CustomUmbracoApplication : UmbracoApplication
{
    protected override IBootManager GetBootManager()
    {
        return new CustomWebBootManager(this);
    }
}
class CustomWebBootManager : WebBootManager
{
    public CustomWebBootManager(Umbraco.Core.UmbracoApplicationBase umbracoApplication) : base(umbracoApplication)
    {
    }

    public override Umbraco.Core.IBootManager Complete(Action<ApplicationContext> afterComplete)
    {
        RouteTable.Routes.MapUmbracoRoute("clientPortal", "userproducts/{action}/{id}",
            new { controller = "Products", id = UrlParameter.Optional }, new UmbracoVirtualNodeByIdRouteHandler(1373));
        return base.Complete(afterComplete);
    }
}