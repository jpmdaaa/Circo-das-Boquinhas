using System;
using System.Collections.Generic;
using System.Linq;
using Playmove.Avatars.API.Vms;
using Playmove.Core;
using Playmove.Core.API.Models;

namespace Playmove.Avatars.API.Models
{
    public interface ICategorySavable
    {
        string GUID { get; }
        Element Element { get; set; }
    }

    public interface ICategory : ICategorySavable
    {
        int Order { get; }
    }

    [Serializable]
    public class Category : VmItem<CategoriaVm>, IDatabaseItem, ICategory
    {
        public long? DefaultElementId { get; set; }
        public ElementoVm DefaultElement { get; set; }
        public List<CategoriaLocalizacaoVm> Localizations { get; set; }
        public List<ElementoVm> Elements { get; set; }


        public string Title
        {
            get
            {
                var localized = Localizations.Find(loc => loc.Localizacao.ToLower() == GameSettings.Language.ToLower());
                return localized.Descricao;
            }
            set { }
        }

        public string GUID { get; set; }
        public int Order { get; set; }

        public Element Element
        {
            get
            {
                if (Elements.Count == 0)
                    Elements.Add(new ElementoVm());
                return GetElement(Elements[0]);
            }
            set
            {
                if (Elements.Count == 0)
                    Elements.Add(new ElementoVm());
                Elements[0] = value.GetVm();
            }
        }

        public List<Element> GetElements()
        {
            var _elements = new List<Element>();
            foreach (var elem in Elements)
                _elements.Add(new Element
                {
                    GUID = elem.Guid,
                    AppliedGUID = elem.AppliedGUID,
                    ThumbnailGUID = elem.ThumbnailGUID,
                    Id = elem.Id,
                    CategoryId = elem.CategoriaId
                });
            return _elements;
        }

        public Element GetElement(ElementoVm elem)
        {
            var _element = new Element
            {
                GUID = elem.Guid,
                AppliedGUID = elem.AppliedGUID,
                ThumbnailGUID = elem.ThumbnailGUID,
                Id = elem.Id,
                CategoryId = elem.CategoriaId
            };
            return _element;
        }

        public override CategoriaVm GetVm()
        {
            return new CategoriaVm
            {
                Id = Id,
                DefaultElement = DefaultElement,
                DefaultElementId = DefaultElementId,
                Elementos = Elements,
                Ordem = Order,
                Guid = GUID
            };
        }

        public override void SetDataFromVm(CategoriaVm vm)
        {
            Id = vm.Id;
            Order = vm.Ordem;
            Localizations = vm.Localizacoes.ToList();
            GUID = vm.Guid;
            Elements = vm.Elementos.ToList();
            DefaultElement = vm.DefaultElement;
            DefaultElementId = vm.DefaultElementId;
        }

        public override string ToString()
        {
            //return base.ToString();
            return
                $"{GUID} => Title: {Title}; Order: {Order}; Elements: {string.Join(", ", Elements.Select(elem => elem.Guid))}";
        }
    }
}