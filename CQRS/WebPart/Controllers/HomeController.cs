using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using CQRS.EventStore;
using WebPart.Facade;
using WebPart.ViewModel;

namespace WebPart.Controllers
{
    public class HomeController : Controller
    {
        //PersonFacade facade = new PersonFacade();
        private PersonFacade facade;
        public HomeController(PersonFacade fcade)
        {
            facade = fcade;
        }
        public ActionResult Index()
        {
            ViewBag.Model = facade.GetAll();
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Delete(Guid id)
        {
            facade.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(VMPerson item)
        {
            facade.Save(item);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            var item = facade.GetById(id);
            var model = new VMPerson()
            {
                Id = item.Id,
                LastName = item.LastName,
                FirstName = item.FirstName,
                Age = item.Age,
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(VMPerson item)
        {
            try
            {
                facade.Update(item);
            }
            catch (ConcurrencyException err)
            {

                ViewBag.error = err.Message;
                ModelState.AddModelError("", err.Message);
                return View();

            }

            return RedirectToAction("Index");
        }
        
        
        
        
        //PersonFacade facade = new PersonFacade();
        //// GET api/diary
        
        //public IEnumerable<VMPerson> Get()
        //{
        //    return facade.GetAll();
        //}

        ////// GET api/diary/5
        //public VMPerson Get(Guid id)
        //{
        //    return facade.GetById(id);
        //}

        ////// POST api/diary
        //public void Post([FromBody]VMPerson item)
        //{
        //    facade.Save(item);
        //}

        ////// PUT api/diary/5
        //public void Put(Guid id, [FromBody]VMPerson item)
        //{
        //    facade.Update(item);
        //}

        ////// DELETE api/diary/5
        //public void Delete(Guid id)
        //{
        //    facade.Delete(id);
        //}
    }
}
