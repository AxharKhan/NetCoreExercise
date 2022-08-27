using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreExercise.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name="Customer Type")]
        public int CustomerTypeId { get; set; }
        public CustomerType? CustomerType { get; set; }
        [StringLength(1024)]
        public string? Description { get; set; }
        [StringLength(50)]
        [Required]
        public string Address { get; set; }
        [StringLength(50)]
        [Required]
        public string City { get; set; }
        [StringLength(2)]
        [Required]
        public string State { get; set; }
        [StringLength(10)]
        [Required]
        public string Zip { get; set; }
        [StringLength(7)]
        public DateTime? LastUpdated { get; set; }
    }
}
