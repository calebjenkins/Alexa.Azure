namespace Azure4Alexa.Models
{
    public class Model
    {
        public string Type { get; set; }

        public ModelValue Value { get; set; }
    }

    public class ModelValue
    {
        public int Id { get; set; }

        public string Joke { get; set; }

        public string[] Categories { get; set; }
    }
}