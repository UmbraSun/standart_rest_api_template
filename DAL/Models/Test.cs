namespace DAL.Models
{
    public class TestModel : BaseModel<int>
    {
        public int Total { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public List<Partners> Partners { get; set; }
    }
}
