using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Atribuicao_Localidade")]
    public class Atribuicao_Localidade
    {
        [Key]
        public int ATRLOCId { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual Localidade Localidades { get; set; }
        public int LocalidadeId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}
