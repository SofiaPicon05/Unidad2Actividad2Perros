namespace Unidad2Actividad2Perros.Models.ViewModels
{
    public class PaisViewModels
    {
        public string Nombre { get; set; } = null!;
        public ICollection<RazasModel> Raza { get; set; } = null!;
    }
    public class RazasModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
