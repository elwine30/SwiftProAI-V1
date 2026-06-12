using System.Collections.Generic;
using Abp.Dependency;
using ThinknInsurTech.Dto;

namespace ThinknInsurTech.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}