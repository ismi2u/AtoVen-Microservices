namespace AtoVen.API.Entities.ValiationResultEntities
{
    public class IBANValidationResult
    {
        public string Status { get; set; }
        public string ResultData { get; set; }
    }
    public class APIValidationResult
    {
        public string status { get; set; }
        public string formattedaddress { get; set; }
    }
    public class AddressValidationResult
    {
        public string Status { get; set; }
        public string ResultData { get; set; }
    }

    public class VATValidationResult
    {
        public string Status { get; set; }
        public string ResultData { get; set; }
    }
}
