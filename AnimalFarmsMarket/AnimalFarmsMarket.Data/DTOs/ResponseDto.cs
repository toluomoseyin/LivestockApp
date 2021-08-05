using System.Collections.Generic;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class ResponseDto<T>
    {
        public string Message { get; set; }
        public Dictionary<string, string> Errs { get; set; }
        public T Data { get; set; } 

    }
}
