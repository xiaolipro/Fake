using Fake.DomainDrivenDesign;

namespace Domain.Aggregates.BuyerAggregate;

/// <remarks> 
/// Card type class should be marked as abstract with protected constructor to encapsulate known enum types
/// this is currently not possible as OrderingContextSeed uses this constructor to load cardTypes from csv file
/// </remarks>
public class CardType(string name, int value) : Enumeration(name, value)
{
    public static CardType Amex = new(nameof(Amex), 1);
    public static CardType Visa = new(nameof(Visa), 2);
    public static CardType MasterCard = new(nameof(MasterCard), 3);
}