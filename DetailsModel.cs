using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmsInfoBot.Model
{
    public class DetailsModel
    {
        public List<Genre> Genres { get; set; }
        public int ID { get; set; }
        public string? Overview { get; set; }
        public string? Poster_path { get; set; }
        public List<Company> Production_companies { get; set; }
        public List<Country> Production_countries { get; set; }
        public string Release_date { get; set; }
        public int Runtime { get; set; }
        public string? Tagline { get; set; }
        public string? Title { get; set; }
        public double Vote_average { get; set; }
    }
    public class Genre
    {
        public string Name { get; set; }
    }
    public class Country
    {
        public string Name { get; set; }
    }
    public class Company
    {
        public string Name { get; set; }
    }
}
