using Pft.Domain.Entities.Users;

namespace Pft.Infrastructure.Repositories;
internal sealed class UserRepository(ApplicationDbContext dbContext)
    : Repository<User, UserId>(dbContext), IUserRepository
{
    public override void Add(User user)
    {
        //This will tell EF Core that any roles present on our user object are already inside of the database and you don't
        //need to insert them again 
        foreach (var role in user.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(user);
    }
}
