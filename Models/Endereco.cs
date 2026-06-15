namespace Interdisciplinar.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public int CEP { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public Pessoa Pessoa { get; set; }
        public int PessoaId { get; set; }
    }
}