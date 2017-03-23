using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for SizingController
/// </summary>
public class SizingStepController : RenderMvcController
{
    public ActionResult SizingStep1(RenderModel model)
    {
        ViewBag.Step = "Step 1";
        return Index(model);
    }

    public ActionResult SizingStep2(RenderModel model)
    {
        ViewBag.Step = "Step 2";
        return Index(model);
    }

    public ActionResult SizingStep3(RenderModel model)
    {
        ViewBag.Step = "Step 3";
        return Index(model);
    }
}