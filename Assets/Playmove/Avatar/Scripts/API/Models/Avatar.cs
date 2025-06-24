using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Playmove.Avatars.API.Models
{
    [Serializable]
    public class Avatar
    {
        [NonSerialized] private List<string> _elementsGUID;

        public List<Element> Elements { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        [JsonIgnore]
        public List<string> ElementsGUID
        {
            get
            {
                if (_elementsGUID == null)
                    _elementsGUID = Categories.Select(cat => cat.Element.GUID).ToList();
                return _elementsGUID;
            }
        }

        public Element GetElement(long categoryID)
        {
            return Elements.Find(cat => cat.CategoryId == categoryID);
        }

        public void SetElement(long categoryID, Element element)
        {
            var avatarElement = GetElement(categoryID);
            if (avatarElement != null)
                Elements.Remove(avatarElement);

            Elements.Add(avatarElement);
        }

        public override string ToString()
        {
            var output = "Categories => ";
            foreach (var cat in Categories)
                output += $"\nCategory: {cat.GUID}; Order: {cat.Order}; Element: {cat.Element.GUID};";
            return output;
        }
    }
}