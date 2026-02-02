using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthApp.Domain.Entities
{
    public class Food
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Calories { get; set; }

        public decimal Protein { get; set; }

        public decimal Carbs { get; set; }

        public decimal Fat { get; set; }

        public int Cost { get; set; }

        [Range(0,100)]
        public int BioAvailibility { get; set; }
    }
}
