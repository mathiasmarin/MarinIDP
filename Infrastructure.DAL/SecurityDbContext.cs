using System;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DAL
{
    public class SecurityDbContext : IdentityDbContext<MarinAppUser>
    {
        private readonly IConfiguration _configuration;

        public SecurityDbContext(IConfiguration configuration) : base(new DbContextOptions<SecurityDbContext>())
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:IdentityConnectionString"]);
        }
    }
}