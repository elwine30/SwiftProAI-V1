
namespace ThinknInsurTech.Common.Dtos
{
    public class CommonDropdownDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class CommonAdjusterDropdownDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class CommonHospitalDropdownDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }
    }
}
