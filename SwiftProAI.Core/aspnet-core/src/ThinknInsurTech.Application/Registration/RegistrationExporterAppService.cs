using Abp.AspNetZeroCore.Net;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThinknInsurTech.Common.Dtos;
using ThinknInsurTech.Dto;
using ThinknInsurTech.Integration;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Storage;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace ThinknInsurTech.Registration
{
    public class RegistrationExporterAppService : ThinknInsurTechAppServiceBase
    {
        private readonly IRepository<CaseInsurer> _caseInsurerRepository;
        private readonly IRepository<CaseInsuredPerson> _insuredPersonRepository;
        private readonly IRepository<CaseIncidentDetail> _caseIncidentDetailRepository;
        private readonly IRepository<CaseThirdPartyVehicle> _caseThirdPartyVehicleRepository;
        private readonly IRepository<CasePoliceReport> _casePoliceReportRepository;
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly FileOrgService _fileOrgService;

        public RegistrationExporterAppService(
            IRepository<CaseInsurer> caseInsurerRepository,
            IRepository<CaseInsuredPerson> insuredPersonRepository,
            IRepository<CaseIncidentDetail> caseIncidentDetailRepository,
            IRepository<CaseThirdPartyVehicle> caseThirdPartyVehicleRepository,
            IRepository<CasePoliceReport> casePoliceReportRepository,
            IRepository<MainRegistration, int> mainRegistrationRepository,
            ITempFileCacheManager tempFileCacheManager,
            FileOrgService fileOrgService)
        {
            _caseInsurerRepository = caseInsurerRepository;
            _insuredPersonRepository = insuredPersonRepository;
            _caseIncidentDetailRepository = caseIncidentDetailRepository;
            _caseThirdPartyVehicleRepository = caseThirdPartyVehicleRepository;
            _casePoliceReportRepository = casePoliceReportRepository;
            _mainRegistrationRepository = mainRegistrationRepository;
            _tempFileCacheManager = tempFileCacheManager;
            _fileOrgService = fileOrgService;
        }

        public async Task<FileDto> PostExportRegistration(int registerId)
        {

            // Get the location of the currently executing assembly
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            // Get the directory of the assembly
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            // Combine the assembly directory with the template file name
            string templateLocation = Path.Combine(assemblyDirectory, "DataExporting/Docx/Templates/", "RegistrationTemplate.docx");

            var item = new RegistrationExportItemDto();

            //Case Registration
            var mainRegistrationDB = _mainRegistrationRepository.GetAll()
                .Where(x => x.Id.Equals(registerId));
            var mainRegistrationItem = await mainRegistrationDB.ToArrayAsync();
            var mainRegistration = new MainRegistrationDocData();
            foreach (var c in mainRegistrationItem)
            {
                mainRegistration.Id = c.Id;
                mainRegistration.VehicleNo = c.VehicleNo;
            }
            item.MainRegistration = mainRegistration;

            //INSURERS DATA
            var insurersData = new InsurersExportData();
            var insurerDB = _caseInsurerRepository.GetAll()
                .Include(x => x.CompanyFk)
                .Where(x => x.RegisterId.Equals(registerId));

            var insurerItem = await insurerDB.ToArrayAsync();
            foreach (var c in insurerItem)
            {
                insurersData.Name = c.Name;
                insurersData.ReferenceNo = c.ReferenceNo;
                if(c.CompanyFk!=null)
                {
                    insurersData.CompanyName = c.CompanyFk.Name;
                    insurersData.CompanyAddress = c.CompanyFk.Address;
                }
                else
                {
                    insurersData.CompanyName= null;
                    insurersData.CompanyAddress= null;
                }
                
            }
            item.Insurers = insurersData;

            //INSURED PERSON OWNER
            var insuredPersonOwnerDB = _insuredPersonRepository.GetAll()
                .Where(x => x.RegisterId.Equals(registerId))
                .Where(x => x.IsOwner.Equals(true));

            var insuredPersonOwnerItem = await insuredPersonOwnerDB.ToArrayAsync();
            var insuredPersonOwner = new InsuredPersonOwnerDocData();
            foreach (var c in insuredPersonOwnerItem)
            {
                insuredPersonOwner.Coverage = c.Coverage;
                insuredPersonOwner.Name = c.Name;
                insuredPersonOwner.PolicyNo = c.PolicyNo;
                insuredPersonOwner.isOwner = c.IsOwner;
                insuredPersonOwner.isDriver = c.IsDriver;
                insuredPersonOwner.Relationship = c.Relationship;
                insuredPersonOwner.Vehicle = c.Make + " " + c.Model + " " + c.Specs;
                insuredPersonOwner.NRIC = c.IdenticationNo;
                insuredPersonOwner.Address = c.Address;
                insuredPersonOwner.Contact = c.Contact;
                insuredPersonOwner.Occupation = c.Occupation;
                insuredPersonOwner.ICFront = c.DriverICFront;
                insuredPersonOwner.ICBack = c.DriverICBack;
                insuredPersonOwner.DriverLicenseFront = c.DriverLicenseFront;
                insuredPersonOwner.DriverLicenseBack = c.DriverLicenseBack;
            }
            item.InsuredPersonOwner = insuredPersonOwner;

            //INSURED PERSON DRIVER
            var insuredPersonDriverDB = _insuredPersonRepository.GetAll()
                .Where(x => x.RegisterId.Equals(registerId))
                .Where(x => x.IsDriver.Equals(true));
            var insuredPersonDriverItem = await insuredPersonDriverDB.ToArrayAsync();
            var insuredPersonDriver = new InsuredPersonDriverDocData();
            foreach (var c in insuredPersonDriverItem)
            {
                insuredPersonDriver.Name = c.Name;
                insuredPersonDriver.isDriver = c.IsDriver;
                insuredPersonDriver.Vehicle = c.Make + " " + c.Model + " " + c.Specs;
                insuredPersonDriver.NRIC = c.IdenticationNo;
                insuredPersonDriver.Address = c.Address;
                insuredPersonDriver.ContactNo = c.Contact;
                insuredPersonDriver.EmployerName = c.EmployerName;
                insuredPersonDriver.Coverage = c.Coverage;
                insuredPersonDriver.LicenseDateFrom = c.LicenseDateFrom;
                insuredPersonDriver.LicenseDateTo = c.LicenseDateTo;
                insuredPersonDriver.LicenseNo = c.LicenseNo;
                insuredPersonDriver.DrivingExp = c.DrivingExperience;
                insuredPersonDriver.EmployerAddress = c.EmployerAddress;
                insuredPersonDriver.MonthlyIncome = (decimal)c.MonthlyIncome;
                insuredPersonDriver.ICFront = c.DriverICFront;
                insuredPersonDriver.ICBack = c.DriverICBack;
                insuredPersonDriver.DriverLicenseFront = c.DriverLicenseFront;
                insuredPersonDriver.DriverLicenseBack = c.DriverLicenseBack;
            }
            item.InsuredPersonDriver = insuredPersonDriver;

            //CASE INCIDENT DETAILS
            var incidentDetailsDB = _caseIncidentDetailRepository.GetAll()
                .Include(x => x.RegisterFk)
                .Where(x => x.RegisterId.Equals(registerId));
            var incidentDetailsItem = await incidentDetailsDB.ToArrayAsync();
            var incidentDetails = new IncidentDetailsDocData();
            foreach (var c in incidentDetailsItem)
            {
                incidentDetails.TimeFrom = c.TimeFrom;
                incidentDetails.TimeTo = c.TimeTo;
                incidentDetails.VehicleNo = c.RegisterFk.VehicleNo;
                incidentDetails.DirectionTo = c.DirectionTo;
                incidentDetails.DirectionFrom = c.DirectionFrom;
                incidentDetails.DrivingWith = c.DriverDrivingWith;
                incidentDetails.SpeedPrior = c.SpeedPrior.ToString();
                incidentDetails.SiteOfAccident = c.SiteOfAccident;
                incidentDetails.TypeOfRoad = c.TypeOfRoad;
                incidentDetails.WidthOfRoad = c.WidthOfRoad.ToString();
                incidentDetails.CenterDemarcation = c.CenterDemarcation;
                incidentDetails.DriverPathRight = c.DriverPathRight;
                incidentDetails.DriverPathLeft = c.DriverPathLeft;
                incidentDetails.ViewOfRoad = c.ViewOfRoad;
                incidentDetails.Visibility = c.Visibility;
                incidentDetails.SurroundingArea = c.SurroundingArea;
                incidentDetails.RoadCondition = c.RoadCondition;
                incidentDetails.WeatherCondition = c.WeatherCondition;
            }
            item.IncidentDetails = incidentDetails;

            //CASE POLICE REPORT
            var casePoliceReportDB = _casePoliceReportRepository.GetAll()
                .Where(x => x.RegisterId.Equals(registerId));
            var casePolicReportItem = await casePoliceReportDB.ToArrayAsync();
            var casePoliceReport = new CasePoliceReportDocItem();
            foreach (var c in casePolicReportItem)
            {
                casePoliceReport.IPD = c.IPD;
                casePoliceReport.ReportNo = c.ReportNo;
                casePoliceReport.ReportDate = c.ReportTime;
                casePoliceReport.PoliceFindings = c.PoliceFinding;
                casePoliceReport.PoliceOutcome = c.PoliceOutcome;
                casePoliceReport.OfficerName = c.OfficerName;
                casePoliceReport.OfficerContact = c.OfficerContact;
                casePoliceReport.ServiceNo = c.ServiceNo;
            }
            item.CasePoliceReport = casePoliceReport;

            //CASE THIRD PARTY VEHICLE
            var thirdPartyVehicleDB = _caseThirdPartyVehicleRepository.GetAll()
                .Where(x => x.RegisterId.Equals(registerId));
            var thirdPartyVehicleItem = await thirdPartyVehicleDB.ToArrayAsync();
            var thirdPartyVehicle = new List<ThirdPartyVehicle>();
            foreach (var c in thirdPartyVehicleItem)
            {
                var v = new ThirdPartyVehicle()
                {
                    VehicleNo = c.VehicleNo,
                    RegisteredOwner = c.RegisteredOwner,

                };
                thirdPartyVehicle.Add(v);
            }
            item.ThirdPartyVehicles = thirdPartyVehicle;



            return GenerateDocxFromTemplate(item, templateLocation);
        }

        private FileDto GenerateDocxFromTemplate(RegistrationExportItemDto data, string templateLocation)
        {
            byte[] templateBytes = File.ReadAllBytes(templateLocation);

            using (MemoryStream mem = new MemoryStream())
            {
                DateTime currentDate = DateTime.Today;
                mem.Write(templateBytes, 0, templateBytes.Length);
                var file = new FileDto("Registration.docx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(mem, true))
                {
                    ReplacePlaceholder(doc, "DATEGENERATED", currentDate.ToString("dd-MM-yyyy"), false);
                    ReplacePlaceholder(doc, "INSURERSCOMPANYNAME", (data.Insurers.CompanyName.IsNullOrEmpty() ? "-" : data.Insurers.CompanyName), false);
                    ReplacePlaceholder(doc, "INSURERSCOMPANYADDRESS", (data.Insurers.CompanyAddress.IsNullOrEmpty() ? "-" : data.Insurers.CompanyAddress), false);
                    ReplacePlaceholder(doc, "INSURERSNAME", (data.Insurers.Name.IsNullOrEmpty() ? "-" : data.Insurers.Name), false);
                    ReplacePlaceholder(doc, "INSURERREFERENCENO", (data.Insurers.ReferenceNo.IsNullOrEmpty() ? "-" : data.Insurers.ReferenceNo), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONPOLICYNO", (data.InsuredPersonOwner.PolicyNo.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.PolicyNo), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONCOVERAGE", (data.InsuredPersonOwner.Coverage.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Coverage), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERNAME", (data.InsuredPersonOwner.Name.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Name), false);
                    ReplacePlaceholder(doc, "CASEINCDETAILSTIMEFROM", (data.IncidentDetails.TimeFrom == null ? "-" : data.IncidentDetails.TimeFrom?.ToString("dd/MM/yyyy HH:mm:ss")), false);
                    ReplacePlaceholder(doc, "CASEINCDETAILSTIMETO", (data.IncidentDetails.TimeTo == null ? "-" : data.IncidentDetails.TimeTo?.ToString("dd/MM/yyyy HH:mm:ss")), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERNAME", (data.InsuredPersonDriver.Name.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.Name), false);

                    //IMAGE FOR CASE INSURED PERSON OWNER
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERICFRONT", null, false, GetImage(data.InsuredPersonOwner.ICFront).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERICBACK", null, false, GetImage(data.InsuredPersonOwner.ICBack).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERLICENSEFRONT", null, false, GetImage(data.InsuredPersonOwner.DriverLicenseFront).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERLICENSEBACK", null, false, GetImage(data.InsuredPersonOwner.DriverLicenseBack).Result.FileContent);

                    //IMAGE FOR CASE INSURED PERSON DRIVER
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERICFRONT", null, false, GetImage(data.InsuredPersonDriver.ICFront).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERICBACK", null, false, GetImage(data.InsuredPersonDriver.ICBack).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERLICENSEFRONT", null, false, GetImage(data.InsuredPersonDriver.DriverLicenseFront).Result.FileContent);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERLICENSEBACK", null, false, GetImage(data.InsuredPersonDriver.DriverLicenseBack).Result.FileContent);


                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERREL", (data.InsuredPersonOwner.Relationship.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Relationship), false);
                    ReplacePlaceholder(doc, "CASEINSUEREDPERSONDRIVERVEH", (data.InsuredPersonDriver.Vehicle.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.Vehicle), false);

                    var countThirdPartyVehicle = data.ThirdPartyVehicles.Count();
                    if (countThirdPartyVehicle > 0)
                    {
                        var countVehicle = 0;
                        foreach (var vehicle in data.ThirdPartyVehicles)
                        {
                            countVehicle++;
                            ReplacePlaceholder(doc, "CASETHIRDPARTYINSUREDVEHLIST", vehicle.VehicleNo, (countVehicle.Equals(countThirdPartyVehicle) ? false : true));
                            ReplacePlaceholder(doc, "CASETHIRDPARTYOWNERLIST", vehicle.RegisteredOwner, (countVehicle.Equals(countThirdPartyVehicle) ? false : true));
                            ReplacePlaceholder(doc, "CASETHIRDPARTYVEHICLEVEHICLENO", (vehicle.VehicleNo.IsNullOrEmpty() ? "-" : (countVehicle > 1 ? ", " : "") + vehicle.VehicleNo), false);
                        }

                    }
                    else
                    {
                        ReplacePlaceholder(doc, "CASETHIRDPARTYINSUREDVEHLIST", "-", false);
                        ReplacePlaceholder(doc, "CASETHIRDPARTYOWNERLIST", "-", false);
                        ReplacePlaceholder(doc, "CASETHIRDPARTYVEHICLEVEHICLENO", "-", false);
                    }

                    ReplacePlaceholder(doc, "CASEPOLICEREPORTIPD", (data.CasePoliceReport.IPD.IsNullOrEmpty() ? "-" : data.CasePoliceReport.IPD), false);
                    ReplacePlaceholder(doc, "CASEPOLICEREPORTREPORTNO", (data.CasePoliceReport.ReportNo.IsNullOrEmpty() ? "-" : data.CasePoliceReport.ReportNo), false);
                    ReplacePlaceholder(doc, "CASEPOLICEREPRTREPORTDATE", (data.CasePoliceReport.ReportDate == null ? "-" : data.CasePoliceReport.ReportDate?.ToString("dd/MM/yyyy")), false);
                    ReplacePlaceholder(doc, "CASEPLCERPTREPORTTIME", (data.CasePoliceReport.ReportDate == null ? "-" : data.CasePoliceReport.ReportDate?.ToString("HH:mm:ss")), false);
                    ReplacePlaceholder(doc, "CASEINVSOFFICERNAME", (data.CasePoliceReport.OfficerName.IsNullOrEmpty() ? "-" : data.CasePoliceReport.OfficerName), false);
                    ReplacePlaceholder(doc, "CASEPLCRPTPOLICEFINDING", (data.CasePoliceReport.PoliceFindings.IsNullOrEmpty() ? "-" : data.CasePoliceReport.PoliceFindings), false);
                    ReplacePlaceholder(doc, "CASEPLCRPTPOLICOUTCOME", (data.CasePoliceReport.PoliceOutcome.IsNullOrEmpty() ? "-" : data.CasePoliceReport.PoliceOutcome), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERIC", (data.InsuredPersonOwner.NRIC.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.NRIC), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNERADDRESS", (data.InsuredPersonOwner.Address.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Address), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONEOWNERCONTACTNO", (data.InsuredPersonOwner.Contact.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Contact), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONOWNEROCCUPATION", (data.InsuredPersonOwner.Occupation.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Occupation), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERIC", (data.InsuredPersonDriver.NRIC.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.NRIC), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERADDRESS", (data.InsuredPersonDriver.Address.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.Address), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERCONTACTNO", (data.InsuredPersonDriver.ContactNo.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.ContactNo), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVEROCCUPATION", (data.InsuredPersonDriver.Occupation.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.Occupation), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVEREMPNAME", (data.InsuredPersonDriver.EmployerName.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.EmployerName), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERCOVERAGE", (data.InsuredPersonDriver.Coverage.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.Coverage), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERLICENSEDATE", (data.InsuredPersonDriver.LicenseDateFrom == null ? "" : data.InsuredPersonDriver.LicenseDateFrom?.ToString("dd/MM/yyyy")) + " - " + (data.InsuredPersonDriver.LicenseDateTo == null ? "" : data.InsuredPersonDriver.LicenseDateTo?.ToString("dd/MM/yyyy")), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERLICENSENO", (data.InsuredPersonDriver.LicenseNo.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.LicenseNo), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVEREXP", (data.InsuredPersonDriver.DrivingExp.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.DrivingExp), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILVEHICLENO", (data.IncidentDetails.VehicleNo.IsNullOrEmpty() ? "-" : data.IncidentDetails.VehicleNo), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILDATE", ((data.IncidentDetails.TimeFrom == null) ? "" : data.IncidentDetails.TimeFrom?.ToString("dd/MM/yyyy")) + " - " + ((data.IncidentDetails.TimeTo == null) ? "" : data.IncidentDetails.TimeTo?.ToString("dd/MM/yyyy")), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILTIME", ((data.IncidentDetails.TimeFrom == null) ? "" : data.IncidentDetails.TimeFrom?.ToString("HH:mm:ss")) + " - " + ((data.IncidentDetails.TimeTo == null) ? "" : data.IncidentDetails.TimeTo?.ToString("HH:mm:ss")), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILDIRECTIONTO", (data.IncidentDetails.DirectionTo.IsNullOrEmpty() ? "-" : data.IncidentDetails.DirectionTo), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILSDIRECTIONFROM", (data.IncidentDetails.DirectionFrom.IsNullOrEmpty() ? "-" : data.IncidentDetails.DirectionFrom), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILDRIVINGWITH", (data.IncidentDetails.DrivingWith.IsNullOrEmpty() ? "-" : data.IncidentDetails.DrivingWith), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILSPEEDPRIOR", (data.IncidentDetails.SpeedPrior.IsNullOrEmpty() ? "-" : data.IncidentDetails.SpeedPrior), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILSITEOFACCIDENT", (data.IncidentDetails.SiteOfAccident.IsNullOrEmpty() ? "-" : data.IncidentDetails.SiteOfAccident), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILTYPEOFROAD", (data.IncidentDetails.TypeOfRoad.IsNullOrEmpty() ? "-" : data.IncidentDetails.TypeOfRoad), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILWIDTHOFROAD", (data.IncidentDetails.WidthOfRoad.IsNullOrEmpty() ? "-" : data.IncidentDetails.WidthOfRoad), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILCENTERDEMARCATION", (data.IncidentDetails.CenterDemarcation.IsNullOrEmpty() ? "-" : data.IncidentDetails.CenterDemarcation), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILDRIVERPATHRIGHT", (data.IncidentDetails.DriverPathRight.IsNullOrEmpty() ? "-" : data.IncidentDetails.DriverPathRight), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILDRIVERPATHLEFT", (data.IncidentDetails.DriverPathLeft.IsNullOrEmpty() ? "-" : data.IncidentDetails.DriverPathLeft), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILVIEWOFROAD", (data.IncidentDetails.ViewOfRoad.IsNullOrEmpty() ? "-" : data.IncidentDetails.ViewOfRoad), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILVISIBILITY", (data.IncidentDetails.Visibility.IsNullOrEmpty() ? "-" : data.IncidentDetails.Visibility), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILSURROUNDINGAREA", (data.IncidentDetails.SurroundingArea.IsNullOrEmpty() ? "-" : data.IncidentDetails.SurroundingArea), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILCONDROAD", (data.IncidentDetails.RoadCondition.IsNullOrEmpty() ? "-" : data.IncidentDetails.RoadCondition), false);
                    ReplacePlaceholder(doc, "CASEINCIDENTDETAILCONDWEATHER", (data.IncidentDetails.WeatherCondition.IsNullOrEmpty() ? "-" : data.IncidentDetails.WeatherCondition), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONEDRIVERICNO", (data.InsuredPersonDriver.NRIC.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.NRIC), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONONWERNAME", (data.InsuredPersonOwner.Name.IsNullOrEmpty() ? "-" : data.InsuredPersonOwner.Name), false);
                    ReplacePlaceholder(doc, "CASEREGISTRATIONVEHICLENO", (data.MainRegistration.VehicleNo.IsNullOrEmpty() ? "-" : data.MainRegistration.VehicleNo), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVEREMPADDRESS", (data.InsuredPersonDriver.EmployerAddress.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.EmployerAddress), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVEREMPCONTACTNO", (data.InsuredPersonDriver.EmployerAddress.IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.EmployerAddress), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERINCOME", (data.InsuredPersonDriver.MonthlyIncome.ToString().IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.MonthlyIncome.ToString()), false);
                    ReplacePlaceholder(doc, "CASEINSUREDPERSONDRIVERLICENSENO", (data.InsuredPersonDriver.MonthlyIncome.ToString().IsNullOrEmpty() ? "-" : data.InsuredPersonDriver.MonthlyIncome.ToString()), false);
                }
                _tempFileCacheManager.SetFile(file.FileToken, mem.ToArray());
                return file;
                //return mem.ToArray();
            }
        }

        private void ReplacePlaceholder(WordprocessingDocument doc, string placeholder, string replacement, bool breakLine, byte[] imageBytes = null)
        {
            if (imageBytes != null)
            {
                InsertImage(doc, placeholder, imageBytes);
            }
            else
            {
                foreach (var text in doc.MainDocumentPart.Document.Descendants<Text>())
                {
                    if (text.Text.Equals(placeholder))
                    {
                        text.Text = text.Text.Replace(placeholder, replacement);
                        if (breakLine)
                            text.InsertAfterSelf(new Break());
                    }
                }
            }
        }
        private void InsertImage(WordprocessingDocument doc, string placeholder, byte[] imageBytes)
        {
            // Get the main document part
            var mainPart = doc.MainDocumentPart;

            // Add an image part to the document
            var imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
            using (var stream = new MemoryStream(imageBytes))
            {
                imagePart.FeedData(stream);
            }

            // Get the ID of the image part
            string imagePartId = mainPart.GetIdOfPart(imagePart);

            // Set the desired size of the image (e.g., 4 inches x 3 inches)
            long widthEmus = 3 * 914400; // 4 inches in EMUs
            long heightEmus = 2 * 914400; // 3 inches in EMUs

            // Create a new drawing element for the image
            var drawingElement = new Drawing(
                new DW.Inline(
                    new DW.Extent() { Cx = widthEmus, Cy = heightEmus },
                    new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                    new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Picture" },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks() { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "New Bitmap Image.jpg" },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip() { Embed = imagePartId },
                                    new A.Stretch(new A.FillRectangle())),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset() { X = 0L, Y = 0L },
                                        new A.Extents() { Cx = 990000L, Cy = 792000L }
                                    ),
                                    new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }
                                )
                            )
                        )
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                    )
                )
            );

            // Find and replace the placeholder with the image
            var text = doc.MainDocumentPart.Document.Descendants<Text>()
                         .FirstOrDefault(t => t.Text.Contains(placeholder));

            if (text != null)
            {
                var run = text.Parent as Run;

                if (run != null)
                {
                    // Remove the placeholder text
                    run.RemoveAllChildren<Text>();
                    // Append the drawing element (image)
                    run.Append(drawingElement);
                }
            }
        }



        private async Task<FileViewDto> GetImage(Guid? referenceNo)
        {
            if (!referenceNo.HasValue)
            {
                return new FileViewDto();
            }

            var fileData = await _fileOrgService.GetFileByReferenceNo(referenceNo.Value);

            return new FileViewDto
            {
                ContentType = fileData.ContentType,
                FileName = fileData.FileName,
                FileContent = fileData.FileContent,
            };
        }

    }
}
