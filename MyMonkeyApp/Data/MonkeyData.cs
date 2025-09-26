using MyMonkeyApp.Models;

namespace MyMonkeyApp.Data;

public static class MonkeyData
{
    public static IReadOnlyList<Monkey> All { get; } = new List<Monkey>
    {
        new()
        {
            Name = "Capuchin Monkey",
            ScientificName = "Cebinae",
            Region = "Central & South America",
            Description = "Small, intelligent; often seen in media; adaptable omnivore."
        },
        new()
        {
            Name = "Howler Monkey",
            ScientificName = "Alouatta",
            Region = "Central & South America",
            Description = "Known for extremely loud howls that can travel several miles."
        },
        new()
        {
            Name = "Spider Monkey",
            ScientificName = "Ateles",
            Region = "Central & South America",
            Description = "Long limbs & prehensile tail used like a fifth arm for agility."
        },
        new()
        {
            Name = "Proboscis Monkey",
            ScientificName = "Nasalis larvatus",
            Region = "Borneo",
            Description = "Distinctive large nose; excellent swimmer; lives near mangroves."
        },
        new()
        {
            Name = "Golden Snub-nosed Monkey",
            ScientificName = "Rhinopithecus roxellana",
            Region = "China (mountain forests)",
            Description = "Bright golden fur; cold-adapted; lives in multi-level social groups."
        },
        new()
        {
            Name = "Mandrill",
            ScientificName = "Mandrillus sphinx",
            Region = "Central Africa",
            Description = "Brightly colored face; powerful; lives in large groups called hordes."
        }
    };
}
