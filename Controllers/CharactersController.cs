using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using tephraSystemEditor.Models;
using System.Linq;
using System;

namespace tephraSystemEditor.Controllers
{
    public class CharactersController : Controller
    {
        List<Character> characters {get; set;}

        CharactersDataAccessLayer chardb = new CharactersDataAccessLayer();

        public ActionResult Index(string userID)
        {
            characters = chardb.GetCharacters(userID).ToList();
            return View(characters);
        }

        [HttpGet]
        public ActionResult Create(string userID)
        {
            var ch = new Character();
            ch.UserID = userID;
            return View(ch);
        }

        [HttpPost]
        public ActionResult Create (Character ch )
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    chardb.Add(ch);

                    return Index(ch.UserID);
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(ch);
        }
    }
}