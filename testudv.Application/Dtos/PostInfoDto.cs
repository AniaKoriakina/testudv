namespace testudv.Application.Dtos;

public class PostInfoDto
{
    public string Domain { get; set; }
    public int Count { get; set; }
    public Dictionary<char, int> LettersCount { get; set; }
    
    public PostInfoDto(string domain, int count, Dictionary<char, int> lettersCount)
    {
        Domain = domain;
        Count = count;
        LettersCount = lettersCount;
    }
}