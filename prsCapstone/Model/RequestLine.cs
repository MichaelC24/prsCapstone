﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace prsCapstone.Model
{
    public class RequestLine
    {
        public int Id { get; set; } = 0;
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        [JsonIgnore]
        public virtual Request? Requests { get; set; }
        
        public virtual Product? Product { get; set; }
    }
}
