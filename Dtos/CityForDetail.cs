using System.Collections.Generic;
using DeepHist.Entity.Concrete;

namespace DeepHist.Web.API.Dtos
{
    public class CityForDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
