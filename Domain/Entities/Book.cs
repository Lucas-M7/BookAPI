using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Domain.Entities;
public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Author { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string DateRelease { get; set; } = default!;

    [Required]
    public bool Readed { get; set; }

    [Required]
    [StringLength(150)]
    public string UserName { get; set; } = default!;
}