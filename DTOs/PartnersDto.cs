namespace DTOs
{
    public class PartnersDto
    {
        public class Add
        {
            public string Name { get; set; }

            public int Count { get; set; }
        }

        public class AddResponce
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
