using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System.Collections.Generic;

namespace SYMB2C.Foundation.Common
{
    public interface ISitecoreHelper
    {
        T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class;
        //List<T> GetChildren<T>(string Id, bool isLazy = false, bool inferType = false) where T : List<T>;
        IEnumerable<Item> GetCompositeItems(Item datasourceItem);
        ImageField ImageUrlGeneric(Item item, string imageField, MediaUrlOptions options = null);
        string ImageUrl(ImageField imageField, MediaUrlOptions options = null);
        Database FetchDB();
        void PublishItem(Item item, bool includeChildren);
        Item CreateItem(string ParentItemPath, string curItemName, string templatePath);
        Item CreateItem(Item parentItem, string curItemName, string templatePath);
        void RemoveLinkReferences(Item item);
        string GetValidItemName(string name);
    }
}