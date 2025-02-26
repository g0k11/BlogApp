namespace BlogApp.DataApi.Entities
{
    public class Educations
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public string School { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
