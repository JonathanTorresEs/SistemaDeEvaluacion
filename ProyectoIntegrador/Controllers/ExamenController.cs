using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using ProyectoIntegrador.ViewModels;

namespace ProyectoIntegrador.Controllers
{
    public class ExamenController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        public static List<Tema> getTemasFromController()
        {
            TemasController c = new TemasController();
            List<Tema> result = (List<Tema>)c.list_temas();

            return result;
        }

        // GET: Examen
        public ActionResult Index()
        {
            var examen = db.Examen.Include(e => e.Usuario);
            return View(examen.ToList());
        }

        public ActionResult examenes_disponibles_alumno()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie c = HttpContext.Request.Cookies.Get("matricula");
            var examenes = db.Alumno.Where(a => a.Matricula == c.Value).FirstOrDefault().Examen;
            return View(examenes);
        }

        public ActionResult view_user()
        {
            var examen = db.Examen.Include(e => e.Usuario);
            return View(examen.ToList());
        }

        // GET: Examen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examen.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // GET: Examen/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre");
            ViewBag.Temas = getTemasFromController();
            return View();
        }

        // POST: Examen/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDExamen,Nombre,Descripcion,FechaDeCreacion,Id")] Examen examen, List<String> temas)
        {
            ICollection<Tema> temasLista = new List<Tema>();
            foreach(String tema in temas)
            {
                temasLista.Add(db.Tema.FirstOrDefault(i => i.ClaveTema == tema));

            }
            if (ModelState.IsValid)
            {
                examen.Tema = temasLista;
                db.Examen.Add(examen);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", examen.Id);
            return View(examen);
        }

        // GET: Examen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Examen examen = db.Examen.Find(id);
            String ids = "";
            var i = 0;
            foreach (Tema tema in examen.Tema)
            {
                if (i == 0)
                {
                    ids = tema.ClaveTema;
                }
                else
                {
                    ids = ids + "," + tema.ClaveTema;
                }
                i++;
            }
            ViewBag.selectedTemas = ids;
            ViewBag.Temas = getTemasFromController();
            
            if (examen == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", examen.Id);
            return View(examen);
        }

        public ActionResult Contestar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examen.Find(id);
            String ids = "";
            var i = 0;
            foreach (Tema tema in examen.Tema)
            {
                if (i == 0)
                {
                    ids = tema.ClaveTema;
                }
                else
                {
                    ids = ids + "," + tema.ClaveTema;
                }
                i++;
            }
            ViewBag.selectedTemas = ids;
            ViewBag.Temas = getTemasFromController();
            if (examen == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", examen.Id);
            return View(examen);
        }

        // POST: Examen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contestar([Bind(Include = "IDExamen,Nombre,Descripcion,CreationDate,Id")] Examen examen, List<String> temas)
        {
            ICollection<Tema> temasLista = new List<Tema>();
            if (temas != null)
            {
                foreach (String tema in temas)
                {
                    temasLista.Add(db.Tema.FirstOrDefault(i => i.ClaveTema == tema));
                }
            }

            if (ModelState.IsValid)
            {
                var prev = db.Examen.FirstOrDefault(i => i.IDExamen == examen.IDExamen);

                var added = new List<Tema>();
                var removed = new List<Tema>();
                var kept = new List<Tema>();


                //check if a new item was added else kept
                foreach (Tema tema in temasLista)
                {
                    if (!prev.Tema.Contains(tema))
                    {
                        added.Add(tema);
                    }
                    else
                    {
                        kept.Add(tema);
                    }
                }

                //check if an item was removed
                foreach (Tema tema in prev.Tema)
                {
                    if (!examen.Tema.Contains(tema))
                    {
                        removed.Add(tema);
                    }
                }



                // System.Diagnostics.Debug.WriteLine(examen.Tema);
                prev.Tema = added.Concat(kept).ToList();
                prev.Nombre = examen.Nombre;
                prev.Descripcion = examen.Descripcion;
                prev.Id = examen.Id;
                //System.Diagnostics.Debug.WriteLine(temasLista);
                //System.Diagnostics.Debug.WriteLine(examen.Tema);
                //db.Entry(examen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", examen.Id);
            return View(examen);
        }*/


        [HttpPost]
        public ActionResult Contestar(String respuestas, int IdExamen, String Matricula)
        {
            JArray array = JArray.Parse(respuestas);

            foreach (JObject obj in array.Children<JObject>())
            {
                string idPregunta = "";
                string respuesta = "";

                foreach (JProperty singleProp in obj.Properties())
                {
                    string name = singleProp.Name;
                    string value = singleProp.Value.ToString();

                    if (name.Equals("preguntaID"))
                        idPregunta = value;
                    if (name.Equals("respuesta"))
                        respuesta = value;

                }

                QuestionInExam q = new QuestionInExam();
                q.IDExamen = IdExamen;
                q.Matricula = Matricula;
                q.IDPregunta = Int32.Parse(idPregunta);
                q.Respuesta = respuesta;

                db.QuestionInExam.Add(q);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { ExamenContestado = true });
            }

            var alumno = db.Alumno.Where(a => a.Matricula == Matricula).FirstOrDefault();
            return Json(new { url = "/Examen/examenes_disponibles_alumno" });
        }

        public ActionResult Contestados()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            HttpCookie c = HttpContext.Request.Cookies.Get("matricula");
            var alumno = db.Alumno.Where(m => m.Matricula == c.Value).FirstOrDefault();

            List<int> q = db.QuestionInExam.Where(a => a.Matricula == alumno.Matricula).Select(b=>b.IDExamen).Distinct().ToList();

            List<Examen> examenes = new List<Examen>();

            foreach(var e in q)
            {
                examenes.Add(db.Examen.Where(a => a.IDExamen == e).FirstOrDefault());
            }

            ExamenesContestadosVM vm = new ExamenesContestadosVM();
            vm.alumno = alumno;
            vm.examenes = examenes;

            return View(vm);
        }

        public ActionResult VerRetro(int id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            HttpCookie c = HttpContext.Request.Cookies.Get("matricula");

            var alumno = db.Alumno.Where(m => m.Matricula == c.Value).FirstOrDefault();

            Examen examen = db.Examen.Where(a => a.IDExamen == id).FirstOrDefault();
            List<Tema> temas = examen.Tema.ToList();

            List<RetroPreguntaVM> vmpList = new List<RetroPreguntaVM>();
            var total = 0.0;
            var acierto = 0.0;
            foreach (var t in temas)
            {
                List<Pregunta> listaPreguntas = t.Pregunta.ToList();
                foreach (var lp in listaPreguntas)
                {
                    Pregunta p = lp;
                    QuestionInExam respuesta = lp.QuestionInExam.Where(a => a.Matricula == alumno.Matricula && a.IDExamen == examen.IDExamen).FirstOrDefault();

                    string explicacion = "";
                    string respuestaInciso = respuesta.Respuesta;
                    string respuestaTexto = "";
                    if (p.RespuestaCorrecta != respuesta.Respuesta)
                    {
                        List<RespuestasErroneas> respuestasErroneas = p.RespuestasErroneas.ToList();
                        foreach (var re in respuestasErroneas)
                        {
                            if (respuesta.Respuesta == re.Opcion)
                            {
                                explicacion = re.Explicacion;
                                respuestaTexto = re.TextoRespuesta;
                            }
                        }
                    }
                    else
                    {
                        respuestaTexto = p.TextoRespuesta;
                        acierto++;
                    }
                        

                    total++;
                    RetroPreguntaVM rpvm = new RetroPreguntaVM();
                    rpvm.pregunta = p;
                    rpvm.respuestaInciso = respuestaInciso;
                    rpvm.respuestaTexto = respuestaTexto;
                    rpvm.explicacion = explicacion;

                    vmpList.Add(rpvm);
                }
            }

            RetroExamenVM vm = new RetroExamenVM();
            vm.examen = examen;
            vm.temas = temas;
            vm.preguntas = vmpList;
            vm.calificacion = Math.Round((acierto / total)*100,2);
            return View(vm);
        }

        public bool IsAuthenticated()
        {
            HttpCookie c = HttpContext.Request.Cookies.Get("matricula");
            if (c != null)
            {
                if (db.Alumno.Where(m => m.Matricula == c.Value).FirstOrDefault() != null)
                    return true;
            }

            return false;
        }

        // POST: Examen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IDExamen,Nombre,Descripcion,CreationDate,Id")] Examen examen, List<String> temas)
        //{
        //    ICollection<Tema> temasLista = new List<Tema>();
        //    if (temas != null)
        //    {
        //        foreach (String tema in temas)
        //        {
        //            temasLista.Add(db.Tema.FirstOrDefault(i => i.ClaveTema == tema));
        //        }
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var prev = db.Examen.FirstOrDefault(i => i.IDExamen == examen.IDExamen);
                
        //        var added = new List<Tema>();
        //        var removed = new List<Tema>();
        //        var kept = new List<Tema>();
                
               
        //        //check if a new item was added else kept
        //        foreach (Tema tema in temasLista) {
        //            if (!prev.Tema.Contains(tema))
        //            {
        //                    added.Add(tema);
        //            }
        //            else
        //            {
        //                kept.Add(tema);
        //            }
        //        }

        //        //check if an item was removed
        //        foreach (Tema tema in prev.Tema) {
        //            if (!examen.Tema.Contains(tema))
        //            {
        //            removed.Add(tema);
        //            }
        //        }
                

            
        //    // System.Diagnostics.Debug.WriteLine(examen.Tema);
        //    prev.Tema = added.Concat(kept).ToList();
        //        prev.Nombre = examen.Nombre;
        //        prev.Descripcion = examen.Descripcion;
        //        prev.Id = examen.Id;
        //        //System.Diagnostics.Debug.WriteLine(temasLista);
        //        //System.Diagnostics.Debug.WriteLine(examen.Tema);
        //        //db.Entry(examen).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", examen.Id);
        //    return View(examen);
        //}

        public ActionResult ListaResultados()
        {
            var alumnos = db.Alumno.ToList();
            List<Resultados> vm = new List<Resultados>();

            foreach (var alumno in alumnos)
            {
                List<Examen> ExamenList = alumno.Examen.ToList();

                foreach (var examen in ExamenList)
                {
                    bool respondido = true;
                    List<Tema> temas = examen.Tema.ToList();
                    var total = 0.0;
                    var acierto = 0.0;

                    foreach (var t in temas)
                    {
                        List<Pregunta> listaPreguntas = t.Pregunta.ToList();
                        foreach (var lp in listaPreguntas)
                        {
                            Pregunta p = lp;
                            QuestionInExam respuesta = lp.QuestionInExam.Where(a => a.Matricula == alumno.Matricula && a.IDExamen == examen.IDExamen).FirstOrDefault();

                            if (respuesta == null)
                                respondido = false;
                            if (respondido)
                            {
                                string explicacion = "";
                                string respuestaInciso = respuesta.Respuesta;
                                string respuestaTexto = "";
                                if (p.RespuestaCorrecta != respuesta.Respuesta)
                                {
                                    List<RespuestasErroneas> respuestasErroneas = p.RespuestasErroneas.ToList();
                                    foreach (var re in respuestasErroneas)
                                    {
                                        if (respuesta.Respuesta == re.Opcion)
                                        {
                                            explicacion = re.Explicacion;
                                            respuestaTexto = re.TextoRespuesta;
                                        }
                                    }
                                }
                                else
                                {
                                    respuestaTexto = p.TextoRespuesta;
                                    acierto++;
                                }


                                total++;
                            }
                        }
                    }

                    if (respondido)
                    {
                        Resultados r = new Resultados();
                        r.examen = examen;
                        r.alumno = alumno;
                        r.calificacion = Math.Round((acierto / total) * 100, 2);

                        vm.Add(r);
                    }
                }
            }
            return View(vm);
        }

        // GET: Examen/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Examen examen = db.Examen.Find(id);
        //    if (examen == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(examen);
        //}

        //// POST: Examen/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Examen examen = db.Examen.Find(id);
        //    db.Examen.Remove(examen);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool IsAuthorized()
        {
            HttpCookie c = HttpContext.Request.Cookies.Get("u");
            if (c != null)
            {
                if (c.Value.Equals("false"))
                    return false;
                else if (c.Value.Equals("true"))
                    return true;
            }

            return false;
        }
    }
}
