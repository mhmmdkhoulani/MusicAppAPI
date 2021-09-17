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
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _db;

        public AlbumsController(ApiDbContext db)
        {
            _db = db;
        }

        //Http Get /api/albums
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var albums = await _db.Albums.ToListAsync();
            return Ok(albums);
        }

        //http get api/albums/albumsdetails?albumId=
        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumDetails = await _db.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
            return Ok(albumDetails);
        }

        //Http Post /api/artist
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            var albumImageUrl = await FileHelper.ImageUpload(album.Image);
            album.ImageUrl = albumImageUrl;
            await _db.Albums.AddAsync(album);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
