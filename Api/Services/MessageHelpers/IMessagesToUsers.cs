using Contracts.Database;

namespace Api.Services.Helpers;

public interface IMessagesToUsers
{
    public string CreateMessage(Product product);
    public string CreateMessage(Product product, User user);
}