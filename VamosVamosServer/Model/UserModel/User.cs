using System;
using VamosVamosServer.Model.ClubModel;

namespace VamosVamosServer.Model.User;

public class User
{
    private int _id;
    private String _firstName;
    private String _lastName;
    private Role _role;
    private DateTime _createdAt;
    private IClub relatedTo;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public String FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public String LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public Role Role
    {
        get => _role;
        set => _role = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }
}