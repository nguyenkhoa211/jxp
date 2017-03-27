using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

/// <summary>
/// Summary description for UserProductsModel
/// </summary>
public class SizingModel
{

}

public class SizingStep1 : RenderModel
{
    public SizingStep1() : base(UmbracoContext.Current.ContentCache.GetById(1373))
    {
        
    }
    public SizingStep1(IPublishedContent content) : base(content)
    {
    }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string UnitTemp { get; set; }
    public decimal CTemp { get; set; }
    public decimal ResultTemp { get; set; }
    public decimal ResultTempR { get; set; }

}

public class SizingStep2
{

}