using System.ComponentModel.DataAnnotations;

namespace ServerBuilding.DTO
{
    public class CakeDTO
    {
        public int CakeId { get; set; }

        public string CakeType { get; set; } = null!;

    }
}
