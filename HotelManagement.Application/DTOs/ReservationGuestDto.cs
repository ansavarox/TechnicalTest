using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HotelManagement.Application.DTOs
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum DocumentType
    {
        Passport,
        ID,
        DriverLicense
    }

    public class ReservationGuestDto : IValidatableObject
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        public string Gender { get; set; }  

        [Required]
        public string DocumentType { get; set; } 

        [Required]
        public string DocumentNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validGenders = Enum.GetNames(typeof(Gender));
            if (!validGenders.Contains(Gender, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    $"Invalid gender. Accepted values: {string.Join(", ", validGenders)}.",
                    new[] { nameof(Gender) });
            }

            var validDocumentTypes = Enum.GetNames(typeof(DocumentType));
            if (!validDocumentTypes.Contains(DocumentType, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    $"Invalid document type. Accepted values: {string.Join(", ", validDocumentTypes)}.",
                    new[] { nameof(DocumentType) });
            }
        }
    }

}
