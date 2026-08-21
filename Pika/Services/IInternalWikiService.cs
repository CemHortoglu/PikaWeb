using System.Collections.Generic;
using Pika.Models;

namespace Pika.Services
{
    public interface IInternalWikiService
    {
        WikiData GetWikiData();
        WikiPage? GetPage(string slug);
        List<WikiCategory> GetCategories();
        List<WikiPageSummary> GetAllPages();
        WikiHomeViewModel GetHomeViewModel();
        WikiArticleViewModel? GetArticleViewModel(string slug);
        List<WikiPageSummary> Search(string query);
    }
}
