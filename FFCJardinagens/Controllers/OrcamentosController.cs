using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FFCJardinagens.Models;

namespace FFCJardinagens.Controllers
{
    public class OrcamentosController : Controller
    {
        private FFCJardinagensContext db = new FFCJardinagensContext();

        // GET: Orcamentos
        public ActionResult Index()
        {
            return View(db.Orcamentoes.ToList());
        }

        // GET: Orcamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        // GET: Orcamentos/Create
        public ActionResult Create(int? ID)
        {
            var id = RouteData.Values["id"];

            var orcamentoID = Convert.ToInt32(id);

            var totalOrcamento = db.TotalOrcamentoes.Find(orcamentoID);

            var clientes = db.Clientes.ToList();

            var cliente = new Cliente();

            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            var clienteID = totalOrcamento.ClienteID;

            cliente = db.Clientes.Find(clienteID);

            ViewBag.ClienteID = new SelectList(clientes, "ID", "Nome", cliente).SelectedValue;

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            var items = db.Orcamentoes.Where(x => x.TotalOrcamentoID == totalOrcamento.ID).ToList();

            if (items != null && items.Count() > 0)
            {
                ViewBag.cartCount = items.Count();
            }
            else
            {
                ViewBag.cartCount = 0;
            }

            

            var orcamento = new Orcamento();

            orcamento.ClienteID = clienteID;
            orcamento.ClienteNome = cliente.Nome;
            orcamento.TotalOrcamentoID = totalOrcamento.ID;

            return View(orcamento);
        }

        // POST: Orcamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Orcamento orcamento)
        {

            var clienteLista = db.Clientes.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} ", c.Empresa, c.Nome)
            }).ToList();

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            decimal countTotal = 0;

            if (ModelState.IsValid)
            {
                var total = (orcamento.ProdutoUnidade * orcamento.ProdutoTotal);

                orcamento.ValorTotal = total;

                var listaOrcamentos = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamento.TotalOrcamentoID).ToList();

                foreach (var item in listaOrcamentos)
                {
                    countTotal += (item.ProdutoTotal * item.ProdutoUnidade);
                }

                var totalConvertido = Convert.ToDecimal(total);

                countTotal += totalConvertido;

                var totalOrcamento = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

                totalOrcamento.ValorTotal = countTotal;

                db.Entry(totalOrcamento).State = EntityState.Modified;
                db.Orcamentoes.Add(orcamento);
                db.SaveChanges();
            }

            var contador = AddCart(orcamento);

            ViewBag.cartCount = contador;

            var listaOrcamentosFinal = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamento.TotalOrcamentoID).ToList();

            ViewBag.ListaOrcamentos = listaOrcamentosFinal;

            return View(orcamento);
        }


        public ActionResult Print(int? totalOrcamentoID)
        {
            var id = RouteData.Values["id"];

            var orcamentoID = Convert.ToInt32(id);

         

            var totalOrcamentos = db.TotalOrcamentoes.Find(orcamentoID);

            ViewBag.Cliente = totalOrcamentos.ClienteNome;

            ViewBag.ValorTotal = totalOrcamentos.ValorTotal;

         var listaOrcamentos = db.Orcamentoes.Where(x => x.TotalOrcamentoID == orcamentoID).ToList();

            return View(listaOrcamentos);
        }


        public IList<Orcamento> getAllItems()
        {
            IList<Orcamento> orcamentos = new List<Orcamento>();
            orcamentos = db.Orcamentoes.ToList();
            return orcamentos;
        }

        public int AddCart(Orcamento orcamentoObj)
        {

            decimal valorTotal = 0;


            //Orcamento orcamento = new Orcamento()
            //{
            //    ClienteID = orcamentoObj.ClienteID,
            //    ValorTotal = orcamentoObj.ValorTotal,
            //    DataOrcamento = DateTime.Now,
            //    TotalOrcamentoID = orcamentoObj.TotalOrcamentoID,
            //    Descriminação = orcamentoObj.Descriminação,
            //    Quantidade = orcamentoObj.Quantidade,
            //    ProdutoTotal = orcamentoObj.ProdutoTotal,
            //    ProdutoUnidade = orcamentoObj.ProdutoUnidade
            //};

            //db.Orcamentoes.Add(orcamento);
            //db.SaveChanges();

            int count = db.Orcamentoes.Where(s => s.TotalOrcamentoID == orcamentoObj.TotalOrcamentoID).Count();

            return count;
        }

        [HttpPost]
        public ActionResult GetCartItems(int? totalOrcamentoID)
        {

            string x = Request.QueryString["id"];

            var queryString = HttpContext.Request.QueryString.Get("id");


            var idTotalOrcamento = Convert.ToInt32(x);

            decimal sum = 0; 

            //var orcamento = db.Orcamentoes.Find(orcamentoID);

            var GetItems = db.Orcamentoes.Where(s => s.TotalOrcamentoID == totalOrcamentoID).ToList();
            //var GetCartItem = from itemList in db.Orcamentoes where GetItemsId.Contains(itemList.ClienteID) select itemList;

            foreach (var totalsum in GetItems)
            {
                sum = sum + totalsum.ProdutoTotal;
            }

            ViewBag.ValorTotal = sum;

            //return PartialView("_cartItem", GetCartItem);

            //return Json(GetCartItem);

            ViewBag.ListaOrcamentos = GetItems;

            //return RedirectToAction("Create", new { id = totalOrcamentoID });
            //var orcamento = new Orcamento();

            return Json(GetItems);

        }

        public ActionResult DeleteCart(int itemId)
        {
            Orcamento removeCart = db.Orcamentoes.Find(itemId);

            var totalOrcamento = db.TotalOrcamentoes.Find(removeCart.TotalOrcamentoID);

            totalOrcamento.ValorTotal -= removeCart.ProdutoTotal;

            db.Entry(totalOrcamento).State = EntityState.Modified;
            db.Orcamentoes.Remove(removeCart);
            db.SaveChanges();

            return GetCartItems(totalOrcamento.ID);
        }

        [HttpPost]
        public ActionResult GetOrcamentoById(int orcamentoID)
        {
            Orcamento editarOrcamento = db.Orcamentoes.Find(orcamentoID);

            //var totalOrcamento = db.TotalOrcamentoes.Find(editarOrcamento.TotalOrcamentoID);

            //totalOrcamento.ValorTotal -= editarOrcamento.ProdutoTotal;
        

            return Json(editarOrcamento);
        }


        // GET: Orcamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        [HttpPost]
        public ActionResult Edit(int id, int totalOrcamentoID, int produtoTotal, int produtoUnidade, string descriminacao)
        {
            Orcamento orcamento = db.Orcamentoes.Find(id);

            orcamento.TotalOrcamentoID = totalOrcamentoID;
            orcamento.ProdutoTotal = produtoTotal;
            orcamento.ProdutoUnidade = produtoUnidade;
            orcamento.Descriminação = descriminacao;

            var orcamentoTotal = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

            var checkValue = orcamentoTotal.ValorTotal;

            if (ModelState.IsValid)
            {
                orcamento.ValorTotal = (orcamento.ProdutoTotal * orcamento.ProdutoUnidade);

                db.Entry(orcamento).State = EntityState.Modified;
                db.SaveChanges();

                var todosItenns = db.Orcamentoes.Where(x => x.TotalOrcamentoID == totalOrcamentoID).ToList();

                Decimal contador = 0;

                foreach(var item in todosItenns)
                {
                    contador += (item.ProdutoTotal * item.ProdutoUnidade);
                }

                orcamentoTotal.ValorTotal = contador;
              

                db.Entry(orcamento).State = EntityState.Modified;
                db.Entry(orcamentoTotal).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(orcamento);
        }

        // GET: Orcamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orcamento orcamento = db.Orcamentoes.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        // POST: Orcamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Orcamento orcamento = db.Orcamentoes.Find(id);

            var orcamentoTotal = db.TotalOrcamentoes.Find(orcamento.TotalOrcamentoID);

            var valorAtual = orcamento.ValorTotal;

            var removedItem = (orcamento.ProdutoTotal * orcamento.ProdutoUnidade);

            var itemToRemoveValue = (orcamentoTotal.ValorTotal -= orcamento.ValorTotal);

            orcamentoTotal.ValorTotal = itemToRemoveValue;


            db.Entry(orcamentoTotal).State = EntityState.Modified;
            db.Orcamentoes.Remove(orcamento);
            db.SaveChanges();
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
