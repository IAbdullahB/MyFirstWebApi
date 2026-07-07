using System;
using System.Collections.Generic;

namespace MyFirstWebAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Sku { get; set; } = null!;
}
