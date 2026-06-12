using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinknInsurTech.MultiTenancy.HostDashboard.Dto;

namespace ThinknInsurTech.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}