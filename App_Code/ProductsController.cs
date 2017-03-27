using System;
using System.ComponentModel.DataAnnotations;
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
        ViewBag.UnitPress = new SelectList(Enum.GetValues(typeof(PressEnum))
                        .Cast<PressEnum>()
                        .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name");
        ViewBag.UnitDenG = new SelectList(Enum.GetValues(typeof(DenGEnum))
                        .Cast<DenGEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
        ViewBag.UnitViscG = new SelectList(Enum.GetValues(typeof(ViscGEnum))
                        .Cast<ViscGEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
        ViewBag.UnitDenL = new SelectList(Enum.GetValues(typeof(DenLEnum))
                        .Cast<DenLEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
        ViewBag.UnitViscL = new SelectList(Enum.GetValues(typeof(ViscLEnum))
                        .Cast<ViscLEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
        ViewBag.UnitFlowG = new SelectList(Enum.GetValues(typeof(QGasEnum))
                        .Cast<QGasEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
        ViewBag.UnitFlowL = new SelectList(Enum.GetValues(typeof(QLiqEnum))
                       .Cast<QLiqEnum>()
                       .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");

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
        ViewBag.Title = "Sizing Step 1";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;
        try
        {
            //Temp
            ConvertTemp(ref model);
            //Press
            ConvertPress(ref model);
            //Zcomp
            ConvertZComp(ref model);
            //DenG
            ConvertDenG(ref model);
            //ViscG
            ConvertViscG(ref model);
            //DenL
            ConvertDenL(ref model);
            //ViscL
            ConvertViscL(ref model);
            //QGas
            ConvertQGas(ref model);      
            //QLiq
            ConvertQLiq(ref model);

            //temporary
            ViewBag.UnitTemp = new SelectList(Enum.GetValues(typeof(TempEnum))
                            .Cast<TempEnum>()
                            .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name");
            ViewBag.UnitPress = new SelectList(Enum.GetValues(typeof(PressEnum))
                            .Cast<PressEnum>()
                            .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name");
            ViewBag.UnitDenG = new SelectList(Enum.GetValues(typeof(DenGEnum))
                            .Cast<DenGEnum>()
                            .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
            ViewBag.UnitViscG = new SelectList(Enum.GetValues(typeof(ViscGEnum))
                            .Cast<ViscGEnum>()
                            .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
            ViewBag.UnitDenL = new SelectList(Enum.GetValues(typeof(DenLEnum))
                            .Cast<DenLEnum>()
                            .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
            ViewBag.UnitViscL = new SelectList(Enum.GetValues(typeof(ViscLEnum))
                            .Cast<ViscLEnum>()
                            .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
            ViewBag.UnitFlowG = new SelectList(Enum.GetValues(typeof(QGasEnum))
                            .Cast<QGasEnum>()
                            .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");
            ViewBag.UnitFlowL = new SelectList(Enum.GetValues(typeof(QLiqEnum))
                           .Cast<QLiqEnum>()
                           .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name");

            return Index(model);
        }
        catch (Exception ex)
        {
            return RedirectToAction("SizingStep1", "Products", new { model, id = model.ProductId });
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
        //calculate Temp
        var cTemp = new ConvertTemp { CTemp = model.CTemp, UnitTemp = model.UnitTemp };
        var parsefTemp = fTemp.Replace("$", "cTemp.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cTemp", cTemp);
        var process = new CompiledExpression(parsefTemp)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.Temp = cTemp.Temp;
        model.TempR = cTemp.TempR;
    }

    private void ConvertPress(ref SizingStep1 model)
    {
        //load Formula Press
        var fPress = UmbracoContext.ContentCache.GetById(1388).GetProperty("cal").Value.ToString();
        //calculate Temp
        var cPress = new ConvertPress { CPress = model.CPress, UnitPress = model.UnitPress };
        var parsefPress = fPress.Replace("$", "cPress.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cPress", cPress);
        var process = new CompiledExpression(parsefPress)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.Press = cPress.Press;
    }
    private void ConvertZComp(ref SizingStep1 model)
    {
        //load Formula Zcomp
        var fZcomp = UmbracoContext.ContentCache.GetById(1389).GetProperty("cal").Value.ToString();
        //calculate Zcomp
        var cZcomp = new ConvertZcomp { CZcomp = model.CZcomp };
        var parsefZcomp = fZcomp.Replace("$", "cZcomp.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cZcomp", cZcomp);
        var process = new CompiledExpression(parsefZcomp)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.Zcomp = cZcomp.Zcomp;
    }

    private void ConvertDenG(ref SizingStep1 model)
    {
        //load Formula DenG
        var fDenG = UmbracoContext.ContentCache.GetById(1390).GetProperty("cal").Value.ToString();
        //calculate DenG
        var cDenG = new ConvertDenG { CDenG = model.CDenG, UnitDenG = model.UnitDenG, Temp = model.Temp, Press = model.Press, Zcomp = model.Zcomp };
        var parsefDenG = fDenG.Replace("$", "cDenG.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cDenG", cDenG);
        var process = new CompiledExpression(parsefDenG)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.DenG = cDenG.DenG;
        model.DenGSG = cDenG.DenGSG;
    }

    private void ConvertViscG(ref SizingStep1 model)
    {
        //load Formula ViscG
        var fViscG = UmbracoContext.ContentCache.GetById(1391).GetProperty("cal").Value.ToString();
        //calculate ViscG
        var cViscG = new ConvertViscG { CViscG = model.CViscG, UnitViscG = model.UnitViscG, DenG = model.DenG };
        var parsefViscG = fViscG.Replace("$", "cViscG.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cViscG", cViscG);
        var process = new CompiledExpression(parsefViscG)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.ViscG = cViscG.ViscG;
    }

    private void ConvertDenL(ref SizingStep1 model)
    {
        //load Formula DenL
        var fDenL = UmbracoContext.ContentCache.GetById(1392).GetProperty("cal").Value.ToString();
        //calculate DenL
        var cDenL = new ConvertDenL { CDenL = model.CDenL, UnitDenL = model.UnitDenL };
        var parsefDenL = fDenL.Replace("$", "cDenL.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cDenL", cDenL);
        var process = new CompiledExpression(parsefDenL)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.DenL = cDenL.DenL;
        model.DenLSG = cDenL.DenLSG;
    }

    private void ConvertViscL(ref SizingStep1 model)
    {
        //load Formula ViscL
        var fViscL = UmbracoContext.ContentCache.GetById(1393).GetProperty("cal").Value.ToString();
        //calculate ViscL
        var cViscL = new ConvertViscL { CViscL = model.CViscL, UnitViscL = model.UnitViscL, DenL = model.DenL };
        var parsefViscL = fViscL.Replace("$", "cViscL.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cViscL", cViscL);
        var process = new CompiledExpression(parsefViscL)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.ViscL = cViscL.ViscL;
    }

    private void ConvertQGas(ref SizingStep1 model)
    {
        //load Formula QGas
        var fQGas = UmbracoContext.ContentCache.GetById(1394).GetProperty("cal").Value.ToString();
        //calculate QGas
        var cQGas = new ConvertQGas { CQGas = model.CQGas, UnitFlowG = model.UnitFlowG, Temp = model.Temp, Press = model.Press, Zcomp = model.Zcomp, DenG = model.DenG, DenGSG = model.DenGSG, ViscG = model.ViscG, QGas = model.QGas, QGasACFM = model.QGasACFM, QGasACFS = model.QGasACFS };
        var parsefQGas = fQGas.Replace("$", "cQGas.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cQGas", cQGas);
        var process = new CompiledExpression(parsefQGas)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.QGas = cQGas.QGas;
        model.QGasACFM = cQGas.QGasACFM;
        model.QGasACFS = cQGas.QGasACFS;
    }

    private void ConvertQLiq(ref SizingStep1 model)
    {
        //load Formula QLiq
        var fQLiq = UmbracoContext.ContentCache.GetById(1395).GetProperty("cal").Value.ToString();
        //calculate QLiq
        var cQLiq = new ConvertQLiq { CQLiq = model.CQLiq, UnitFlowL = model.UnitFlowL, DenL = model.DenL};
        var parsefQLiq = fQLiq.Replace("$", "cQLiq.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cQLiq", cQLiq);
        var process = new CompiledExpression(parsefQLiq)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.QLiq = cQLiq.QLiq;
        model.QLiqACFM = cQLiq.QLiqACFM;
    }
}