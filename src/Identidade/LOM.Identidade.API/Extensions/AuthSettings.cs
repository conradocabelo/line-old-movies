namespace LOM.Identidade.API.Extensions
{
    public class AuthSettings
    {
        public string Secret { get; set; }
        public int TempoExpirtacao { get; set; }
        public string Emissor { get; set; }
        public string DominioValidade { get; set; }

        public AuthSettings()
        {
            Secret = string.Empty;
            Emissor = string.Empty;
            DominioValidade = string.Empty;
        }
    }
}
