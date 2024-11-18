using System;
using System.Collections.Generic;

namespace angel1953_backend.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookName { get; set; } = null!;

    public string? Author { get; set; }

    public DateTime? PublicDate { get; set; }

    public string ISBN {get;set;}
    public string? ISBNUrl{get;set;}


}
