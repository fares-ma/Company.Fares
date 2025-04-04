using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Fares.PL.Dtos
{
    public class CreateEmployeeDto
    {

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Range(18, 60, ErrorMessage = "Age Must be Between 22-60")]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Not Found")]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-z]{5,10}$", ErrorMessage = "Address Must be 123-street-city-country")]
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Hirring Date")]
        public DateTime HirringDate { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime CreateAt { get; set; }
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        public string? ImageName { get; set; }

        public IFormFile? Image { get; set; }
    }
}
