using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

/// <summary>
/// Summary description for UserProductsModel
/// </summary>
public class UserProductsModel : RenderModel
{
    private readonly UmbracoHelper _umbraco;
    private const int CompanyNodeId = 1322;

    public UserProductsModel(IPublishedContent content, UmbracoHelper umraco) : base(content)
    {
        _umbraco = umraco;
    }

    public IPublishedContent Company
    {
        get
        {
            if (SessionManager.UserLogin == null) return null;
            var companyOverView = _umbraco.TypedContent(CompanyNodeId);
            if (companyOverView == null) return null;
            return companyOverView.Children.FirstOrDefault(x => x.Name.Equals(SessionManager.UserLogin.CompanyName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public int PageIndex { get; set; }

    public IEnumerable<int> ExistedProducts
    {
        get
        {
            if (Company == null) return null;
            return Company.GetPropertyValue<string>("assignedProducts").Split(',').Select(int.Parse);
        }
    }

    public IEnumerable<IPublishedContent> Products
    {
        get
        {
            if (ExistedProducts == null) return null;
            var existedProduct = ExistedProducts.Skip(0).Take(6).ToList();
            return _umbraco.TypedContent(existedProduct).ToList();
        }
    }

    public IEnumerable<IPublishedContent> this[int pageIndex]
    {
        get {
            if (ExistedProducts == null) return null;
            var existedProduct = ExistedProducts.Skip((pageIndex - 1) * 6).Take(6).ToList();
            return _umbraco.TypedContent(existedProduct).ToList();
        }
    }
}