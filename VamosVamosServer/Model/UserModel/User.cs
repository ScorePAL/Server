using System;
using VamosVamosServer.Model.ClubModel;

namespace VamosVamosServer.Model.User;

public class User
{
    private long id;
    private String firstName;
    private String lastName;
    private Role role;
    private DateTime createdAt;
    private IClub relatedTo;

    public long Id
    {
        get => id;
        set => id = value;
    }

    public String FirstName
    {
        get => firstName;
        set => firstName = value;
    }

    public String LastName
    {
        get => lastName;
        set => lastName = value;
    }

    public Role Role
    {
        get => role;
        set => role = value;
    }

    public DateTime CreatedAt
    {
        get => createdAt;
        set => createdAt = value;
    }

    public IClub RelatedTo
    {
        get => relatedTo;
        set => relatedTo = value;
    }
}