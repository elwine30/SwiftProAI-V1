namespace ThinknInsurTech.Integration.Dto
{
    public class ChatGPTResponseDto
    {
        public ErrorMessageDto error;
    }

    public class ErrorMessageDto
    {
        public string message { get; set; }
        public string type { get; set; }
        public string param { get; set; }
        public string code { get; set; }
    }
}
