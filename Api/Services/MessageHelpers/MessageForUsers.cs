using System.Text;
using Contracts.Database;
using Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Helpers;

public class MessageForUsers : IMessagesToUsers
{
    public string CreateMessage(Product product)
    {
        StringBuilder messageBuilder = new StringBuilder();

        messageBuilder.Append("Ім'я: " + product.Name + "\n");
        product.Variations.Where(x => x.Quantity != 0).ToList()
            .ForEach(
            x =>
                {
                    messageBuilder.Append($"\nВаріація: {x.Name}\n");
                    messageBuilder.Append($"-----------------------------\n");
                    messageBuilder.Append($"Ціни:\n");
                    x.VariationPrices.ToList()
                    .ForEach(
                        y =>
                            messageBuilder.Append($"{y.PriceType.Name}: {y.Price} {y.PriceType.CurrencyCode} \n"));
                    messageBuilder.Append($"-----------------------------\n");
                    messageBuilder.Append($"Залишки:\n");

                    messageBuilder.Append($"Загалом: {x.Quantity}\n");

                    x.VariationStocks.Where(y => y.Stock != 0).ToList()
                    .ForEach(
                    y =>
                        messageBuilder.Append($"{y.Storage.Name}: {y.Stock} од. \n"));
                    messageBuilder.Append($"_______________________________\n");
                }
            );
        return messageBuilder.ToString();
    }

    public string CreateMessage(Product product, User user)
    {
        StringBuilder messageBuilder = new StringBuilder();

        messageBuilder.Append("Ім'я: " + product.Name + "\n");
        product.Variations.Where(x => x.Quantity != 0).ToList()
            .ForEach(
            x =>
                {
                    messageBuilder.Append($"\nВаріація: {x.Name}\n");
                    messageBuilder.Append($"-----------------------------\n");
                    messageBuilder.Append($"Ціни:\n");
                    x.VariationPrices.Where(vp => user.UserRole.PriceTypesId.
                        Any(ptid => ptid.Equals(vp.PriceTypeId))).ToList()
                    .ForEach(
                        y =>
                            messageBuilder.Append($"{y.PriceType.Name}: {y.Price} {y.PriceType.CurrencyCode} \n"));
                    messageBuilder.Append($"-----------------------------\n");
                    messageBuilder.Append($"Залишки:\n");
                    if (user.UserRole.Name == "guest" && x.Quantity > 0)
                    {
                        messageBuilder.Append($"Загалом: В наявності\n");
                    }
                    else
                    {
                        messageBuilder.Append($"Загалом: {x.Quantity}\n");
                    }
                    x.VariationStocks.Where(y => y.Stock != 0
                        && user.UserRole.StoragesId != null && user.UserRole.StoragesId.Contains(y.StorageId)).ToList()
                    .ForEach(
                    y =>
                        messageBuilder.Append($"{y.Storage.Name}: {y.Stock} од. \n"));
                    messageBuilder.Append($"________________________________\n");
                }
            );
        return messageBuilder.ToString();
    }
}
