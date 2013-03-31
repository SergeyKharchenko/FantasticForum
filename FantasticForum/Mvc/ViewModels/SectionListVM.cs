using System.Collections.Generic;
using Models;
using WebGrease.Css.Extensions;

namespace Mvc.ViewModels
{
    public class SectionListVM
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public Image Image { get; set; }

        public SectionListVM(Section section)
        {
            Id = section.Id;
            Title = section.Title;
            Image = section.Image;
        }
    }

    public static class EnumerableSectionExtension
    {
        public static List<SectionListVM> ToVMList(this IEnumerable<Section> sections)
        {
            var sectionListVMs = new List<SectionListVM>();
            sections.ForEach(section => sectionListVMs.Add(new SectionListVM(section)));
            return sectionListVMs;
        }
    }
}