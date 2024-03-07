namespace Domain.Aggregates.BuyerAggregate;

/// <remarks> 
/// Card type class should be marked as abstract with protected constructor to encapsulate known enum types
/// this is currently not possible as OrderingContextSeed uses this constructor to load cardTypes from csv file
/// </remarks>
public enum CardType
{
    Amex = 1,
    Visa,
    MasterCard,
}