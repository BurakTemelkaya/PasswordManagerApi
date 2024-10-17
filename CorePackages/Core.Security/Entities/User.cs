using Core.Persistence.Repositories;

namespace Core.Security.Entities;

public class User<TId> : Entity<TId>
{
    public string Email { get; set; }

    public User()
    {
        Email = string.Empty;
    }

    public User(string email)
    {
        Email = email;
    }

    public User(TId id, string email)
        : base(id)
    {
        Email = email;
    }
}
