using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Domain.Entities;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fullname", TypeName = "character varying")]
    public string Fullname { get; set; } = null!;

    [Column("email", TypeName = "character varying")]
    public string Email { get; set; } = null!;

    [Column("passwordhash")]
    public string Passwordhash { get; set; } = null!;

    [Column("role", TypeName = "character varying")]
    public string Role { get; set; } = null!;

    [InverseProperty("Traveler")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
