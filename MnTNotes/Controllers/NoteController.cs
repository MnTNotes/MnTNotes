using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MnTNotes.Data;
using MnTNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MnTNotes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly ApplicationDbContext _db;

        public NoteController(ILogger<NoteController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Note> Get()
        {
            // TEST DATA!!!
            //return new List<Note> {
            // new Note{ Title = "asdasd1", Content = "asdaaa1", CreatedDate = DateTime.Now, UpdateDate = DateTime.Now },
            // new Note{ Title = "asdasd2", Content = "asdaaa2", CreatedDate = DateTime.Now, UpdateDate = DateTime.Now },
            // new Note{ Title = "asdasd3", Content = "asdaaa3", CreatedDate = DateTime.Now, UpdateDate = DateTime.Now },
            //};

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //string userId = User.Claims.First(x => x.Type == "sub").Value;

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var usernotes = _db.Notes.Where(x => x.UserId == userId).ToList();
                    return usernotes;
                }
            }

            return new List<Note> { };
        }

        [HttpGet("{id}")]
        public Note GetNote(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var note = _db.Notes.FirstOrDefault(x => x.UserId == userId && x.Id == id);
                    if (note != null)
                    {
                        return note;
                    }
                }
            }
            return new Note { };
        }

        [HttpPut("{id}")]
        public Note PutNote(int id, [FromForm] Note note)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    note.Id = id;
                    note.UserId = userId;

                    Regex rx = new Regex(@"(<img).*\/>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var matches = rx.Matches(note.Content).ToArray();
                    var splitedContent = rx.Split(note.Content);
                    int matchIndex = 0;
                    for (int i = 0; i < splitedContent.Length; i++)
                    {
                        if (splitedContent[i] == "<img")
                        {
                            string value = "<figure>" + matches[matchIndex].Value + "</figure>";
                            splitedContent[i] = value;
                            matchIndex++;
                        }
                    }
                    note.Content = string.Join("", splitedContent);

                    _db.Entry(note).State = EntityState.Modified;

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NoteExists(id))
                        {
                            return new Note { };
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            return new Note { };
        }

        [HttpPost]
        public Note PostNote([FromForm] Note note)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    note.UserId = userId;
                    note.CreatedDate = DateTime.Now;
                    note.Title = note.Title != null ? note.Title : "";
                    note.Content = note.Content != null ? note.Content : "";

                    // add figure example better way
                    Regex rx = new Regex(@"(<img).*\/>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var matches = rx.Matches(note.Content).ToArray();
                    var splitedContent = rx.Split(note.Content);
                    int matchIndex = 0;
                    for (int i = 0; i < splitedContent.Length; i++)
                    {
                        if (splitedContent[i] == "<img")
                        {
                            string value = "<figure>" + matches[matchIndex].Value + "</figure>";
                            splitedContent[i] = value;
                            matchIndex++;
                        }
                    }

                    note.Content = string.Join("", splitedContent);

                    // add figure example
                    //foreach (Match item in matches)
                    //    note.Content = note.Content.Insert(item.Index, "<figure>").Insert(item.Index + item.Length + 8, "</figure>");

                    _db.Notes.Add(note);
                    _db.SaveChanges();
                    return note;
                }
            }

            return note;
        }

        [HttpDelete("{id}")]
        public Note DeleteNote(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var note = _db.Notes.Find(id);
                    if (note == null)
                    {
                        return new Note { };
                    }

                    _db.Notes.Remove(note);
                    _db.SaveChanges();
                }
            }

            return new Note { };
        }

        private bool NoteExists(int id)
        {
            return _db.Notes.Any(e => e.Id == id);
        }
    }
}
