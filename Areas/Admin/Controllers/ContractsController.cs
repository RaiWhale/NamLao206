using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.Admin.Controllers
{
    public class ContractsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/Contracts
        public async Task<ActionResult> Index()
        {
            var contracts = db.Contracts.Include(c => c.Account).Include(c => c.Account1);
            return View(await contracts.ToListAsync());
        }

        // GET: Admin/Contracts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Admin/Contracts/Create
        public ActionResult Create()
        {
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName");
            return View();
        }

        // POST: Admin/Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ContractTypeName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Contracts.Add(contract);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.ModifiedUserId);
            return View(contract);
        }

        // GET: Admin/Contracts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.ModifiedUserId);
            return View(contract);
        }

        // POST: Admin/Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ContractTypeName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", contract.ModifiedUserId);
            return View(contract);
        }

        // GET: Admin/Contracts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Admin/Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            db.Contracts.Remove(contract);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
