using System;
using System.Linq;
using System.Web.Mvc;
using Calculator;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using ExpressionEvaluator;

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
        return PartialView("~/Views/Partials/UserProductsWidget.cshtml", userProducts);
    }

    public ActionResult Detail(RenderModel model, string id)
    {
        ViewBag.Title = "Products";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;
        var product = string.IsNullOrEmpty(id) ? null : UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", id));

        return View("~/Views/UserProductsDetail.cshtml", product);
    }

    public ActionResult SizingStep1(RenderModel model, string id)
    {
        ViewBag.Title = "Sizing Step 1";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;

        ViewBag.UnitTemp = new SelectList(Enum.GetValues(typeof(TempEnum))
                                        .Cast<TempEnum>()
                                        .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name");

        var sizing = string.IsNullOrEmpty(id) ? null : new SizingStep1(model.Content);
        if (sizing != null)
        {
            var product = UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", id));
            if (product != null)
            {
                sizing.ProductId = id;
                sizing.ProductName = product.Name;
            }
        }
        return Index(sizing);
    }

    [HttpPost]
    public ActionResult SizingStep1(SizingStep1 model)
    {
        try
        {
            //Temp
            if (model.CTemp >= 0)
            {
                ConvertTemp(ref model);
            }

            //temporary
            ViewBag.UnitTemp = new SelectList(Enum.GetValues(typeof(TempEnum))
                                       .Cast<TempEnum>()
                                       .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name");
            return Index(model);
        }
        catch (Exception)
        {
            return RedirectToAction("SizingStep1", "Products", new {model, id = model.ProductId});
        }
    }

    public ActionResult SizingStep2(RenderModel model)
    {
        ViewBag.Title = "Sizing Step 2";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;

        return View("~/Views/SizingStep2.cshtml");
    }

    public ActionResult SizingStep3(RenderModel model)
    {
        ViewBag.Title = "Sizing Step 3";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;

        return View("~/Views/SizingStep3.cshtml");
    }

    private void ConvertTemp(ref SizingStep1 model)
    {
        //load Formula Temp, TempR
        var fTemp = UmbracoContext.ContentCache.GetById(1386).GetProperty("cal").Value.ToString();
        var fTempR = UmbracoContext.ContentCache.GetById(1387).GetProperty("cal").Value.ToString();
        //calculate Temp
        var cTemp = new ConvertTemp { CTemp = model.CTemp, UnitTemp = model.UnitTemp };
        var parsefTemp = fTemp.Replace("$", "cTemp.");
        var parsefTempR = fTempR.Replace("$", "cTemp.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cTemp", cTemp);
        var process = new CompiledExpression(parsefTemp)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //calculate TempR
        process = new CompiledExpression(parsefTempR)
        {
            TypeRegistry = reg
        };
        process.Compile();
        process.Eval();
        //Result
        model.ResultTemp = cTemp.Temp;
        model.ResultTempR = cTemp.TempR;
    }

}