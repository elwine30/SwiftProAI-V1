using System.Collections.Generic;

namespace ThinknInsurTech.Integration.Dto
{
    public class ChatGPTInputDto
    {
        public string model { get; set; }
        public List<ChatGPTInputMessagesDto> messages { get; set; }
        public int temperature { get; set; }
        public int max_tokens { get; set; }
        public int top_p { get; set; }
        public int frequency_penalty { get; set; }
        public int presence_penalty { get; set; }    
    }

    public class ChatGPTInputMessagesDto
    {
        public string role { get; set; }
        public List<object> content { get; set; }
    }
}
