using Abp.Dependency;

namespace ThinknInsurTech.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}