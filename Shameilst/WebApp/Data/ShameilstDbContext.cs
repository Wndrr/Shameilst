using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data
{
    public class ShameilstDbContext : IdentityDbContext
    {
        public ShameilstDbContext(DbContextOptions<ShameilstDbContext> options)
            : base(options)
        {
        }
    }
}