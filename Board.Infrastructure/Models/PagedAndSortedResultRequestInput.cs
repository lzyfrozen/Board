using Board.ToolKits;
using System.ComponentModel.DataAnnotations;

namespace Board.Infrastructure.Models
{
    public class PagedAndSortedResultRequestInput
    {
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        [Range(0, int.MaxValue)]
        public int MaxResultCount { get; set; }

        public string Sort { get; set; }

        public string Keyword { get; set; }

        private string sorting;
        public string Sorting
        {
            get
            {
                if (sorting.HasValue())
                {
                    return sorting;
                }
                if (Sort.HasValue())
                {
                    var sorts = Sort.ToObject<List<SortItem>>();
                    if (sorts != null && sorts.Count > 0)
                    {
                        sorting = string.Join(",", sorts.Select(p => p.Property + " " + p.Direction.ToUpper()));
                    }
                    else
                    {
                        sorting = GetDefaultSorting();
                    }
                }
                else
                {
                    sorting = GetDefaultSorting();
                }
                return sorting;
            }
            set
            {
                sorting = value;
            }
        }

        public virtual void Normalize()
        {
            if (Sort.HasValue())
            {
                var sorts = Sort.ToObject<List<SortItem>>();
                if (sorts != null && sorts.Count > 0)
                {
                    Sorting = string.Join(",", sorts.Select(p => p.Property + " " + p.Direction.ToUpper()));
                }
                else
                {
                    Sorting = GetDefaultSorting();//Sort;
                }
            }
            else
            {
                if (!sorting.HasValue())
                {
                    Sorting = GetDefaultSorting();
                }
            }
        }

        string GetDefaultSorting()
        {
            return "CreationTime DESC";
        }
    }
}
