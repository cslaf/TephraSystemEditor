using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using tephraSystemEditor.Models;
using System.Linq;

namespace tephraSystemEditor.Controllers
{
    public class ViewSystemController : Controller
    {
        [BindProperty]
        List<Attr> attributes {get; set;}

        [BindProperty]
        List<Skill> skills {get; set;}
        TephraSystemDataAccessLayer systemdb = new TephraSystemDataAccessLayer();

        public ActionResult Index()
        {
            attributes = systemdb.GetAllAttributes().ToList();
            return View(attributes);
        }

        public ActionResult GetAttribute(string id)
        {
            object model = null;
            var valid = int.TryParse(id, out var ID);
            if(valid)
                attributes = systemdb.GetAllAttributes().ToList();

            model = attributes.ElementAtOrDefault(ID-1);
            
            if(model == null)
                return View("Index", attributes);

            return View("AttributeView", model);
        }

        public ActionResult GetSkill(string attributeID, string select)
        {
            object model = null;
            var valid = int.TryParse(attributeID, out var ID) && int.TryParse(select, out var skill);
            if(valid)
                attributes = systemdb.GetAllAttributes().ToList();


            return View("SkillView", model);
        }

    }
}