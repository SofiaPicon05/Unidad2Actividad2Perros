using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using Unidad2Actividad2Perros.Models.Entities;
using Unidad2Actividad2Perros.Models.ViewModels;

namespace Unidad2Actividad2Perros.Controllers
{
    public class HomeController : Controller
    {
        public readonly PerrosContext context = new();
        public HomeController()
        {
            LlenarAbecedario();
        }
        Random R = new();
        private  char[] abecedario = new char[26];
        List<OtrosPerros> ListaPerros { get; set; } = new();
        void LlenarAbecedario()
        {
            byte b = 65;
            for (int i = 0; i < 26; i++)
            {
                abecedario[i] += (char)b;
                b++;
            }
        }
        //[Route("/{letra}")]
        public IActionResult Index(string Id)
        {
            
            IndexViewModels vm = new();
            if (Id == null)
            {

                var datos = context.Razas
                    .OrderBy(x => x.Nombre)
                    .Select(x => new PerrosModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                    }).ToList();
                vm.Perros = datos;
                vm.Abecedario = abecedario;
            }
            else
            {

            var perros = context.Razas
                .Where(x => x.Nombre.StartsWith(Id))
                .OrderBy(x => x.Nombre)
                .Select(x => new PerrosModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                }).ToList();
                vm.Perros = perros;
                vm.Abecedario = abecedario;
                
            }

            
            
            return View(vm);
        }

        
        //public IActionResult Index(string filtro)
        //{
        //    var datos = context.Razas
        //        .Where(x => x.Nombre.StartsWith(filtro))
        //        .OrderBy(x => x.Nombre)
        //        .Select(x => new PerrosModel
        //        {
        //            Id = x.Id,
        //            Nombre = x.Nombre,
        //        }).ToList();

        //    IndexViewModels vm = new()
        //    {
        //        Perros = datos,
        //        Abecedario = abecedario
        //    };
        //    return View(vm);
        //}

        [Route("/Paises")]
        public IActionResult Pais()
        {
            var datos = context.Paises.OrderBy(x => x.Nombre).Select(x => new PaisViewModels
            {
                Nombre = x.Nombre ?? "",
                Raza = x.Razas.OrderBy(r => r.Nombre).Select(r => new RazasModel
                {
                    Id = (int)r.Id,
                    Nombre = r.Nombre
                }).ToList()
            }).AsEnumerable();
            return View(datos);
        }

        [Route("/Perro/{id}")]
        public IActionResult Raza(string id)
        {
            id = id.Replace('-', ' ');
            var datos = context.Razas
                .Where(x => x.Nombre == id)
                .Select(x => new DetallesViewModels
                {
                    Id = x.Id,
                    Nombre = x.Nombre ?? "",
                    AlturaMaxima = x.AlturaMax,
                    AlturaMinima = x.AlturaMin,
                    Descripcion = x.Descripcion ?? "",
                    EsperanzaDVida = x.EsperanzaVida,
                    OtrosNombres = x.OtrosNombres ?? "No Tiene",
                    Pais = x.IdPaisNavigation != null ? (x.IdPaisNavigation.Nombre ?? "") : "",
                    PesoMaximo = x.PesoMax,
                    PesoMinimo = x.PesoMin,
                    Estadistica = new EstadisticasModel
                    {
                        AmistadDesconocidos = x.Estadisticasraza != null ? x.Estadisticasraza.AmistadDesconocidos : 0,
                        AmistadPerros = x.Estadisticasraza != null ? x.Estadisticasraza.AmistadPerros : 0,
                        EjercicioObligatorio = x.Estadisticasraza != null ? x.Estadisticasraza.EjercicioObligatorio : 0,
                        FacilidadEntrenamiento = x.Estadisticasraza != null ? x.Estadisticasraza.FacilidadEntrenamiento : 0,
                        NecesidadDCepillado = x.Estadisticasraza != null ? x.Estadisticasraza.NecesidadCepillado : 0,
                        NivelEnergia = x.Estadisticasraza != null ? x.Estadisticasraza.NivelEnergia : 0
                    },
                    Caracteristica = new CaracteristicasModel
                    {
                        Cola = x.Caracteristicasfisicas != null ? x.Caracteristicasfisicas.Cola ?? "" : "",
                        Color = x.Caracteristicasfisicas != null ? x.Caracteristicasfisicas.Color ?? "" : "",
                        Hocico = x.Caracteristicasfisicas != null ? x.Caracteristicasfisicas.Hocico ?? "" : "",
                        Patas = x.Caracteristicasfisicas != null ? x.Caracteristicasfisicas.Patas ?? "" : "",
                        Pelo = x.Caracteristicasfisicas != null ? x.Caracteristicasfisicas.Pelo ?? "" : "",
                    },
                    OtrosPerros = null!
                })
                .First();
            LlenarPerrosRandom(id);
            datos.OtrosPerros = ListaPerros;
            return View(datos);
        }

        private void LlenarPerrosRandom(string nombre)
        {
            ListaPerros.Clear();
            var datos = context.Razas.Where(x => x.Nombre != nombre).Select(x => new OtrosPerros
            {
                Id = x.Id,
                Nombre = x.Nombre
            }).ToList();

            for (int i = 0; i < 4; i++)
            {
                int a = R.Next(0, datos.Count);
                OtrosPerros otros = datos[a];
                if (!ListaPerros.Contains(otros))
                    ListaPerros.Add(otros);
            }
        }
    }
}
