using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServerBuilding.Models;

namespace ServerBuilding.Models;

public partial class TamiDBContext: DbContext
{
    // This function find the matching user deatails using the email 
    public AppUser? GetUser(string email)
    {
        return this.AppUsers.Where(u => u.UserEmail == email)
                            .FirstOrDefault();
    }
}
