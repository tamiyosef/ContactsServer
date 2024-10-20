using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerBuilding.Models;

public partial class Cake
{
    [Key]
    public int CakeId { get; set; }

    [StringLength(100)]
    public string CakeType { get; set; } = null!;
}
