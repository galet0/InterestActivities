using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCMovie.Models.BindingModels
{
    public class EditMovieBindingModel : MovieBindingModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
    }
}
