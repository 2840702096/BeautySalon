using BeautySalon.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeautySalon.Components
{
    public class BlogCategoryComponent : ViewComponent
    {
        private IHelpingServices _helpingServices;

        public BlogCategoryComponent(IHelpingServices helpingServices)
        {
            _helpingServices = helpingServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("BlogCategory", _helpingServices.GetMainCategories()));
        }
    }
}
