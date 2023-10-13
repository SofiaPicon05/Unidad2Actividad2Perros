namespace Unidad2Actividad2Perros.Models.ViewModels
{
    public class IndexViewModels
    {
        public char[] Abecedario { get; set; } = null!;
        public ICollection<PerrosModel> Perros { get; set; } = null!;
    }
    public class PerrosModel
    {
        public uint Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
