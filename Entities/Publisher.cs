using System;
using System.Collections.Generic;

namespace LibraryManagement.Entities;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
