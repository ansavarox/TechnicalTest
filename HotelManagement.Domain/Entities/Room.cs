using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Domain.Entities;

[Table("rooms")]
public partial class Room
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("hotelid")]
    public int Hotelid { get; set; }

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("basecost")]
    [Precision(10, 2)]
    public decimal Basecost { get; set; }

    [Column("taxes")]
    [Precision(10, 2)]
    public decimal Taxes { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }
    [Column("location")]
    public string Location { get; set; } 

    [Column("roomtype")]
    public string RoomType { get; set; }  

    [ForeignKey("Hotelid")]
    [InverseProperty("Rooms")]
    public virtual Hotel Hotel { get; set; } = null!;

    [InverseProperty("Room")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
