using System;
namespace EmployeeManagement.Domain.Entities
{
    public class Position
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
