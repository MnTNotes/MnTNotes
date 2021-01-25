using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MnTNotes.Core.Data.Domain;
using MnTNotes.Services.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MnTNotes.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly INoteService _noteService;

        public NoteController(ILogger<NoteController> logger, INoteService noteService)
        {
            _logger = logger;
            _noteService = noteService;
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
                    _logger.LogDebug("GET: ALL USER NOTES");
                    var usernotes = _noteService.GetByUserId(userId);
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
                    //_db.Notes.FirstOrDefault(x => x.UserId == userId && x.Id == id);
                    var note = _noteService.GetById(id);

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

                    _noteService.Update(note);
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
                    note.Title ??= "";
                    note.Content ??= "";

                    // add figure example 2 better way
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

                    // add figure example 1
                    //foreach (Match item in matches)
                    //    note.Content = note.Content.Insert(item.Index, "<figure>").Insert(item.Index + item.Length + 8, "</figure>");

                    _noteService.Add(note);

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
                    var note = _noteService.GetById(id);
                    if (note != null)
                    {
                        _noteService.Delete(note);
                    }
                }
            }

            return new Note { };
        }
    }
}