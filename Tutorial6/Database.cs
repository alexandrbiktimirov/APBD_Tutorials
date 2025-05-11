using Tutorial6.Models;

namespace Tutorial6;

public static class Database
{
    public static List<Animal> Animals = new List<Animal>()
    {
        new Animal() { Id = 1, Name = "Animal 1", Category = "Cat", Weight = 3, FurColor = "Black"},
        new Animal() { Id = 2, Name = "Animal 2", Category = "Dog", Weight = 10, FurColor = "White" },
        new Animal() { Id = 3, Name = "Animal 3", Category = "Horse", Weight = 20, FurColor = "Brown" },
    };
    
    public static readonly List<Visit> Visits = new List<Visit>()
    {
        new Visit() { Id = 1, Animal = 1, Description = "Description of visit 1", Price = 100},
        new Visit() { Id = 2, Animal = 2, Description = "Description of visit 2", Price = 200},
        new Visit() { Id = 3, Animal = 3, Description = "Description of visit 3", Price = 300},
    };
}