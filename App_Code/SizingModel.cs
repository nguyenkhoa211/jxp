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
        CZcomp = 1;
    }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string UnitTemp { get; set; }
    public double CTemp { get; set; }
    public double Temp { get; set; }
    public double TempR { get; set; }
    public string UnitPress { get; set; }
    public double CPress { get; set; }
    public double Press { get; set; }
    public string UnitDenG { get; set; }
    public double CDenG { get; set; }
    public double DenG { get; set; }
    public double DenGSG { get; set; }
    public double CZcomp { get; set; }
    public double Zcomp { get; set; }
    public string UnitViscG { get; set; }
    public double CViscG { get; set; }
    public double ViscG { get; set; }
    public string UnitDenL { get; set; }
    public double CDenL { get; set; }
    public double DenL { get; set; }
    public double DenLSG { get; set; }
    public string UnitViscL { get; set; }
    public double CViscL { get; set; }
    public double ViscL { get; set; }
    public string UnitFlowG { get; set; }
    public double CQGas { get; set; }
    public double QGas { get; set; }
    public double QGasACFM { get; set; }
    public double QGasACFS { get; set; }
    public string UnitFlowL { get; set; }
    public double CQLiq { get; set; }
    public double QLiq { get; set; }
    public double QLiqACFM { get; set; }
}

public class SizingStep2
{

}