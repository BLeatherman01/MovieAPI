﻿using System;
using System.Collections.Generic;

namespace MovieAPI.Models
{
    public partial class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
    }
}
