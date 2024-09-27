namespace ASP.NET___CRUD.Models
{
    public class Joke
    {
        // prop
        public int Id { get; set; }
        public String JokeQuestion { get; set; } = string.Empty;
        public String JokeAnswer { get; set; } = string.Empty;

        // ctor
        public Joke()
        {
            
        }
    }
}
