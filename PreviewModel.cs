using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmsInfoBot.Model
{
    public class PreviewModel
    {
        public List<Film> Results { get; set; }
    }
    public class Film
    {
        public int ID { get; set; }
        public string? Release_date { get; set; }
        public string Title { get; set; }
    }
}
