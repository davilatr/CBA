using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CBA.Web.Models
{
    public class PerfilModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }



        public static int RecuperarPerfilQtde()
        {
            var retorno = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {

                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from perfil";
                    retorno = (int)comando.ExecuteScalar();

                }
            }
            return retorno;
        }

        public static List<PerfilModel> RecuperarPerfil(int pag, int tamPag)
        {
            var retorno = new List<PerfilModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = ((pag - 1) * tamPag) + 1;
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from perfil order by perfil_nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPag);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno.Add(new PerfilModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Ativo = (bool)reader[2]
                        });
                    }
                }
            }
            return retorno;
        }

        public static PerfilModel RecuperarPerfil(int id)
        {
            PerfilModel retorno = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    comando.CommandText = "select * from perfil where (perfil_id = @id)";
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        retorno = new PerfilModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Ativo = (bool)reader[2]
                        };
                    }
                }
            }
            return retorno;
        }

        public static List<PerfilModel> RecuperarPerfilAtivo()
        {
            var retorno = new List<PerfilModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.CommandText = "select * from perfil where perfil_ativo=1 order by perfil_nome";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno.Add(new PerfilModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Ativo = (bool)reader[2]
                        });
                    }
                }
            }
            return retorno;
        }

        public static bool ExcluirPerfil(int id)
        {
            var retorno = false;

            if (RecuperarPerfil(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        comando.CommandText = "delete from perfil where (perfil_id = @id)";
                        retorno = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return retorno;
        }

        public int SalvarPerfil()
        {
            var retorno = 0;
            var model = RecuperarPerfil(this.Id);

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
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = this.Ativo ? 1 : 0;

                        comando.CommandText =
                            "insert into perfil (perfil_nome, perfil_ativo) values (@nome, @ativo); select convert(int, scope_identity())";
                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        comando.CommandText =
                            "update perfil set perfil_nome=@nome, perfil_ativo=@ativo where perfil_id=@id";

                        if (comando.ExecuteNonQuery() > 0)
                            retorno = this.Id;

                    }
                }
            }
            return retorno;
        }

    }
}