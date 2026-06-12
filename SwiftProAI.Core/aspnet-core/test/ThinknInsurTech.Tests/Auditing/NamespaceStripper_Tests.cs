using ThinknInsurTech.Auditing;
using ThinknInsurTech.Test.Base;
using Shouldly;
using Xunit;

namespace ThinknInsurTech.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("ThinknInsurTech.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("ThinknInsurTech.Auditing.GenericEntityService`1[[ThinknInsurTech.Storage.BinaryObject, ThinknInsurTech.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("ThinknInsurTech.Auditing.XEntityService`1[ThinknInsurTech.Auditing.AService`5[[ThinknInsurTech.Storage.BinaryObject, ThinknInsurTech.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[ThinknInsurTech.Storage.TestObject, ThinknInsurTech.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
