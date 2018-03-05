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
using ProyectoIntegrador.ViewModels;

namespace ProyectoIntegrador.Controllers
{
    public class PreguntasController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        //GET TEMAS
        public static List<Tema> getTemasFromController()
        {
            TemasController c = new TemasController();
            List<Tema> result = (List<Tema>)c.list_temas();

            return result;
        }

        // GET: Temas
        public List<Pregunta> list_preguntas()
        {
            return db.Pregunta.ToList();
        }

        // GET: Preguntas
        public ActionResult Index()
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            var pregunta = db.Pregunta.Include(p => p.Usuario).Include(t => t.Tema);
            return View(pregunta.ToList());
        }



        public ActionResult Contestar()
        {
            var pregunta = db.Pregunta.Include(p => p.Usuario).Include(t => t.Tema);
            return View(pregunta.ToList());
        }

        // GET: Preguntas/Details/5
        public ActionResult Details(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // GET: Preguntas/Create
        public ActionResult Create()
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre");
            ViewBag.Temas = getTemasFromController();
            return View();
        }

        public ActionResult CreaPreguntas()
        {
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: Preguntas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(String pregunta, int tipo, int dificultad, String tema, String texto, int id, string imageFileURL)
        {
            //ICollection<Tema> temasLista = new List<Tema>();
            var temas = db.Tema.Select(a=>a.NombreTema).ToList();
            /*foreach (String t in temas)
            {
                temasLista.Add(db.Tema.FirstOrDefault(i => i.ClaveTema == tema));

            }*/

            bool valid = true;
            if ((((tipo == 0) || (dificultad.Equals(""))) || (texto.Equals(""))) || (pregunta.Equals("")))
                valid = false;

            if (valid)
            {
                Pregunta p = new Pregunta();
                p.Texto = texto;
                p.Tipo = (short) tipo;
                p.Id = id;
                p.Tema = db.Tema.Where(a=>a.ClaveTema == tema).ToList();
                p.Dificultad = (short) dificultad;
                //p.Imagen = System.IO.File.ReadAllBytes("C: \\"+ImageFile);
                if ((imageFileURL != null)) {
                    WebClient client = new WebClient();
                    byte[] img = null;
                    try
                    {
                        img = client.DownloadData(imageFileURL);
                    }catch(Exception e)
                    {

                    }
                    if(img!=null)
                        p.Imagen = img;
                 }

                    if (tipo == 1)
                {
                    JArray array = JArray.Parse(JObject.Parse(pregunta).Properties().FirstOrDefault().Value.ToString());
                    string ctr = "";
                    string cr = "";

                    var i = 0;
                    foreach (JObject obj in array.Children<JObject>())
                    {
                        string itr = "";
                        string ir = "";
                        string ie = "";

                        foreach (JProperty singleProp in obj.Properties())
                        {
                            string name = singleProp.Name;
                            string value = singleProp.Value.ToString();

                            if (i == 0)
                            {
                                if (name.Equals("ctr"))
                                    ctr = value;
                                if (name.Equals("cr"))
                                    cr = value;
                            }
                            else
                            {
                                if (name.Equals("itr"))
                                    itr = value;
                                if (name.Equals("ir"))
                                    ir = value;
                                if (name.Equals("ie"))
                                    ie = value;
                            }
                        }

                        if (i!=0)
                        {
                            RespuestasErroneas re = new RespuestasErroneas();
                            re.ID_Pregunta = p.IDPregunta;
                            re.Explicacion = ie;
                            re.Opcion = itr;
                            re.TextoRespuesta = ir;
                            db.RespuestasErroneas.Add(re);
                        }
                        i++;
                    }

                    p.RespuestaCorrecta = ctr;
                    p.TextoRespuesta = cr;

                    db.Pregunta.Add(p);
                    db.SaveChanges();
                    return Json(new { url = "/Preguntas/Index" });
                }
                else if (tipo == 2)
                {
                    JObject obj = JObject.Parse(pregunta);

                    string textoRespuestaV = "";
                    string textoRespuestaF = "";
                    string explicacion = "";
                    string respuesta = "";

                    foreach (JProperty singleProp in obj.Properties())
                    {
                        string name = singleProp.Name;
                        string value = singleProp.Value.ToString();

                        if (name.Equals("v"))
                            textoRespuestaV = value;
                        if (name.Equals("f"))
                            textoRespuestaF = value;
                        if (name.Equals("respuesta"))
                            respuesta = value;
                        if (name.Equals("explicacion"))
                            explicacion = value;
                    }

                    RespuestasErroneas re = new RespuestasErroneas();
                    re.ID_Pregunta = p.IDPregunta;
                    re.Explicacion = explicacion;

                    if (Int32.Parse(respuesta) == 0) {
                        p.RespuestaCorrecta = textoRespuestaF;
                        p.TextoRespuesta = "Falso";
                        re.Opcion = textoRespuestaV;
                        re.TextoRespuesta = "Verdadero";
                    }
                    else if (Int32.Parse(respuesta) == 1)
                    {
                        p.RespuestaCorrecta = textoRespuestaV;
                        p.TextoRespuesta = "Verdadero";
                        re.Opcion = textoRespuestaF;
                        re.TextoRespuesta = "Falso";
                    }

                    db.RespuestasErroneas.Add(re);
                    db.Pregunta.Add(p);
                    db.SaveChanges();
                    return Json(new { url = "/Preguntas/Index" });
                }
            }
            
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", id);
            return View(new Pregunta {Id = id });
        }

        // GET: Preguntas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", pregunta.Id);
            ViewBag.Temas = getTemasFromController();
            ViewBag.TemaSelected = pregunta.Tema.FirstOrDefault().NombreTema;

            List<RespuestasErroneas> respuestasErroneas = db.RespuestasErroneas.Where(a => a.ID_Pregunta == pregunta.IDPregunta).ToList();

            PreguntaRespuestasVM vm = new PreguntaRespuestasVM();
            vm.pregunta = pregunta;
            vm.respuestasErroneas = respuestasErroneas;

            return View(vm);
        }

        // POST: Preguntas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string pregunta, int tipo, int dificultad, string texto, int idPregunta, string imageFileURL/*, string tema*/)
        {

            bool valid = true;
            if ((((tipo == 0) || (dificultad.Equals(""))) || (texto.Equals(""))) || (pregunta.Equals("")))
                valid = false;

            if (valid)
            {
                Pregunta p = db.Pregunta.Where(a => a.IDPregunta == idPregunta).FirstOrDefault();
                p.Texto = texto;
                p.Tipo = (short)tipo;
                //p.Id = id;
                //p.Tema = db.Tema.Where(a => a.ClaveTema == tema).ToList();
                p.Dificultad = (short)dificultad;
                //p.Imagen = System.IO.File.ReadAllBytes("C: \\"+ImageFile);
                if ((imageFileURL != null))
                {
                    WebClient client = new WebClient();
                    byte[] img = null;
                    try
                    {
                        img = client.DownloadData(imageFileURL);
                    }
                    catch (Exception e)
                    {

                    }
                    if (img != null)
                        p.Imagen = img;
                }

                if (tipo == 1)
                {
                    //borrar respuestas erroneas anteriores
                    List<int> respuestasErroneas = db.RespuestasErroneas.Where(a => a.ID_Pregunta == idPregunta).Select(b=>b.Consecutivo).ToList();

                    foreach(var x in respuestasErroneas)
                    {
                        RespuestasErroneas r = db.RespuestasErroneas.Where(a => a.Consecutivo == x).FirstOrDefault();
                        db.RespuestasErroneas.Remove(r);
                    }


                    //Guardar la informacion
                    JArray array = JArray.Parse(JObject.Parse(pregunta).Properties().FirstOrDefault().Value.ToString());
                    string ctr = "";
                    string cr = "";

                    var i = 0;
                    foreach (JObject obj in array.Children<JObject>())
                    {
                        string itr = "";
                        string ir = "";
                        string ie = "";

                        foreach (JProperty singleProp in obj.Properties())
                        {
                            string name = singleProp.Name;
                            string value = singleProp.Value.ToString();

                            if (i == 0)
                            {
                                if (name.Equals("ctr"))
                                    ctr = value;
                                if (name.Equals("cr"))
                                    cr = value;
                            }
                            else
                            {
                                if (name.Equals("itr"))
                                    itr = value;
                                if (name.Equals("ir"))
                                    ir = value;
                                if (name.Equals("ie"))
                                    ie = value;
                            }
                        }

                        if (i != 0)
                        {
                            RespuestasErroneas re = new RespuestasErroneas();
                            re.ID_Pregunta = p.IDPregunta;
                            re.Explicacion = ie;
                            re.Opcion = itr;
                            re.TextoRespuesta = ir;
                            db.RespuestasErroneas.Add(re);
                        }
                        i++;
                    }

                    p.RespuestaCorrecta = ctr;
                    p.TextoRespuesta = cr;

                    //db.Pregunta.Add(p);
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { url = "/Preguntas/Index" });
                }
                else if (tipo == 2)
                {
                    //borrar respuestas erroneas anteriores
                    List<int> respuestasErroneas = db.RespuestasErroneas.Where(a => a.ID_Pregunta == idPregunta).Select(b => b.Consecutivo).ToList();

                    foreach (var x in respuestasErroneas)
                    {
                        RespuestasErroneas r = db.RespuestasErroneas.Where(a => a.Consecutivo == x).FirstOrDefault();
                        db.RespuestasErroneas.Remove(r);
                    }

                    //Nueva informacion
                    JObject obj = JObject.Parse(pregunta);

                    string textoRespuestaV = "";
                    string textoRespuestaF = "";
                    string explicacion = "";
                    string respuesta = "";

                    foreach (JProperty singleProp in obj.Properties())
                    {
                        string name = singleProp.Name;
                        string value = singleProp.Value.ToString();

                        if (name.Equals("v"))
                            textoRespuestaV = value;
                        if (name.Equals("f"))
                            textoRespuestaF = value;
                        if (name.Equals("respuesta"))
                            respuesta = value;
                        if (name.Equals("explicacion"))
                            explicacion = value;
                    }

                    RespuestasErroneas re = new RespuestasErroneas();
                    re.ID_Pregunta = p.IDPregunta;
                    re.Explicacion = explicacion;

                    if (Int32.Parse(respuesta) == 0)
                    {
                        p.RespuestaCorrecta = textoRespuestaF;
                        p.TextoRespuesta = "Falso";
                        re.Opcion = textoRespuestaV;
                        re.TextoRespuesta = "Verdadero";
                    }
                    else if (Int32.Parse(respuesta) == 1)
                    {
                        p.RespuestaCorrecta = textoRespuestaV;
                        p.TextoRespuesta = "Verdadero";
                        re.Opcion = textoRespuestaF;
                        re.TextoRespuesta = "Falso";
                    }

                    db.RespuestasErroneas.Add(re);
                    //db.Pregunta.Add(p);
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { url = "/Preguntas/Index" });
                }
            }

            /*if (ModelState.IsValid)
            {
                db.Entry(pregunta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }*/
            ViewBag.Id = new SelectList(db.Usuario, "Id", "Nombre", idPregunta);

            PreguntaRespuestasVM vm = new PreguntaRespuestasVM();
            vm.pregunta = db.Pregunta.Find(idPregunta);
            vm.respuestasErroneas = db.RespuestasErroneas.Where(a => a.ID_Pregunta == idPregunta).ToList();

            return View(vm);
        }

        // GET: Preguntas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pregunta pregunta = db.Pregunta.Find(id);
            List<int> re = db.RespuestasErroneas.Where(a => a.Pregunta.IDPregunta == pregunta.IDPregunta).Select(b=>b.Consecutivo).ToList();

            foreach (var consec in re)
            {
                var respIncorrecta = db.RespuestasErroneas.Where(a => a.Consecutivo == consec).FirstOrDefault();
                db.RespuestasErroneas.Remove(respIncorrecta);
            }

            Tema tema = pregunta.Tema.FirstOrDefault();
            List<QuestionInExam> q = pregunta.QuestionInExam.ToList();
            foreach(var item in q)
            {
                db.QuestionInExam.Remove(item);
            }

            db.Pregunta.Remove(pregunta);
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
