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
        TephraSystemDataAccessLayer systemdb = new TephraSystemDataAccessLayer();
        public ActionResult Index()
        {
            characters = chardb.GetCharacters(User.GetUserId()).ToList();
            return View(characters);
        }
        [HttpGet]
        public ActionResult ViewCharacter(int ID)
        {
            var character = chardb.GetCharacter(ID);
            if (character == null)
                return NotFound();
            return View(character);
        }

        [HttpGet]
        public ActionResult SelectSpecialty(int characterID)
        {
            var sp = new Specialty();
            sp.CharacterID = characterID;
            ViewBag.specialtyList = systemdb.GetAllSpecialties().ToList();
            return View(sp);
        }

        [HttpGet]
        public ActionResult DeleteSpecialty(int charID, int ID)
        {
            var character = new Character();
            character.ID = charID;
            var sp = chardb.GetCharacterSpecialties(character).FirstOrDefault(e => e.ID == ID);
            if(sp == null)
                return NotFound();
            return View(sp);
        }
        [HttpPost]
        public ActionResult DeleteSpecialty(Specialty sp)
        {
            chardb.DeleteCharacterSpecialty(sp.CharacterID, sp);
            return RedirectToAction("ViewCharacter", "Characters", new { ID = sp.CharacterID});
        }
        [HttpPost]
        public ActionResult SelectSpecialty(Specialty specialty)
        {
            int charID = specialty.CharacterID;

            var sp = systemdb.GetSpecialty(specialty.SpecialtyID);
            specialty.CharacterID = charID;
            try
            {
                if (ModelState.IsValid)
                {
                    chardb.AddCharacterSpecialty(charID, sp);
                    return RedirectToAction("ViewCharacter", "Characters", new { ID = charID});
                }
                
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            ViewBag.specialtyList = systemdb.GetAllSpecialties().ToList();
            return View(specialty);
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