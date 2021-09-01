using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Multa")]
    public class Multa
    {   //atributos...
        [Key]
        public int MultaId { get; set; }

        [DisplayName("Org. Atuador")]
        [Required(ErrorMessage = "O campo org. atuador é obrigatório.")]
        public string OrgAtuador { get; set; }

        [DisplayName("Veículo")]
        [Required(ErrorMessage = "O campo veículo é obrigatório.")]
        public string Veiculo { get; set; }

        [DisplayName("Pontuação")]
        [Required(ErrorMessage = "O campo pontuação é obrigatório.")]
        public int Pontuacao { get; set; }

        [DisplayName("Processo PAT")]
        [Required(ErrorMessage = "O campo processo PAT é obrigatório.")]
        public string Processo { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual CNH CNH { get; set; }
        public int CNHId { get; set; }
    }
}
