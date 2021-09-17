using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Helpers;
using MusicAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApiDbContext _db;

        public SongsController(ApiDbContext db)
        {
            _db = db;
        }

        // GET: api/<SongsController>
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 1;
            var songs = await _db.Songs.ToListAsync();
            return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var song = await _db.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("There is no record with this Id");
            }
            return Ok(song);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.ImageUpload(song.Image);
            song.ImageUrl = imageUrl;
            var audioUrl = await FileHelper.AudioUpload(song.AudioFile);
            song.AudioUrl = audioUrl;
            song.UploadDate = DateTime.Now;
            await _db.Songs.AddAsync(song);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }



        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song songObj)
        {
            var song = await _db.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound("There is no recoed with this Id");
            }
            else
            {

                song.Title = songObj.Title;
                song.Language = songObj.Language;
                song.Duration = songObj.Duration;
                await _db.SaveChangesAsync();
                return Ok("Recored updated successfully");
            }

        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _db.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("There is no recoed with this Id");
            }
            else
            {
                _db.Songs.Remove(song);
                await _db.SaveChangesAsync();
                return Ok("Recored deleted successfully");
            }

        }

        //Featuredsongs api/songs/FeaturedSiongs
        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in _db.Songs where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudionUrl = song.AudioUrl
                               }
                               ).ToListAsync();
            return Ok(songs);
        }

        //NewSongs api/songs/newsongs
        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in _db.Songs
                               orderby song.UploadDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudionUrl = song.AudioUrl
                               }
                               ).Take(10).ToListAsync();
            return Ok(songs);
        }
        //searchsong api/songs/searchSong
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSong(string query)
        {
            var songs = await (from song in _db.Songs
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudionUrl = song.AudioUrl
                               }
                               ).Take(10).ToListAsync();

            return Ok(songs);
        }

    }
}
