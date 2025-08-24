using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAvanadeApi.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;
        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = default!;
        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = default!;
        [Required]
        [StringLength(10)]
        public int Ano { get; set; } = default!;
    }
}