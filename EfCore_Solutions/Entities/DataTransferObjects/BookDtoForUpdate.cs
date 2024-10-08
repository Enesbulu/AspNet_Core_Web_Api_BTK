﻿using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    //public record BookDtoForUpdate(int Id, String Title, decimal Price);
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        public int Id { get; init; }

        [Required(ErrorMessage = "Title is a required field!")]
        [MinLength(2, ErrorMessage = "Title must consist of at least characters")]
        [MaxLength(50, ErrorMessage = "Title must consist of at maximum 50 characters")]
        public String Title { get; init; }

        [Required(ErrorMessage = "Price is a required field!")]
        [Range(10, 1000)]
        public decimal Price { get; init; }
    }
}
