using BeautySalon.Models.Entities;
using BeautySalon.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BeautySalon.Models.Services.Interfaces
{
    public interface IHelpingServices
    {
        List<Job> GetMainCategories();
        List<SubJob> GetSubCategories(int id);
        void NoneActiveFormerDays(DateTime n);
        void NoneActiveFormerTimes(DateTime n, TimeSpan ts);
        List<SelectListItem> GetPersonel(int id);
        List<SelectListItem> GetDay(int id);
        List<SelectListItem> GetWorkingTimeDueToPersonel(int id, int dayId);
        List<SelectListItem> GetServices();
        string GetUserFullName(int id);
        bool IsThisUserAnAdmin(string phone);
    }
}
