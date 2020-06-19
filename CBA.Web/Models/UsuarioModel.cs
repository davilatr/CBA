using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CBA.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }


        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel retorno = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);

                    comando.CommandText = "select * from usuario where usuario_login=@login and usuario_senha=@senha";

                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                        retorno = new UsuarioModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Login = (string)reader[2]
                        };
                }
            }
            return retorno;
        }

        public string ListaPerfis()
        {
            var retorno = string.Empty;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@idUsuario", SqlDbType.Int).Value = this.Id;

                    comando.CommandText = string.Format(
                        "select perfil_nome " +
                        "from perfil_usuario pu, perfil p " +
                        "where (pu.usuario_id = @idUsuario) and (pu.perfil_id = p.perfil_id) and (p.perfil_ativo = 1)");


                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno += (retorno != string.Empty ? ";" : string.Empty) + (string)reader[0];
                    }
                }
            }
            return retorno;
        }

        public static int RecuperarUsuarioQtde()
        {
            var retorno = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {

                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from usuario";
                    retorno = (int)comando.ExecuteScalar();

                }
            }
            return retorno;
        }

        public static List<UsuarioModel> RecuperarUsuario(int pag = -2, int tamPag = -2)
        {
            var retorno = new List<UsuarioModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = ((pag - 1) * tamPag) + 1;
                    comando.Connection = conexao;

                    if (pag == -2 && tamPag == -2)
                    {
                        comando.CommandText = "select * from usuario order by usuario_nome";
                    }
                    else
                    {
                        comando.CommandText = string.Format(
                            "select * from usuario order by usuario_nome offset {0} rows fetch next {1} rows only",
                            pos > 0 ? pos - 1 : 0, tamPag);
                    }
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno.Add(new UsuarioModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Login = (string)reader[2]
                        });
                    }
                }
            }
            return retorno;
        }

        public static UsuarioModel RecuperarUsuario(int id)
        {
            UsuarioModel retorno = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    comando.CommandText = "select * from usuario where (usuario_id = @id)";
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        retorno = new UsuarioModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Login = (string)reader[2]
                        };
                    }
                }
            }
            return retorno;
        }

        public static bool ExcluirUsuario(int id)
        {
            var retorno = false;

            if (RecuperarUsuario(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        comando.CommandText = "delete from usuario where (usuario_id = @id)";
                        retorno = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return retorno;
        }

        public int SalvarUsuario()
        {
            var retorno = 0;
            var model = RecuperarUsuario(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);

                        comando.CommandText =
                            "insert into usuario (usuario_nome, usuario_login, usuario_senha, perfil_id) values (@nome, @login, @senha);" +
                            "select convert(int, scope_identity())";

                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;

                        if (!string.IsNullOrEmpty(this.Senha))
                            comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);

                        comando.CommandText =
                            "update usuario set usuario_nome=@nome, usuario_login=@login" +
                            (!string.IsNullOrEmpty(this.Senha) ? ", usuario_senha=@senha" : "") +
                            " where usuario_id=@id";

                        if (comando.ExecuteNonQuery() > 0)
                            retorno = this.Id;

                    }
                }
            }
            return retorno;
        }
    }
}