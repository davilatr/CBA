using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CBA.Web.Models
{
    public class UnidadeMedidaModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A sigla é obrigatória.")]
        public string Sigla { get; set; }

        public bool Ativo { get; set; }


        public static int RecuperarUnidadeMedidaQtde()
        {
            var retorno = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {

                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from unidade_medida";
                    retorno = (int)comando.ExecuteScalar();

                }
            }
            return retorno;
        }

        public static List<UnidadeMedidaModel> RecuperarUnidadeMedida(int pag, int tamPag)
        {
            var retorno = new List<UnidadeMedidaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = ((pag - 1) * tamPag) + 1;
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from unidade_medida order by unidade_medida_nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPag);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno.Add(new UnidadeMedidaModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Sigla = (string)reader[2],
                            Ativo = (bool)reader[3]
                        });
                    }
                }
            }
            return retorno;
        }

        public static UnidadeMedidaModel RecuperarUnidadeMedida(int id)
        {
            UnidadeMedidaModel retorno = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    comando.CommandText = "select * from unidade_medida where (unidade_medida_id = @id)";
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        retorno = new UnidadeMedidaModel
                        {
                            Id = (int)reader[0],
                            Nome = (string)reader[1],
                            Sigla = (string)reader[2],
                            Ativo = (bool)reader[3]
                        };
                    }
                }
            }
            return retorno;
        }

        public static bool ExcluirUnidadeMedida(int id)
        {
            var retorno = false;

            if (RecuperarUnidadeMedida(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        comando.CommandText = "delete from unidade_medida where (unidade_medida_id = @id)";
                        retorno = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return retorno;
        }

        public int SalvarUnidadeMedida()
        {
            var retorno = 0;
            var model = RecuperarUnidadeMedida(this.Id);

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
                        comando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = this.Ativo ? 1 : 0;

                        comando.CommandText =
                            "insert into unidade_medida (unidade_medida_nome, unidade_medida_sigla, unidade_medida_ativo) values (@nome, @sigla, @ativo); select convert(int, scope_identity())";
                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        comando.CommandText =
                            "update unidade_medida set unidade_medida_nome=@nome, unidade_medida_sigla=@sigla, unidade_medida_ativo=@ativo where unidade_medida_id=@id";

                        if (comando.ExecuteNonQuery() > 0)
                            retorno = this.Id;

                    }
                }
            }
            return retorno;
        }
    }
}