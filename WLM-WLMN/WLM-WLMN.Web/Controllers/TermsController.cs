using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WLM_WLMN.Common.Interfaces;
using WLM_WLMN.Web.Models;

namespace WLM_WLMN.Web.Controllers
{
    public class TermsController : Controller
    {
        private ITermLogic _termLogic;

        public TermsController(ITermLogic termLogic)
        {
            _termLogic = termLogic;
        }

        // GET: Terms
        public ActionResult Index()
        {
            List<Term> terms = _termLogic.GetTerms().Select(term => Mapper.Map<Models.Term>(term)).ToList();
            return View(terms);
        }

        public ActionResult Create()
        {
            return View(new Term());
        }

        [HttpPost]
        public ActionResult Create(Term term)
        {
            if (ModelState.IsValid)
            {
                _termLogic.CreateOrUpdate(Mapper.Map<Common.DTOs.Term>(term));
                return RedirectToAction("Index");
            }
            return View(term);
        }

        public ActionResult Details(Guid id)
        {
            return View(Mapper.Map<Term>(_termLogic.GetTerm(id)));
        }

        public ActionResult Delete(Guid id)
        {
            _termLogic.DeleteTerm(id);
            return RedirectToAction("Index");
        }
    }
}