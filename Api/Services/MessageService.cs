using System.Text;
using Contracts.Database;
using Domain.Database;
using Microsoft.EntityFrameworkCore;
using Api.Services.Helpers;

namespace Api.Services;

public class MessageService
{
    private readonly BotDbContext _context;
    private readonly IMessagesToUsers _messagesToUsers;

    public MessageService(BotDbContext botDbContext, IMessagesToUsers messagesToUsers)
    {
        _context = botDbContext;
        _messagesToUsers = messagesToUsers;

    }

    public List<string> CreateMessageForProducts(List<Product> products, int userId)
    {
        List<string> messagesList = new List<string>();

        if (!products.Any())
        {
            messagesList.Add("Вбачаюсь, немає такого продукту в наявності, або щось не те у вас в запиті");
            return messagesList;
        }

        var user = GetUser(userId);

        string message = "";

        foreach (var product in products)
        {
            if (product.Url.Length > 0)
            {
                messagesList.Add(product.Url);
            }

            message = (user.UserRole.IsAdmin)
                ? _messagesToUsers.CreateMessage(product)
                : _messagesToUsers.CreateMessage(product, user);

            messagesList.Add(message);
        }

        return messagesList;
    }

    private User GetUser(int userId)
    {
        var user = _context.Users.Where(u => u.Id.Equals(userId))
            .Include(u => u.UserRole).FirstOrDefault();

        if (user != null) return user;

        return new User()
        {
            Id = userId,
            UserRole = _context.UserRoles.Where(ur => ur.Name == "guest").FirstOrDefault()
        };

    }
}