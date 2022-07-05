namespace FilmsInfoBot.Model
{
    public class GenresModel
    {
        public List<GenreInfo> genres { get; set; }
    }
    public class GenreInfo
    {        
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
