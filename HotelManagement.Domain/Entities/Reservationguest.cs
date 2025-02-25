using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Domain.Entities;

[Table("reservationguests")]
public partial class Reservationguest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reservationid")]
    public int Reservationid { get; set; }

    [Column("fullname", TypeName = "character varying")]
    public string Fullname { get; set; } = null!;

    [Column("birthdate")]
    public DateOnly Birthdate { get; set; }

    [Column("gender", TypeName = "character varying")]
    public string Gender { get; set; } = null!;

    [Column("documenttype", TypeName = "character varying")]
    public string Documenttype { get; set; } = null!;

    [Column("documentnumber", TypeName = "character varying")]
    public string Documentnumber { get; set; } = null!;

    [Column("email", TypeName = "character varying")]
    public string Email { get; set; } = null!;

    [Column("phone", TypeName = "character varying")]
    public string Phone { get; set; } = null!;

    [ForeignKey("Reservationid")]
    [InverseProperty("Reservationguests")]
    public virtual Reservation Reservation { get; set; } = null!;
}
