using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
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



        //-------------------------------------------------------------------
        //Métodos

        public static List<DestinacaoBemModel> RecuperarDestinacaoBem()
        {
            var retorno = new List<DestinacaoBemModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tipo_destinacao order by tipo_destinacao_nome";
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
                    comando.CommandText = string.Format(
                        "select * from tipo_destinacao where (tipo_destinacao_id = {0})", id);
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
                        comando.CommandText = string.Format(
                            "delete from tipo_destinacao where (tipo_destinacao_id = {0})", id);
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
                        comando.CommandText = string.Format(
                            "insert into tipo_destinacao (tipo_destinacao_nome, tipo_destinacao_ativo) values ('{0}', {1}); " +
                            "select convert(int, scope_identity())", this.Nome, this.Ativo ? 1 : 0);
                        retorno = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = string.Format(
                            "update tipo_destinacao set tipo_destinacao_nome='{1}', tipo_destinacao_ativo={2} where tipo_destinacao_id={0}", 
                            this.Id, this.Nome, this.Ativo ? 1 : 0);
                        
                        if (comando.ExecuteNonQuery() > 0)
                            retorno = this.Id;
                        
                    }
                }
            }
            return retorno;
        }
    }
}