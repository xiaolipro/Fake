﻿using System;
using Fake;
using Fake.Domain.Entities;

namespace Domain.Aggregates.BuyerAggregate;
public class PaymentMethod : Entity<Guid>
{
    private string _alias;
    private string _cardNumber;
    private string _securityNumber;
    private string _cardHolderName;
    private DateTime _expiration;

    private CardType _cardType;
    public CardType CardType { get; private set; }


    protected PaymentMethod() { }

    public PaymentMethod(CardType cardType, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
    {

        _cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new BusinessException(nameof(cardNumber));
        _securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new BusinessException(nameof(securityNumber));
        _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new BusinessException(nameof(cardHolderName));

        if (expiration < DateTime.UtcNow)
        {
            throw new BusinessException(nameof(expiration));
        }

        _alias = alias;
        _expiration = expiration;
        _cardType = cardType;
    }

    public bool IsEqualTo(CardType cardType, string cardNumber, DateTime expiration)
    {
        return _cardType == cardType
            && _cardNumber == cardNumber
            && _expiration == expiration;
    }
}
