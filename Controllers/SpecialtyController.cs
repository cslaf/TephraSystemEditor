using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using tephraSystemEditor.Models;
using System.Linq;
using System;


namespace tephraSystemEditor.Controllers
{
    public class SpecialtyController : Controller
    {

        [BindProperty]
        List<Skill> skills {get; set;}
        TephraSystemDataAccessLayer systemdb = new TephraSystemDataAccessLayer();
        [HttpGet]
        public ActionResult Index()
        {
            return View(systemdb.GetAllAttributes());
        }

        [HttpGet]
        public ActionResult ShowAttribute(string name)
        {
            Attr attr = systemdb.GetAllAttributes().FirstOrDefault(a => name == a.Name);
            if(attr == null)
                return NotFound();
            return View("ShowAttribute", attr);
        }
        [HttpGet]
        public ActionResult ShowSkill(string name)
        {
            Skill skill = systemdb.GetAllSkills().FirstOrDefault(a => name == a.Name);
            if(skill == null)
                return NotFound();

            return View("ShowSkill", skill);
        }

        [HttpGet]
        public ActionResult Edit (int id)
        {
            var sp = systemdb.GetSpecialty(id);
            if (sp == null)
                return NotFound();
            return View(sp);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit (Specialty sp)
        {       
            try
            {
                if (ModelState.IsValid)
                { 
                    systemdb.Update(sp);

                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(sp);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Delete (int id )
        {
            var sp = systemdb.GetSpecialty(id);
            if (sp == null)
                return NotFound();
            return View(sp); 
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(Specialty sp)
        {
           var spec = systemdb.GetSpecialty(sp.ID);
           if (spec == null)
                return NotFound();
            systemdb.Delete(spec.ID);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public ActionResult Create(int skillID)
        {
            var sp = new Specialty();
            sp.SkillID = skillID;
            return View(sp);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create (Specialty spec )
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    systemdb.Add(spec);

                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(spec);
        }

    }
}