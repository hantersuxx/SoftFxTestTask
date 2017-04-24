using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class SymbolsController : ApiController
    {
        private TaskDbContext db = new TaskDbContext();

        // GET: api/Symbols
        public IHttpActionResult GetSymbols()
        {
            return Json(db.Symbols);
        }

        // GET: api/Symbols/5
        [ResponseType(typeof(Symbol))]
        public IHttpActionResult GetSymbol(Guid id)
        {
            Symbol symbol = db.Symbols.Find(id);
            if (symbol == null)
            {
                return NotFound();
            }

            return Json(symbol);
        }

        // PUT: api/Symbols/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSymbol(Guid id, Symbol symbol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != symbol.Id)
            {
                return BadRequest();
            }

            db.Entry(symbol).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SymbolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Symbols
        [ResponseType(typeof(Symbol))]
        public IHttpActionResult PostSymbol(Symbol symbol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Symbols.Add(symbol);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SymbolExists(symbol.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = symbol.Id }, symbol);
        }

        // DELETE: api/Symbols/5
        [ResponseType(typeof(Symbol))]
        public IHttpActionResult DeleteSymbol(Guid id)
        {
            Symbol symbol = db.Symbols.Find(id);
            if (symbol == null)
            {
                return NotFound();
            }

            db.Symbols.Remove(symbol);
            db.SaveChanges();

            return Ok(symbol);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SymbolExists(Guid id)
        {
            return db.Symbols.Count(e => e.Id == id) > 0;
        }
    }
}