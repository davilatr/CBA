using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CBA.Web.Models
{
    public class DestinacaoBemModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }



        public static int RecuperarDestinacaoBemQtde()
        {
            var retorno = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from tipo_destinacao";
                    retorno = (int)comando.ExecuteScalar();
                    
                }
            }
            return retorno;
        }

        public static List<DestinacaoBemModel> RecuperarDestinacaoBem(int pag, int tamPag)
        {
            var retorno = new List<DestinacaoBemModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = ((pag - 1) * tamPag)+1;
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from tipo_destinacao order by tipo_destinacao_nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPag);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        retorno.Add(new DestinacaoBemModel
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

        public static DestinacaoBemModel RecuperarDestinacaoBem(int id)
        {
            DestinacaoBemModel retorno = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    comando.CommandText = "select * from tipo_destinacao where (tipo_destinacao_id = @id)";
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        retorno = new DestinacaoBemModel
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

        public static bool ExcluirDestinacaoBem(int id)
        {
            var retorno = false;

            if (RecuperarDestinacaoBem(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        comando.CommandText = "delete from tipo_destinacao where (tipo_destinacao_id = @id)";
                        retorno = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return retorno;
        }

        public int SalvarDestinacaoBem()
        {
            var retorno = 0;
            var model = RecuperarDestinacaoBem(this.Id);

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
                            "insert into tipo_destinacao (tipo_destinacao_nome, tipo_destinacao_ativo) values (@nome, @ativo); select convert(int, scope_identity())";
                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        comando.CommandText =
                            "update tipo_destinacao set tipo_destinacao_nome=@nome, tipo_destinacao_ativo=@ativo where tipo_destinacao_id=@id";

                        if (comando.ExecuteNonQuery() > 0)
                            retorno = this.Id;

                    }
                }
            }
            return retorno;
        }
    }
}