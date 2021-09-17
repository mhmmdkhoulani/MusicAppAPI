using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAPI.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
