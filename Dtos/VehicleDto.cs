using System.Collections.Generic;

namespace ElectronALPR.Dtos
{
      public class VehicleDto
      {
            public List<DetailDto> color { get; set; }

            public List<DetailDto> make { get; set; }

            public List<DetailDto> body_type { get; set; }

            public List<DetailDto> year { get; set; }

            public List<DetailDto> make_model { get; set; }
      }
}