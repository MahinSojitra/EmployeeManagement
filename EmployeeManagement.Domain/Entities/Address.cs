﻿namespace EmployeeManagement.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public Guid EmployeeId { get; set; }
    }

}
