using System;
using System.Collections.Generic;
using System.Text;

namespace ThinknInsurTech.Integration.Dtos
{
    public class OpenAIIntegrationLogRequest
    {
        public string FileName { get; set; }
        public string Prompt { get; set; }
    }
}
