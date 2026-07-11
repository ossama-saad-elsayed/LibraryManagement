namespace LibraryManagement.DTOS
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Biography { get; set; }

        public List<string> BookTitles { get; set; } = new List<string>();
    }

    public class CreateAuthorDto
    {
        public string Name { get; set; } = null!;
        public string? Biography { get; set; }
    }

}
