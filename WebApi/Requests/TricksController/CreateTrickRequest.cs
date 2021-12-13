﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApi.Requests.TricksController
{
    public class CreateTrickRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Video { get; set; }
    }
}