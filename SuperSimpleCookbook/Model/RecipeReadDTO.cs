﻿using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class RecipeReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
