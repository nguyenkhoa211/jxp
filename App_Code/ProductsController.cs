using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for ProductsController
/// </summary>
[CustomAuthorize]
public class ProductsController : RenderMvcController
{
    public ActionResult UserProducts(RenderModel model, int pageIndex = 1)
    {
        var userProducts = new UserProductsModel(model.Content, Umbraco) { PageIndex = pageIndex };
        if (pageIndex == 1)
        {
            ViewBag.Title = model.Content.GetProperty("title").Value;
            ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;
            return Index(userProducts);
        }
        else
        {
            return PartialView("~/Views/Partials/UserProductsWidget.cshtml", userProducts);
        }
    }

    public ActionResult Detail(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        var product = UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", id));
        if (product != null)
        {
            ViewBag.Title = "Products";
            ViewBag.SiteName = product.GetProperty("siteName", true).Value;
        }
        return View("~/Views/UserProductsDetail.cshtml", product);
    }

    public ActionResult SizingStep1(string id)
    {
        ViewBag.Step = id;
        return View("~/Views/SizingStep1.cshtml");
    }

    [HttpPost]
    public ActionResult SizingStep1()
    {
        ViewBag.Step = "Step 1";
        return View("~/Views/SizingStep1.cshtml");
    }
    public ActionResult SizingStep2(RenderModel model)
    {
        ViewBag.Step = "Step 2";
        return View("~/Views/SizingStep2.cshtml");
    }

    public ActionResult SizingStep3(RenderModel model)
    {
        ViewBag.Step = "Step 3";
        return View("~/Views/SizingStep3.cshtml");
    }
}