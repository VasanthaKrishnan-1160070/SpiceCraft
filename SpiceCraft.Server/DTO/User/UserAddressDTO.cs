namespace SpiceCraft.Server.DTO.User
{
    public class UserAddressDTO
    {
        public string StateOrProvince { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string? AddressType { get; set; }
    }
}
