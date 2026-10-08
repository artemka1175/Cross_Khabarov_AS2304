using Khabarov_Artem_AS2304.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Khabarov_Artem_AS2304.Data
{
    public class LabContext : DbContext //БД
    {
        public LabContext(DbContextOptions<LabContext> options) : base(options)
        {

        }

        public DbSet<Petroleum> Petroleums { get; set; }
        public DbSet<LabApparatus> LabApparatuses { get; set; }
        public DbSet<LabSession> LabSessions { get; set; }
    }
}