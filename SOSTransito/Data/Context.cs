using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOSTransito.Models;

namespace SOSTransito.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> opcoes) : base(opcoes)
        {

        }
        public DbSet<SOSTransito.Models.Usuario> Usuario { get; set; }
        public DbSet<SOSTransito.Models.Localidade> Localidade { get; set; }
        public DbSet<SOSTransito.Models.Atribuicao_Localidade> Atribuicao_Localidade { get; set; }
        public DbSet<SOSTransito.Models.Cliente> Cliente { get; set; }
        public DbSet<SOSTransito.Models.CNH> CNH { get; set; }
        public DbSet<SOSTransito.Models.Multa> Multa { get; set; }
        public DbSet<SOSTransito.Models.Veiculo> Veiculo { get; set; }

    }
}
