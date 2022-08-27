using System.ComponentModel.DataAnnotations;

namespace NetCoreExercise.Models
{
    public class CustomerType
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
