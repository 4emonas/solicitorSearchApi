using SolicitorSearch.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolicitorSearch.DataAccess.Entities
{
    [Table("solicitor")]
    public class SolicitorEntity
    {
        public SolicitorEntity() { }
        public SolicitorEntity(Solicitor origin)
        {
            Name = origin.Name;
            Address = origin.Address;
            PhoneNumber = origin.PhoneNumber;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("city")]
        public string? City { get; set; }
    }
}
