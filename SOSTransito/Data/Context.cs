using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> opcoes) : base(opcoes)
        {

        }

        //public DbSet<Usuario> Usuario { get; set; }
        //public DbSet<Cliente> Cliente { get; set; }
        //public DbSet<CNH> CNH { get; set; }
        //public DbSet<Localidade> Localidade { get; set; }
        //public DbSet<PAT> PAT { get; set; }
        //public DbSet<Veiculo> Veiculo { get; set; }
    }
}
