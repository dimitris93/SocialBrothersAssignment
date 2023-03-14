namespace SocialBrothersAssignment
{
    public class Address
    {
        public long Id { get; set; }
        public string Street { get; set; } = String.Empty;
        public string HouseNumber { get; set; } = String.Empty;
        public string ZipCode { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;

        public bool Validate()
        {
            return Street.Length >= 1 && Street.Length <= 100 &&
                    HouseNumber.Length >= 1 && HouseNumber.Length <= 10 &&
                    ZipCode.Length >= 1 && ZipCode.Length <= 10 &&
                    City.Length >= 1 && City.Length <= 25 &&
                    Country.Length >= 1 && Country.Length <= 50
                ;
        }
    }
}
