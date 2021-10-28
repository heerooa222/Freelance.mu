using System;

namespace Services.Freelancer.Utility
{
    public class PageUtility
    {
        public static string MakePagination(int affichageParPage, int nombreResultat, int pageActuel, string url)
        {
            var nombreDePage = Convert.ToInt32(nombreResultat / affichageParPage);
            if (nombreResultat == 0 || nombreDePage < 2)
            {
                return "";
            }

            var tabIndex = "tabindex='-1'";
            var numeroPaginationDom = "";
            var prevDom = "";
            var nextDom = "";
            var classLi = "page-item";
            var classA = "page-link";
            var active = " active";
            var disabled = " disabled";
            var classPrev = classLi;
            var hrefPrev = $"{url}{(pageActuel - 1)}";
            var attribute = "";
            var href = $"href = '{hrefPrev}'";
            if (pageActuel <= 1) { classPrev = $"{classPrev}{disabled}"; attribute = tabIndex; href = ""; }
            prevDom =
                $"<li class='{classPrev}'><a class='{classA}' {href} {attribute} aria-label='Previous'><span aria-hidden='true'>&laquo;</span><span class='sr-only'>Previous</span></a></li>";
            if (nombreResultat <= affichageParPage)
            {
                var li = $"<li class='{classLi}{active}'><a class='{classA}' href='{url}1'>1</a></li>";
                numeroPaginationDom = li;
            }
            if (nombreResultat > affichageParPage)
            {
                numeroPaginationDom = "";
                if ((nombreResultat % affichageParPage) != 0) nombreDePage++;
                if ((nombreDePage) <= 5)
                {
                    for (var i = 1; i <= nombreDePage; i++)
                    {
                        var classListe = classLi;
                        if (i == pageActuel) classListe = $"{classListe}{active}";
                        var li = $"<li class='{classListe}'><a class='{classA}' href='{url}{i}'>{i}</a></li>";
                        numeroPaginationDom = $"{numeroPaginationDom}{li}";
                    }
                }
                if ((nombreDePage) > 5)
                {
                    if (IsLastTwoOrFirstTwo(pageActuel, nombreDePage))
                    {
                        string[] numpage = { "1", "2", "...", $"{(nombreDePage - 1)}", $"{nombreDePage}"};
                        string[] class_num = { classLi, classLi, $"{classLi}{disabled}", classLi, classLi };
                        for (var i = 0; i < numpage.Length; i++)
                        {
                            var classe = class_num[i];
                            attribute = "";
                            href = $"href='{url}{numpage[i]}'";
                            if (class_num[i].Contains("disabled")) { attribute = tabIndex; href = ""; }
                            if (numpage[i].Equals(pageActuel.ToString())) classe = $"{classe}{active}";
                            var li =
                                $"<li class='{classe}'><a class='{classA}' {attribute} {href}>{numpage[i]}</a></li>";
                            numeroPaginationDom = $"{numeroPaginationDom}{li}";
                        }
                    }
                    else
                    {
                        string[] numpage = { "1", "...", $"{pageActuel}", "...", $"{nombreDePage}"};
                        string[] class_num = { classLi, $"{classLi}{disabled}", $"{classLi}{active}",
                            $"{classLi}{disabled}", classLi };
                        for (var i = 0; i < numpage.Length; i++)
                        {
                            var classe = class_num[i];
                            href = $"href='{url}{numpage[i]}'";
                            if (class_num[i].Contains("disabled")) { href = ""; }
                            var li = $"<li class='{classe}'><a class='{classA}' {href}>{numpage[i]}</a></li>";
                            numeroPaginationDom = $"{numeroPaginationDom}{li}";
                        }
                    }
                }
            }
            var class_next = "page-item";
            var href_next = $"{url}{(pageActuel + 1)}";
            attribute = "";
            href = $"href = '{href_next}'";
            if (pageActuel >= nombreDePage) { class_next = "page-item disabled"; attribute = tabIndex; href = ""; }
            nextDom =
                $"<li class='{class_next}'><a class='{classA}' {attribute} {href}><span aria-hidden='true'>&raquo;</span><span class='sr-only'>Next</span></a></li>";
            return $"{prevDom}{numeroPaginationDom}{nextDom}";
        }
        private static bool IsLastTwoOrFirstTwo(int currentPage, int numberOfPage)
        {
            return currentPage <= 2 || Math.Abs(numberOfPage - currentPage) <= 1;
        }
    }
}