using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using tephraSystemEditor.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Principal;
using Shared.Web.MvcExtensions;

namespace tephraSystemEditor.Controllers
{
    [Authorize]
    public class CharactersController : Controller
    {
        List<Character> characters {get; set;}
        CharactersDataAccessLayer chardb = new CharactersDataAccessLayer();
        public ActionResult Index()
        {
            characters = chardb.GetCharacters(User.GetUserId()).ToList();
            return View(characters);
        }

        public ActionResult ViewCharacter(int ID)
        {
            var character = chardb.GetCharacter(ID);
            if (character == null)
                return NotFound();
            return View(character);
        }

        [HttpGet]
        public ActionResult Edit (int ID)
        {
            var character = chardb.GetCharacter(ID);
            if (character == null)
                return NotFound();
            return View(character);
        }

       [HttpPost]
        public ActionResult Edit(Character ch)
        {       
            try
            {
                if (ModelState.IsValid)
                { 
                    chardb.Update(ch);

                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(ch);
        }


        [HttpGet]
        public ActionResult Delete (int ID)
        {
            var character = chardb.GetCharacter(ID);
            if ( character == null)
                return NotFound();
            return View(character); 
        }

        [HttpPost]
        public ActionResult Delete(Character ch)
        {
           var character = chardb.GetCharacter(ch.ID);
            if (character == null)
                return NotFound();
            chardb.Delete(character);
            return RedirectToAction(nameof(Index));
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
                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return RedirectToAction("ViewCharacter", ch);
        }
    }
}