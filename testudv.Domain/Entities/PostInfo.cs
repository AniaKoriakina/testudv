namespace testudv.Domain.Entities;

public class PostInfo
{
    public int PostId { get; set; }
    public string Domain { get; set; }
    public int Count {get; set;}
    public Dictionary<char, int> LettersCount { get; set; } = new();

    public PostInfo()
    {
        LettersCount = new Dictionary<char, int>();
    }
    
    public PostInfo(string domain, int count, Dictionary<char, int> lettersCount)
    {
        Domain = domain;
        Count = count;
        LettersCount = lettersCount;
    }
}