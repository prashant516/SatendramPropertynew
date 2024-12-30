namespace SatendramProperty.Models
{
    public class PropertyMaster
    {
        public int PropertyID { get; set; }
        public string? PropertyStatus { get; set; }
        public string? PropertyType { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Landmark { get; set; }
        public string? Address { get; set; }
        public string? SaleType { get; set; }
        public string? Ownership { get; set; }
        public int NoofFloor { get; set; }
        public string? Availablity { get; set; }
        public int PropertyonFloor { get; set; }
        public string? BuiltUpArea { get; set; }
        public string? PlotArea { get; set; }
        public string? CarpetArea { get; set; }
        public string? SuperArea { get; set; }
        public string? ExpectedPrice { get; set; }
        public string? BookingAmount { get; set; }
        public string? MaintenanceCharges { get; set; }
        public string? NoofBedrooms { get; set; }
        public int NoofBathRooms { get; set; }
        public int NoofBalconies { get; set; }
        public string? Description { get; set; }
        public string? VideoURl { get; set; }

        public string? PropertyMedia { get; set; }


    }

    public class Property
    {

        public string? propertyMaster { get; set; }
        public string? PropertyMedia { get; set; }

    }


}
