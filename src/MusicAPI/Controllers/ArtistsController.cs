using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Helpers;
using MusicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _db;

        public ArtistsController(ApiDbContext db)
        {
            _db = db;
        }
        
        //Http Get /api/artists
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var artist = await _db.Artists.ToListAsync();
            return Ok(artist);
        }

        //http get api/artists/artistdetails?artistid=
        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int artistId)
        {
            var artistDetails = await _db.Artists.Where(a => a.Id == artistId).Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }
        //Http Post /api/artist
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]Artist artist)
        {
            var artistImageUrl = await FileHelper.ImageUpload(artist.Image);
            artist.ImageUrl = artistImageUrl;
            await _db.Artists.AddAsync(artist);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
