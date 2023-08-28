using BeautySalon.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeautySalon.Components
{
    public class CategoryComponent : ViewComponent
    {
        private IHelpingServices _helpingServices;

        public CategoryComponent(IHelpingServices helpingServisse)
        {
            _helpingServices = helpingServisse;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("Category", _helpingServices.GetMainCategories()));
        }
    }
}
