using System;
using System.ComponentModel.DataAnnotations;

namespace JudgeMyTaste.Models
{
    public class FavoriteBand
    {
        public int Id { get; set; }
         public string Name { get; set; }
          public string EnteredBy { get; set; }
           public DateTime EnteredOn { get; set; }

        
    }
}