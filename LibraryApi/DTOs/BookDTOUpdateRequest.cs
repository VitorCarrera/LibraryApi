using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class BookDTOUpdateRequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage = "Stok must be between 1 and 9999")]
        public float Stock { get; set; }
        public DateTime RegistrationDate { get; set; }
         
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(RegistrationDate.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("The date must be higher than the actually date",
                    new[] { nameof(this.RegistrationDate) });
            }
        }
    }
}

