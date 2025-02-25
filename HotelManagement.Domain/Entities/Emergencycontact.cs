using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Domain.Entities;

[Table("emergencycontacts")]
public partial class Emergencycontact
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reservationid")]
    public int Reservationid { get; set; }

    [Column("fullname", TypeName = "character varying")]
    public string Fullname { get; set; } = null!;

    [Column("phone", TypeName = "character varying")]
    public string Phone { get; set; } = null!;

    [ForeignKey("Reservationid")]
    [InverseProperty("Emergencycontacts")]
    public virtual Reservation Reservation { get; set; } = null!;
}
