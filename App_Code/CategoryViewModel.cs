using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CategoryViewModel
/// </summary>
public class CategoryViewModel
{
    public CategoryViewModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string CategoryName { get; set; }
    public string CategoryUrl { get; set; }

    public List<SubCategoryViewModel> SubCategories { get; set; }
}