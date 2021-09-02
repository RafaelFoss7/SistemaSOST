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

        [DisplayName("Nº AIT")]
        [Required(ErrorMessage = "O campo Nº AIT é obrigatório.")]
        public string NAIT { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data da Infração")]
        [Required(ErrorMessage = "O campo data da infração é obrigatório.")]
        public DateTime DataInfracao { get; set; }

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

        public string Arquivo { get; set; }

        //Relacionamentos...
        public virtual CNH CNH { get; set; }
        public int CNHId { get; set; }
    }
}
