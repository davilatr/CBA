using System.Data.SqlClient;

namespace CBA.Web.Models
{
    public class UsuarioModel
    {
        public static bool ValidarUsuario(string login, string senha)
        {
            var retorno = false;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = "Data Source=localhost;Initial Catalog=Estelar;User id=sa;Password=123456";
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select count(*) from usuario where usuario_login='{0}' and usuario_senha='{1}'", login, senha);
                    retorno = ((int)comando.ExecuteScalar() > 0);
                }
            }
            return retorno;
        }
    }
}