using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Calculator;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using ExpressionEvaluator;
using Umbraco.Core.Models;
using Umbraco.Web;

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

    public ActionResult SizingStep1(RenderModel model, string id, string step)
    {
        if (string.IsNullOrEmpty(id))
        {
            return RedirectToAction("UserProducts", "Products");
        }
        ViewBag.Title = "Sizing Step 1";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;

        var sizing = !string.IsNullOrEmpty(step) ? GetSizingFromContent(step) : new SizingModel(model.Content);
        if (sizing != null)
        {
            var product = UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", id));
            if (product != null)
            {
                sizing.ProductId = id;
                sizing.ProductName = product.Name;
                sizing.SizingName = step;
            }
        }
        ViewBag.UnitTemp = new SelectList(Enum.GetValues(typeof(TempEnum))
                .Cast<TempEnum>()
                .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name", sizing == null ? string.Empty : sizing.UnitTemp);
        ViewBag.UnitPress = new SelectList(Enum.GetValues(typeof(PressEnum))
                        .Cast<PressEnum>()
                        .Select(s => new { name = s.ToString(), value = s.ToString() }), "value", "name", sizing == null ? string.Empty : sizing.UnitPress);
        ViewBag.UnitDenG = new SelectList(Enum.GetValues(typeof(DenGEnum))
                        .Cast<DenGEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitDenG);
        ViewBag.UnitViscG = new SelectList(Enum.GetValues(typeof(ViscGEnum))
                        .Cast<ViscGEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitViscG);
        ViewBag.UnitDenL = new SelectList(Enum.GetValues(typeof(DenLEnum))
                        .Cast<DenLEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitDenL);
        ViewBag.UnitViscL = new SelectList(Enum.GetValues(typeof(ViscLEnum))
                        .Cast<ViscLEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitViscL);
        ViewBag.UnitFlowG = new SelectList(Enum.GetValues(typeof(QGasEnum))
                        .Cast<QGasEnum>()
                        .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitFlowG);
        ViewBag.UnitFlowL = new SelectList(Enum.GetValues(typeof(QLiqEnum))
                       .Cast<QLiqEnum>()
                       .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.UnitFlowL);
        ViewBag.JHFSELEM = new SelectList(Enum.GetValues(typeof(ElemEnum))
                       .Cast<ElemEnum>()
                       .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.JHFSELEM);
        ViewBag.JHFSSEP = new SelectList(Enum.GetValues(typeof(SepEnum))
                       .Cast<SepEnum>()
                       .Select(s => new { name = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name, value = s.GetType().GetMember(s.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().First().Name }), "value", "name", sizing == null ? string.Empty : sizing.JHFSSEP);

        return Index(sizing);
    }

    [HttpPost]
    public ActionResult SizingStep1(SizingModel model)
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
            //Elem
            GetElem(ref model);
            //NumLem
            CalNumElem(ref model);

            //store Content
            var contentService = Services.ContentService;
            IContent contentSizing;
            if (string.IsNullOrEmpty(model.SizingName))
            {
                model.SizingName = Guid.NewGuid().ToString("n");
                contentSizing = contentService.CreateContent(model.SizingName, 1399, "sizingItem", SessionManager.UserLogin.UserId);
            }
            else
            {
                contentSizing = contentService.GetChildrenByName(1399, model.SizingName).FirstOrDefault();
            }
            if (contentSizing != null)
            {
                contentSizing.SetValue("userName", SessionManager.UserLogin.UserName);
                contentSizing.SetValue("productName", model.ProductId);
                contentSizing.SetValue("step", 1);
                contentSizing.SetValue("unitTemp", model.UnitTemp);
                contentSizing.SetValue("cTemp", model.CTemp);
                contentSizing.SetValue("temp", model.Temp);
                contentSizing.SetValue("tempR", model.TempR);
                contentSizing.SetValue("unitPress", model.UnitPress);
                contentSizing.SetValue("cPress", model.CPress);
                contentSizing.SetValue("press", model.Press);
                contentSizing.SetValue("unitDenG", model.UnitDenG);
                contentSizing.SetValue("cDenG", model.CDenG);
                contentSizing.SetValue("denG", model.DenG);
                contentSizing.SetValue("denGSG", model.DenGSG);
                contentSizing.SetValue("cZcomp", model.CZcomp);
                contentSizing.SetValue("zcomp", model.Zcomp);
                contentSizing.SetValue("unitViscG", model.UnitViscG);
                contentSizing.SetValue("cViscG", model.CViscG);
                contentSizing.SetValue("viscG", model.ViscG);
                contentSizing.SetValue("unitDenL", model.UnitDenL);
                contentSizing.SetValue("cDenL", model.CDenL);
                contentSizing.SetValue("denL", model.DenL);
                contentSizing.SetValue("denLSG", model.DenLSG);
                contentSizing.SetValue("unitViscL", model.UnitViscL);
                contentSizing.SetValue("cViscL", model.CViscL);
                contentSizing.SetValue("viscL", model.ViscL);
                contentSizing.SetValue("unitFlowG", model.UnitFlowG);
                contentSizing.SetValue("cQGas", model.CQGas);
                contentSizing.SetValue("qGas", model.QGas);
                contentSizing.SetValue("qGasACFM", model.QGasACFM);
                contentSizing.SetValue("qGasACFS", model.QGasACFS);
                contentSizing.SetValue("unitFlowL", model.UnitFlowL);
                contentSizing.SetValue("cQLiq", model.CQLiq);
                contentSizing.SetValue("qLiq", model.QLiq);
                contentSizing.SetValue("qLiqACFM", model.QLiqACFM);
                contentSizing.SetValue("jHFSELEM", model.JHFSELEM);
                contentSizing.SetValue("jHFSSEP", model.JHFSSEP);
                contentSizing.SetValue("kELEM", model.KELEM);
                contentSizing.SetValue("eLEMPRICE", model.ELEMPRICE);
                contentSizing.SetValue("eLEMWEIGHT", model.ELEMWEIGHT);
                contentSizing.SetValue("eLEMSUP", model.ELEMSUP);
                contentSizing.SetValue("eLEMOD", model.ELEMOD);
                contentSizing.SetValue("eLEMID", model.ELEMID);
                contentSizing.SetValue("eLEMLEN", model.ELEMLEN);
                contentSizing.SetValue("numElem", model.NumElem);

                contentService.Save(contentSizing);
            }

            TempData["step"] = model;
            return RedirectToAction("SizingStep2", "Products", new { id = model.ProductId, step = model.SizingName });
        }
        catch (Exception ex)
        {
            return RedirectToAction("SizingStep1", "Products", new { id = model.ProductId });
        }
    }

    public ActionResult SizingStep2(RenderModel model, string id, string step)
    {
        ViewBag.Title = "Sizing Step 2";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;
        if (TempData["step"] == null && string.IsNullOrEmpty(step))
        {
            return RedirectToAction("SizingStep1", "Products", new { id });
        }
        SizingModel sizing = null;
        if (TempData["step"] != null)
        {
            sizing = (SizingModel)TempData["step"];
        }
        else
        {
            sizing = GetSizingFromContent(step);
            var product = UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", id));
            if (product != null)
            {
                sizing.ProductId = id;
                sizing.ProductName = product.Name;
                sizing.SizingName = step;
            }
        }
        if (sizing == null)
        {
            return RedirectToAction("SizingStep1", "Products", new { id });
        }
        return Index(sizing);
    }

    [HttpPost]
    public ActionResult SizingStep2(SizingModel model)
    {
        return RedirectToAction("SizingStep3", "Products", new { id = model.ProductId });
    }

    public ActionResult SizingStep3(RenderModel model, string id)
    {
        ViewBag.Title = "Sizing Step 3";
        ViewBag.SiteName = model.Content.GetProperty("siteName", true).Value;

        return View("~/Views/SizingStep3.cshtml");
    }

    private void ConvertTemp(ref SizingModel model)
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

    private void ConvertPress(ref SizingModel model)
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
    private void ConvertZComp(ref SizingModel model)
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

    private void ConvertDenG(ref SizingModel model)
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

    private void ConvertViscG(ref SizingModel model)
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

    private void ConvertDenL(ref SizingModel model)
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

    private void ConvertViscL(ref SizingModel model)
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

    private void ConvertQGas(ref SizingModel model)
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

    private void ConvertQLiq(ref SizingModel model)
    {
        //load Formula QLiq
        var fQLiq = UmbracoContext.ContentCache.GetById(1395).GetProperty("cal").Value.ToString();
        //calculate QLiq
        var cQLiq = new ConvertQLiq { CQLiq = model.CQLiq, UnitFlowL = model.UnitFlowL, DenL = model.DenL };
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

    private void GetElem(ref SizingModel model)
    {
        //load Formula Elem
        var fElem = UmbracoContext.ContentCache.GetById(1396).GetProperty("cal").Value.ToString();
        //calculate Elem
        var cElem = new GetElem { JHFSELEM = model.JHFSELEM };
        var parsefElem = fElem.Replace("$", "cElem.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("cElem", cElem);
        var process = new CompiledExpression(parsefElem)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.ELEMID = cElem.ELEMID;
        model.ELEMLEN = cElem.ELEMLEN;
        model.ELEMOD = cElem.ELEMOD;
        model.ELEMPRICE = cElem.ELEMPRICE;
        model.ELEMSUP = cElem.ELEMSUP;
        model.ELEMWEIGHT = cElem.ELEMWEIGHT;
        model.KELEM = cElem.KELEM;
    }
    private void CalNumElem(ref SizingModel model)
    {
        //load Formula calElem
        var fcalElem = UmbracoContext.ContentCache.GetById(1550).GetProperty("cal").Value.ToString();
        //calculate calElem
        GetElemFromUser(SessionManager.UserLogin.UserName, SessionManager.UserLogin.CompanyName, ref model);
        var calElem = new CalNumElem { QGas = model.QGas, DenG = model.DenG, Press = model.Press, ELEMID = model.ELEMID, ViscG = model.ViscG, Temp = model.Temp, QLiq = model.QLiq, ZComp = model.Zcomp, KELEM = model.KELEM, MAXDPJHFSELEM = model.MAXDPJHFSELEM,MAXVJHFSELEM = model.MAXVJHFSELEM};
        var parsefcalElem = fcalElem.Replace("$", "calElem.");
        var reg = new TypeRegistry();
        reg.RegisterSymbol("calElem", calElem);
        var process = new CompiledExpression(parsefcalElem)
        {
            TypeRegistry = reg,
            ExpressionType = CompiledExpressionType.StatementList
        };
        process.Compile();
        process.Eval();
        //Result
        model.NumElem = calElem.NumElem;
    }

    private SizingModel GetSizingFromContent(string sizingName)
    {
        var sizing = new SizingModel();
        var content = Services.ContentService.GetChildrenByName(1399, sizingName).FirstOrDefault();
        if (content != null)
        {
            sizing.Step = content.GetValue<int>("step");
            sizing.UnitTemp = content.GetValue<string>("unitTemp");
            sizing.CTemp = content.GetValue<double>("cTemp");
            sizing.Temp = content.GetValue<double>("temp");
            sizing.TempR = content.GetValue<double>("tempR");
            sizing.UnitPress = content.GetValue<string>("unitPress");
            sizing.CPress = content.GetValue<double>("cPress");
            sizing.Press = content.GetValue<double>("press");
            sizing.UnitDenG = content.GetValue<string>("unitDenG");
            sizing.CDenG = content.GetValue<double>("cDenG");
            sizing.DenG = content.GetValue<double>("denG");
            sizing.DenGSG = content.GetValue<double>("denGSG");
            sizing.CZcomp = content.GetValue<double>("cZcomp");
            sizing.Zcomp = content.GetValue<double>("zcomp");
            sizing.UnitViscG = content.GetValue<string>("unitViscG");
            sizing.CViscG = content.GetValue<double>("cViscG");
            sizing.ViscG = content.GetValue<double>("viscG");
            sizing.UnitDenL = content.GetValue<string>("unitDenL");
            sizing.CDenL = content.GetValue<double>("cDenL");
            sizing.DenL = content.GetValue<double>("denL");
            sizing.DenLSG = content.GetValue<double>("denLSG");
            sizing.UnitViscL = content.GetValue<string>("unitViscL");
            sizing.CViscL = content.GetValue<double>("cViscL");
            sizing.ViscL = content.GetValue<double>("viscL");
            sizing.UnitFlowG = content.GetValue<string>("unitFlowG");
            sizing.CQGas = content.GetValue<double>("cQGas");
            sizing.QGas = content.GetValue<double>("qGas");
            sizing.QGasACFM = content.GetValue<double>("qGasACFM");
            sizing.QGasACFS = content.GetValue<double>("qGasACFS");
            sizing.UnitFlowL = content.GetValue<string>("unitFlowL");
            sizing.CQLiq = content.GetValue<double>("cQLiq");
            sizing.QLiq = content.GetValue<double>("qLiq");
            sizing.QLiqACFM = content.GetValue<double>("qLiqACFM");
            sizing.JHFSELEM = content.GetValue<string>("jHFSELEM");
            sizing.KELEM = content.GetValue<double>("kELEM");
            sizing.ELEMPRICE = content.GetValue<double>("eLEMPRICE");
            sizing.ELEMWEIGHT = content.GetValue<double>("eLEMWEIGHT");
            sizing.ELEMSUP = content.GetValue<string>("eLEMSUP");
            sizing.ELEMOD = content.GetValue<double>("eLEMOD");
            sizing.ELEMID = content.GetValue<double>("eLEMID");
            sizing.ELEMLEN = content.GetValue<double>("eLEMLEN");
            sizing.JHFSSEP = content.GetValue<string>("jHFSSEP");
            sizing.NumElem = content.GetValue<double>("numElem");
        }
        return sizing;
    }

    private void GetElemFromUser(string userName, string companyName, ref SizingModel model)
    {
        var content = UmbracoContext.ContentCache.GetSingleByXPath(string.Format(@"/root/descendant-or-self::*[@urlName='{0}']", userName));
        if (content != null && string.Equals(content.GetProperty("company").Value.ToString(), companyName, StringComparison.OrdinalIgnoreCase))
        {
            double temp;
            if (double.TryParse(content.GetProperty("mAXVJHFSELEM").Value.ToString(), out temp))
            {
                model.MAXVJHFSELEM = temp;
            }
            if (double.TryParse(content.GetProperty("mAXDPJHFSELEM").Value.ToString(), out temp))
            {
                model.MAXDPJHFSELEM = temp;
            }
            if (double.TryParse(content.GetProperty("mAXK2JHFS").Value.ToString(), out temp))
            {
                model.MAXK2JHFS = temp;
            }
            if (double.TryParse(content.GetProperty("mAXK3JHFS").Value.ToString(), out temp))
            {
                model.MAXK3JHFS = temp;
            }
            if (double.TryParse(content.GetProperty("mAXRV2JHFS").Value.ToString(), out temp))
            {
                model.MAXRV2JHFS = temp;
            }
        }
    }
}