using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Domain.Entities;

[Table("reservations")]
public partial class Reservation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("hotelid")]
    public int Hotelid { get; set; }

    [Column("roomid")]
    public int Roomid { get; set; }

    [Column("travelerid")]
    public int Travelerid { get; set; }

    [Column("checkindate")]
    public DateOnly Checkindate { get; set; }

    [Column("checkoutdate")]
    public DateOnly Checkoutdate { get; set; }

    [Column("totalcost")]
    [Precision(10, 2)]
    public decimal Totalcost { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<Emergencycontact> Emergencycontacts { get; set; } = new List<Emergencycontact>();

    [ForeignKey("Hotelid")]
    [InverseProperty("Reservations")]
    public virtual Hotel? Hotel { get; set; } = null!;

    [InverseProperty("Reservation")]
    public virtual ICollection<Reservationguest> Reservationguests { get; set; } = new List<Reservationguest>();

    [ForeignKey("Roomid")]
    [InverseProperty("Reservations")]
    public virtual Room? Room { get; set; } = null!;

    [ForeignKey("Travelerid")]
    [InverseProperty("Reservations")]
    public virtual User? Traveler { get; set; } = null!;
}
