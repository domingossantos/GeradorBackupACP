using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Sql;

namespace GeradorBackup
{
    public partial class FrmMain : Form
    {
        ConexaoDB conn;
        int maximo = 0;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnBuscarArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();

                open.ShowDialog();
                txNomeArquivo.Text = open.FileName;
                
                // Ler header do arquivo para mostrar o que será carregado.
                StreamReader arquivo = new StreamReader(txNomeArquivo.Text);



                String linha = arquivo.ReadLine();
                lblNumCadastroRestore.Text = linha.Substring(14, 5);
                lblNumContribuicoesRestore.Text = linha.Substring(20, 5);
                lblContrbEfetivasRestores.Text = linha.Substring(26, 5);
                lblDoacaoSolicitRest.Text = linha.Substring(32, 5);
                lblDoacaoConsedidaRest.Text = linha.Substring(38, 5);
                lblNumDoacaoRest.Text = linha.Substring(44, 5);

                btnRestaurar.Enabled = true;

                arquivo.Close();
                
            }
            catch (Exception ex) {
                MessageBox.Show("Erro " + ex.Message);
            }

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            conn = new ConexaoDB(Properties.Settings.Default.connBackup);
           
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            try
            {
                String sql = "select count(*) num from tbContribuinte";
                conn.abreBanco();
                lblNumCadastros.Text = conn.retornoInteiro(sql).ToString();

                sql = "select count(*) num from tbContribuicoes where dt_contribuicao >= '"+formatoData(dtCopia.Value.ToShortDateString(),true)+"'";
                lblNumContribuicoes.Text = conn.retornoInteiro(sql).ToString();

                sql = "select count(*) num from tbPagamentoMensal where " +
                       "dt_anoPagamento >= " + dtCopia.Value.Year.ToString() +
                       " and dt_mesPagamento >= " + dtCopia.Value.Month.ToString() +
                       " and cd_TipRubrica in(1,11)";
                lblNumDoacoes.Text = conn.retornoInteiro(sql).ToString();

                sql = "select count(*)  from tbPagtoContribuicao where dt_pagtoContribuicao >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "'";
                lblEfetivadas.Text = conn.retornoInteiro(sql).ToString();

                sql = "select * from tbRequerimentoBeneficio where dt_requerimento >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "'";

                lblSolicitacoes.Text = conn.retornoInteiro(sql).ToString();

                sql = "select * from tbValBeneficio where no_reqBeneficio in( "+
                                "select no_reqBeneficio from tbRequerimentoBeneficio where " +
                                "dt_requerimento >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "')";
                lblConcecoes.Text = conn.retornoInteiro(sql).ToString();


                maximo = Convert.ToInt32(lblNumCadastros.Text) +
                             Convert.ToInt32(lblNumDoacoes.Text) +
                             Convert.ToInt32(lblNumContribuicoes.Text) +
                             Convert.ToInt32(lblEfetivadas.Text) +
                             Convert.ToInt32(lblConcecoes.Text) +
                             Convert.ToInt32(lblSolicitacoes.Text);
                
                btnBackup.Enabled = true;


            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
            finally { conn.fechaBanco(); }
        }

        public string formatoData(String data, bool shot = false)
        {
            //0123456789012345678
            //20/09/2011 20:51:24
            if (shot)
                return data.Substring(6, 4) + "-" + data.Substring(3, 2) + "-" + data.Substring(0, 2);
            else
                return data.Substring(6, 4) + "-" + data.Substring(3, 2) + "-" + data.Substring(0, 2) + " " + data.Substring(11, 8);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try{
                progressBar1.Value = 0;
                Refresh();
                String arquivo = @"backup-"+DateTime.Now.Day.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Year.ToString()+".txt";

                if (!File.Exists(arquivo)) {
                    File.Delete(arquivo);
                }
                
                int count = 1;
            
                StreamWriter escrever = new StreamWriter(arquivo);

                progressBar1.Maximum = maximo;
                
                conn.abreBanco();
                String header = Preencher(DateTime.Now.Day.ToString(), "0", 0, 2) +
                                Preencher(DateTime.Now.Month.ToString(), "0", 0, 2) + 
                                DateTime.Now.Year.ToString() +
                                Preencher(maximo.ToString(),"0",0,5) +
                                " "+ Preencher(lblNumCadastros.Text,"0",0,5)+
                                " "+ Preencher(lblNumContribuicoes.Text,"0",0,5)+
                                " "+ Preencher(lblEfetivadas.Text,"0",0,5)+
                                " "+ Preencher(lblNumDoacoes.Text,"0",0,5)+
                                " "+ Preencher(lblSolicitacoes.Text,"0",0,5)+
                                " "+ Preencher(lblConcecoes.Text,"0",0,5);

                escrever.WriteLine(header);

                String sql = "select * from tbContribuinte";
                DataTable dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '1', ref count);

                sql = "select * from tbContribuicoes where dt_contribuicao >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "'";
                dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '2', ref count);

                sql = "select * from tbPagtoContribuicao where dt_pagtoContribuicao >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "'";
                dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '3', ref count);

                sql = "select * from tbRequerimentoBeneficio where dt_requerimento >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "'";
                dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '4', ref count);

                sql = "select * from tbValBeneficio where no_reqBeneficio in( "+
                                "select no_reqBeneficio from tbRequerimentoBeneficio where " +
                                "dt_requerimento >= '" + formatoData(dtCopia.Value.ToShortDateString(), true) + "')";
                dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '5', ref count);

                sql = "select * from tbPagamentoMensal where " +
                       "dt_anoPagamento >= " + dtCopia.Value.Year.ToString() +
                       " and dt_mesPagamento >= " + dtCopia.Value.Month.ToString() +
                       " and cd_TipRubrica in(1,11)";
                dados = conn.retornarDataSet(sql);
                gerarLinhasTabela(ref escrever, dados, '6', ref count);
                
                MessageBox.Show("Geração Concluida!");
                btnBackup.Enabled = false;
                //escrever.WriteLine("#"+ maximo.ToString());
                escrever.Close();
                escrever.Dispose();
                
            } 
            catch(Exception ex){ MessageBox.Show("Erro ao gerar backup\n"+ex.Message); }
            finally{ conn.fechaBanco(); }

        }



        #region Atualiza Dados Contribuinte

        private bool verificaEstadoContribuinte(String codigo) {
            bool retorno = false;

            try
            {
                String sql = "select count(*) from tbContribuinte where cd_cadastro = "+codigo;

                if (conn.retornoInteiro(sql) > 0) {
                    retorno = true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao consultar contribuinte " + ex.Message);
            }
            
            return retorno;
        }

        private void carregaContribuinte(String[] dados) {
            try
            {
                String sql = "";
                sql = sql + " INSERT INTO tbContribuinte \n ";
                sql = sql + "            (cd_cadastro \n ";
                sql = sql + "            ,cd_formTeologica \n ";
                sql = sql + "            ,cd_natPastor \n ";
                sql = sql + "            ,cd_escPastor \n ";
                sql = sql + "            ,cd_nacPastor \n ";
                sql = sql + "            ,cd_estCivil \n ";
                sql = sql + "            ,cd_categoria \n ";
                sql = sql + "            ,nm_pastor \n ";
                sql = sql + "            ,no_regConv \n ";
                sql = sql + "            ,dt_nascPastor \n ";
                sql = sql + "            ,tp_sanguineo \n ";
                sql = sql + "            ,no_regGeral \n ";
                sql = sql + "            ,no_cpf \n ";
                sql = sql + "            ,ds_endereco \n ";
                sql = sql + "            ,ds_compEndPastor \n ";
                sql = sql + "            ,no_cepPastor \n ";
                sql = sql + "            ,dt_filiacao \n ";
                sql = sql + "            ,nm_pai \n ";
                sql = sql + "            ,nm_mae \n ";
                sql = sql + "            ,nm_conjuge \n ";
                sql = sql + "            ,dt_nascConjuge \n ";
                sql = sql + "            ,no_fone \n ";
                sql = sql + "            ,dt_batismo \n ";
                sql = sql + "            ,ds_localBatismo \n ";
                sql = sql + "            ,dt_autEvangelista \n ";
                sql = sql + "            ,dt_consagEvangelista \n ";
                sql = sql + "            ,dt_ordenacPastor \n ";
                sql = sql + "            ,ds_localConsagracao \n ";
                sql = sql + "            ,ds_campo \n ";
                sql = sql + "            ,ds_supervisao \n ";
                sql = sql + "            ,no_certCasamento \n ";
                sql = sql + "            ,ds_orgaoemissorrg \n ";
                sql = sql + "            ,dt_emissao \n ";
                sql = sql + "            ,ds_bairro \n ";
                sql = sql + "            ,ds_uf \n ";
                sql = sql + "            ,ds_cidade \n ";
                sql = sql + "            ,cd_sitJubilamento \n ";
                sql = sql + "            ,dt_falecimento \n ";
                sql = sql + "            ,cd_banco \n ";
                sql = sql + "            ,st_arquivoMorto) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@cd_cadastro \n ";
                sql = sql + "            ,@cd_formTeologica \n ";
                sql = sql + "            ,@cd_natPastor \n ";
                sql = sql + "            ,@cd_escPastor \n ";
                sql = sql + "            ,@cd_nacPastor \n ";
                sql = sql + "            ,@cd_estCivil \n ";
                sql = sql + "            ,@cd_categoria \n ";
                sql = sql + "            ,@nm_pastor \n ";
                sql = sql + "            ,@no_regConv \n ";
                sql = sql + "            ,@dt_nascPastor \n ";
                sql = sql + "            ,@tp_sanguineo \n ";
                sql = sql + "            ,@no_regGeral \n ";
                sql = sql + "            ,@no_cpf \n ";
                sql = sql + "            ,@ds_endereco \n ";
                sql = sql + "            ,@ds_compEndPastor \n ";
                sql = sql + "            ,@no_cepPastor \n ";
                sql = sql + "            ,@dt_filiacao \n ";
                sql = sql + "            ,@nm_pai \n ";
                sql = sql + "            ,@nm_mae \n ";
                sql = sql + "            ,@nm_conjuge \n ";
                sql = sql + "            ,@dt_nascConjuge \n ";
                sql = sql + "            ,@no_fone \n ";
                sql = sql + "            ,@dt_batismo \n ";
                sql = sql + "            ,@ds_localBatismo \n ";
                sql = sql + "            ,@dt_autEvangelista \n ";
                sql = sql + "            ,@dt_consagEvangelista \n ";
                sql = sql + "            ,@dt_ordenacPastor \n ";
                sql = sql + "            ,@ds_localConsagracao \n ";
                sql = sql + "            ,@ds_campo \n ";
                sql = sql + "            ,@ds_supervisao \n ";
                sql = sql + "            ,@no_certCasamento \n ";
                sql = sql + "            ,@ds_orgaoemissorrg \n ";
                sql = sql + "            ,@dt_emissao \n ";
                sql = sql + "            ,@ds_bairro \n ";
                sql = sql + "            ,@ds_uf \n ";
                sql = sql + "            ,@ds_cidade \n ";
                sql = sql + "            ,@cd_sitJubilamento \n ";
                sql = sql + "            ,@dt_falecimento \n ";
                sql = sql + "            ,@cd_banco \n ";
                sql = sql + "            ,@st_arquivoMorto) \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("cd_formTeologica", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("cd_natPastor", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("cd_escPastor", Convert.ToInt32(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_nacPastor", Convert.ToInt32(dados[5]));
                conn.cmd.Parameters.AddWithValue("cd_estCivil", Convert.ToInt32(dados[6]));
                conn.cmd.Parameters.AddWithValue("cd_categoria", Convert.ToInt32(dados[7]));
                conn.cmd.Parameters.AddWithValue("nm_pastor", dados[8]);
                if (dados[9].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("no_regConv", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("no_regConv", Convert.ToInt32(dados[9]));
                }

                if (dados[10].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascPastor", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascPastor", Convert.ToDateTime(formatoData(dados[10], true)));
                }
                conn.cmd.Parameters.AddWithValue("tp_sanguineo", dados[11]);
                conn.cmd.Parameters.AddWithValue("no_regGeral", dados[12]);
                conn.cmd.Parameters.AddWithValue("no_cpf", dados[13]);
                conn.cmd.Parameters.AddWithValue("ds_endereco", dados[14]);
                conn.cmd.Parameters.AddWithValue("ds_compEndPastor", dados[15]);
                conn.cmd.Parameters.AddWithValue("no_cepPastor", dados[16]);

                if (dados[17].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_filiacao", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_filiacao", Convert.ToDateTime(formatoData(dados[17], true)));
                }

                conn.cmd.Parameters.AddWithValue("nm_pai", dados[18]);
                conn.cmd.Parameters.AddWithValue("nm_mae", dados[19]);
                conn.cmd.Parameters.AddWithValue("nm_conjuge", dados[20]);

                if (dados[21].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascConjuge", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascConjuge", Convert.ToDateTime(formatoData(dados[21], true)));
                }

                conn.cmd.Parameters.AddWithValue("no_fone", dados[22]);

                if (dados[23].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_batismo", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_batismo", Convert.ToDateTime(formatoData(dados[23], true)));
                }

                conn.cmd.Parameters.AddWithValue("ds_localBatismo", dados[24]);

                if (dados[25].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_autEvangelista", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_autEvangelista", Convert.ToDateTime(formatoData(dados[25], true)));
                }

                if (dados[26].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_consagEvangelista", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_consagEvangelista", Convert.ToDateTime(formatoData(dados[26], true)));
                }

                if (dados[27].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_ordenacPastor", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_ordenacPastor", Convert.ToDateTime(formatoData(dados[27], true)));
                }


                conn.cmd.Parameters.AddWithValue("ds_localConsagracao", dados[28]);
                conn.cmd.Parameters.AddWithValue("ds_campo", dados[29]);
                conn.cmd.Parameters.AddWithValue("ds_supervisao", dados[30]);
                conn.cmd.Parameters.AddWithValue("no_certCasamento", dados[31]);
                conn.cmd.Parameters.AddWithValue("ds_orgaoemissorrg", dados[32]);

                if (dados[33].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_emissao", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_emissao", Convert.ToDateTime(formatoData(dados[33], true)));
                }


                conn.cmd.Parameters.AddWithValue("ds_bairro", dados[34]);
                conn.cmd.Parameters.AddWithValue("ds_uf", dados[35]);
                conn.cmd.Parameters.AddWithValue("ds_cidade", dados[36]);
                conn.cmd.Parameters.AddWithValue("cd_sitJubilamento", dados[37]);
                if (dados[38].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_falecimento", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_falecimento", Convert.ToDateTime(formatoData(dados[38], true)));
                }


                conn.cmd.Parameters.AddWithValue("cd_banco", dados[39]);
                conn.cmd.Parameters.AddWithValue("st_arquivoMorto", dados[40]);

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir contribuinte "+ex.Message);
            }

        }

        private void alteraContribuinte(String[] dados) {
            try {
                String sql = "";
                sql = sql + " UPDATE tbContribuinte \n ";
                sql = sql + "    SET cd_cadastro = @cd_cadastro \n ";
                sql = sql + "       ,cd_formTeologica = @cd_formTeologica \n ";
                sql = sql + "       ,cd_natPastor = @cd_natPastor \n ";
                sql = sql + "       ,cd_escPastor = @cd_escPastor \n ";
                sql = sql + "       ,cd_nacPastor = @cd_nacPastor \n ";
                sql = sql + "       ,cd_estCivil = @cd_estCivil \n ";
                sql = sql + "       ,cd_categoria = @cd_categoria \n ";
                sql = sql + "       ,nm_pastor = @nm_pastor \n ";
                sql = sql + "       ,no_regConv = @no_regConv \n ";
                sql = sql + "       ,dt_nascPastor = @dt_nascPastor \n ";
                sql = sql + "       ,tp_sanguineo = @tp_sanguineo \n ";
                sql = sql + "       ,no_regGeral = @no_regGeral \n ";
                sql = sql + "       ,no_cpf = @no_cpf \n ";
                sql = sql + "       ,ds_endereco = @ds_endereco \n ";
                sql = sql + "       ,ds_compEndPastor = @ds_compEndPastor \n ";
                sql = sql + "       ,no_cepPastor = @no_cepPastor \n ";
                sql = sql + "       ,dt_filiacao = @dt_filiacao \n ";
                sql = sql + "       ,nm_pai = @nm_pai \n ";
                sql = sql + "       ,nm_mae = @nm_mae \n ";
                sql = sql + "       ,nm_conjuge = @nm_conjuge \n ";
                sql = sql + "       ,dt_nascConjuge = @dt_nascConjuge \n ";
                sql = sql + "       ,no_fone = @no_fone \n ";
                sql = sql + "       ,dt_batismo = @dt_batismo \n ";
                sql = sql + "       ,ds_localBatismo = @ds_localBatismo \n ";
                sql = sql + "       ,dt_autEvangelista = @dt_autEvangelista \n ";
                sql = sql + "       ,dt_consagEvangelista = @dt_consagEvangelista \n ";
                sql = sql + "       ,dt_ordenacPastor = @dt_ordenacPastor \n ";
                sql = sql + "       ,ds_localConsagracao = @ds_localConsagracao \n ";
                sql = sql + "       ,ds_campo = @ds_campo \n ";
                sql = sql + "       ,ds_supervisao = @ds_supervisao \n ";
                sql = sql + "       ,no_certCasamento = @no_certCasamento \n ";
                sql = sql + "       ,ds_orgaoemissorrg = @ds_orgaoemissorrg \n ";
                sql = sql + "       ,dt_emissao = @dt_emissao \n ";
                sql = sql + "       ,ds_bairro = @ds_bairro \n ";
                sql = sql + "       ,ds_uf = @ds_uf \n ";
                sql = sql + "       ,ds_cidade = @ds_cidade \n ";
                sql = sql + "       ,cd_sitJubilamento = @cd_sitJubilamento \n ";
                sql = sql + "       ,dt_falecimento = @dt_falecimento \n ";
                sql = sql + "       ,cd_banco = @cd_banco \n ";
                sql = sql + "       ,st_arquivoMorto = @st_arquivoMorto \n ";
                sql = sql + "  WHERE cd_cadastro = @cd_cadastro \n ";

                
                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("cd_formTeologica", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("cd_natPastor", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("cd_escPastor", Convert.ToInt32(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_nacPastor", Convert.ToInt32(dados[5]));
                conn.cmd.Parameters.AddWithValue("cd_estCivil", Convert.ToInt32(dados[6]));
                conn.cmd.Parameters.AddWithValue("cd_categoria", Convert.ToInt32(dados[7]));
                conn.cmd.Parameters.AddWithValue("nm_pastor", dados[8]);
                if (dados[9].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("no_regConv", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("no_regConv", Convert.ToInt32(dados[9]));
                }

                if (dados[10].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascPastor", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascPastor", Convert.ToDateTime(formatoData(dados[10],true)));
                }
                conn.cmd.Parameters.AddWithValue("tp_sanguineo", dados[11]);
                conn.cmd.Parameters.AddWithValue("no_regGeral", dados[12]);
                conn.cmd.Parameters.AddWithValue("no_cpf", dados[13]);
                conn.cmd.Parameters.AddWithValue("ds_endereco", dados[14]);
                conn.cmd.Parameters.AddWithValue("ds_compEndPastor", dados[15]);
                conn.cmd.Parameters.AddWithValue("no_cepPastor", dados[16]);

                if (dados[17].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_filiacao", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_filiacao", Convert.ToDateTime(formatoData(dados[17],true)));
                }

                conn.cmd.Parameters.AddWithValue("nm_pai", dados[18]);
                conn.cmd.Parameters.AddWithValue("nm_mae", dados[19]);
                conn.cmd.Parameters.AddWithValue("nm_conjuge", dados[20]);

                if (dados[21].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascConjuge", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_nascConjuge", Convert.ToDateTime(formatoData(dados[21],true)));
                }

                conn.cmd.Parameters.AddWithValue("no_fone", dados[22]);

                if (dados[23].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_batismo", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_batismo", Convert.ToDateTime(formatoData(dados[23],true)));
                }

                conn.cmd.Parameters.AddWithValue("ds_localBatismo", dados[24]);

                if (dados[25].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_autEvangelista", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_autEvangelista", Convert.ToDateTime(formatoData(dados[25],true)));
                }

                if (dados[26].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_consagEvangelista", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_consagEvangelista", Convert.ToDateTime(formatoData(dados[26],true)));
                }

                if (dados[27].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_ordenacPastor", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_ordenacPastor", Convert.ToDateTime(formatoData(dados[27],true)));
                }


                conn.cmd.Parameters.AddWithValue("ds_localConsagracao", dados[28]);
                conn.cmd.Parameters.AddWithValue("ds_campo", dados[29]);
                conn.cmd.Parameters.AddWithValue("ds_supervisao", dados[30]);
                conn.cmd.Parameters.AddWithValue("no_certCasamento", dados[31]);
                conn.cmd.Parameters.AddWithValue("ds_orgaoemissorrg", dados[32]);

                if (dados[33].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_emissao", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_emissao", Convert.ToDateTime(formatoData(dados[33],true)));
                }


                conn.cmd.Parameters.AddWithValue("ds_bairro", dados[34]);
                conn.cmd.Parameters.AddWithValue("ds_uf", dados[35]);
                conn.cmd.Parameters.AddWithValue("ds_cidade", dados[36]);
                conn.cmd.Parameters.AddWithValue("cd_sitJubilamento", dados[37]);
                if (dados[38].ToString().Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_falecimento", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_falecimento", Convert.ToDateTime(formatoData(dados[38],true)));
                }


                conn.cmd.Parameters.AddWithValue("cd_banco", dados[39]);
                conn.cmd.Parameters.AddWithValue("st_arquivoMorto", dados[40]);

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar contribuinte " + ex.Message);
            }
        }

        #endregion

        #region Atualiza Dados Contribuicao
        private bool verificaEstadoContribuicao(String codigo,String ano, String mes, String tipo)
        {
            bool retorno = false;

            try
            {
                String sql = "select count(*) from tbContribuicoes "+
                             "where cd_cadastro = " + codigo +
                             " and dt_anoRefContribuicao = "+ano+
                             " and dt_mesRefContribuicao = "+mes +
                             " and cd_tipoContribuicao = "+tipo;

                if (conn.retornoInteiro(sql) > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar contribuinção " + ex.Message);
            }

            return retorno;
        }

        private void carregaContribuicao(String[] dados) {
            try
            {
                String sql = "";
                sql = sql + " INSERT INTO tbContribuicoes \n ";
                sql = sql + "            (cd_cadastro \n ";
                sql = sql + "            ,dt_anoRefContribuicao \n ";
                sql = sql + "            ,dt_mesRefContribuicao \n ";
                sql = sql + "            ,vl_ContribuicaoVencimento \n ";
                sql = sql + "            ,cd_tipoContribuicao \n ";
                sql = sql + "            ,cd_sitQuitacao \n ";
                sql = sql + "            ,dt_Contribuicao) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@cd_cadastro \n ";
                sql = sql + "            ,@dt_anoRefContribuicao \n ";
                sql = sql + "            ,@dt_mesRefContribuicao \n ";
                sql = sql + "            ,@vl_ContribuicaoVencimento \n ";
                sql = sql + "            ,@cd_tipoContribuicao \n ";
                sql = sql + "            ,@cd_sitQuitacao \n ";
                sql = sql + "            ,@dt_Contribuicao) \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("dt_anoRefContribuicao", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_mesRefContribuicao", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("vl_ContribuicaoVencimento", Convert.ToDouble(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_tipoContribuicao", Convert.ToInt32(dados[5]));
                if (dados[6].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("cd_sitQuitacao", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("cd_sitQuitacao", Convert.ToInt32(dados[6]));
                }

                conn.cmd.Parameters.AddWithValue("dt_Contribuicao", Convert.ToDateTime(dados[7]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex) {
                throw new Exception("Erro ao carregar contribuição\n" + ex.Message);
            }
        }

        private void alteraContribuicao(String[] dados) {
            try
            {
                String sql = "";
                sql = sql + " UPDATE tbContribuicoes \n ";
                sql = sql + "    SET vl_ContribuicaoVencimento = @vl_ContribuicaoVencimento \n ";
                sql = sql + "       ,cd_sitQuitacao = @cd_sitQuitacao \n ";
                sql = sql + "       ,dt_Contribuicao = @dt_Contribuicao \n ";
                sql = sql + "  WHERE cd_cadastro = @cd_cadastro AND \n ";
                sql = sql + "       dt_anoRefContribuicao = @dt_anoRefContribuicao AND \n ";
                sql = sql + "       dt_mesRefContribuicao = @dt_mesRefContribuicao AND \n ";
                sql = sql + "       cd_tipoContribuicao = @cd_tipoContribuicao \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("dt_anoRefContribuicao", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_mesRefContribuicao", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("vl_ContribuicaoVencimento", Convert.ToDouble(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_tipoContribuicao", Convert.ToInt32(dados[5]));
                if (dados[6].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("cd_sitQuitacao", DBNull.Value);
                }
                else {
                    conn.cmd.Parameters.AddWithValue("cd_sitQuitacao", Convert.ToInt32(dados[6]));
                }
                
                conn.cmd.Parameters.AddWithValue("dt_Contribuicao", Convert.ToDateTime(dados[7]));
                //conn.cmd.Parameters.AddWithValue("id_usuario", Convert.ToInt32(dados[8]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar contribuição\n" + ex.Message);
            }
        }

        #endregion

        #region Atualiza Dados Pagto Contribuicao

        private bool verificaEstadoPagtoContribuicao(String codigo)
        {
            bool retorno = false;

            try
            {
                String sql = "select count(*) from tbPagtoContribuicao " +
                             "where id_PagtoContribuincao = " + codigo;

                if (conn.retornoInteiro(sql) > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar pagamento contribuinção " + ex.Message);
            }

            return retorno;
        }

        private void carregaPagtoContribuicao(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " SET IDENTITY_INSERT tbPagtoContribuicao ON \n ";
                sql = sql + "  \n ";
                sql = sql + " INSERT INTO tbPagtoContribuicao \n ";
                sql = sql + "            (id_PagtoContribuincao \n ";
                sql = sql + "            ,vl_pagtoContribuicao \n ";
                sql = sql + "            ,dt_pagtoContribuicao \n ";
                sql = sql + "            ,ds_bancoCheque \n ";
                sql = sql + "            ,ds_agenciaCheque \n ";
                sql = sql + "            ,no_chequePagto \n ";
                sql = sql + "            ,dt_resgChequePre \n ";
                sql = sql + "            ,cd_formPagto \n ";
                sql = sql + "            ,cd_cadastro \n ";
                sql = sql + "            ,dt_anoRefContribuicao \n ";
                sql = sql + "            ,dt_mesRefContribuicao \n ";
                sql = sql + "            ,sq_chequePagto \n ";
                sql = sql + "            ,cd_tipoContribuicao) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@id_PagtoContribuincao \n ";
                sql = sql + "            ,@vl_pagtoContribuicao \n ";
                sql = sql + "            ,@dt_pagtoContribuicao \n ";
                sql = sql + "            ,@ds_bancoCheque \n ";
                sql = sql + "            ,@ds_agenciaCheque \n ";
                sql = sql + "            ,@no_chequePagto \n ";
                sql = sql + "            ,@dt_resgChequePre \n ";
                sql = sql + "            ,@cd_formPagto \n ";
                sql = sql + "            ,@cd_cadastro \n ";
                sql = sql + "            ,@dt_anoRefContribuicao \n ";
                sql = sql + "            ,@dt_mesRefContribuicao \n ";
                sql = sql + "            ,@sq_chequePagto \n ";
                sql = sql + "            ,@cd_tipoContribuicao) \n ";
                sql = sql + "  \n ";
                sql = sql + " SET IDENTITY_INSERT tbPagtoContribuicao OFF \n ";


                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("id_PagtoContribuincao", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("vl_pagtoContribuicao", Convert.ToDouble(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_pagtoContribuicao", Convert.ToDateTime(dados[3]));
                conn.cmd.Parameters.AddWithValue("ds_bancoCheque", dados[4]);
                conn.cmd.Parameters.AddWithValue("ds_agenciaCheque", dados[5]);
                conn.cmd.Parameters.AddWithValue("no_chequePagto", dados[6]);

                if (dados[7].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_resgChequePre", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_resgChequePre", Convert.ToDateTime(dados[7]));
                }

                conn.cmd.Parameters.AddWithValue("cd_formPagto", Convert.ToInt32(dados[8]));
                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToDouble(dados[9]));
                conn.cmd.Parameters.AddWithValue("dt_anoRefContribuicao", Convert.ToInt32(dados[10]));
                conn.cmd.Parameters.AddWithValue("dt_mesRefContribuicao", Convert.ToInt32(dados[11]));

                if (dados[12].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("sq_chequePagto", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("sq_chequePagto", Convert.ToInt32(dados[12]));
                }
                conn.cmd.Parameters.AddWithValue("cd_tipoContribuicao", Convert.ToInt32(dados[13]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar contribuição\n" + ex.Message);
            }
        }

        private void alteraPagtoContribuicao(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " UPDATE tbPagtoContribuicao \n ";
                sql = sql + "    SET vl_pagtoContribuicao = @vl_pagtoContribuicao \n ";
                sql = sql + "       ,dt_pagtoContribuicao = @dt_pagtoContribuicao \n ";
                sql = sql + "       ,ds_bancoCheque = @ds_bancoCheque \n ";
                sql = sql + "       ,ds_agenciaCheque = @ds_agenciaCheque \n ";
                sql = sql + "       ,no_chequePagto = @no_chequePagto \n ";
                sql = sql + "       ,dt_resgChequePre = @dt_resgChequePre \n ";
                sql = sql + "       ,cd_formPagto = @cd_formPagto \n ";
                sql = sql + "       ,cd_cadastro = @cd_cadastro \n ";
                sql = sql + "       ,dt_anoRefContribuicao = @dt_anoRefContribuicao \n ";
                sql = sql + "       ,dt_mesRefContribuicao = @dt_mesRefContribuicao \n ";
                sql = sql + "       ,sq_chequePagto = @sq_chequePagto \n ";
                sql = sql + "       ,cd_tipoContribuicao = @cd_tipoContribuicao \n ";
                sql = sql + "  WHERE id_PagtoContribuincao = @id_PagtoContribuincao \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("id_PagtoContribuincao", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("vl_pagtoContribuicao", Convert.ToDouble(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_pagtoContribuicao", Convert.ToDateTime(dados[3]));
                conn.cmd.Parameters.AddWithValue("ds_bancoCheque", dados[4]);
                conn.cmd.Parameters.AddWithValue("ds_agenciaCheque", dados[5]);
                conn.cmd.Parameters.AddWithValue("no_chequePagto", dados[6]);

                if (dados[7].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("dt_resgChequePre", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("dt_resgChequePre", Convert.ToDateTime(dados[7]));
                }

                conn.cmd.Parameters.AddWithValue("cd_formPagto", Convert.ToInt32(dados[8]));
                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToDouble(dados[9]));
                conn.cmd.Parameters.AddWithValue("dt_anoRefContribuicao", Convert.ToInt32(dados[10]));
                conn.cmd.Parameters.AddWithValue("dt_mesRefContribuicao", Convert.ToInt32(dados[11]));

                if (dados[12].Equals(""))
                {
                    conn.cmd.Parameters.AddWithValue("sq_chequePagto", DBNull.Value);
                }
                else
                {
                    conn.cmd.Parameters.AddWithValue("sq_chequePagto", Convert.ToInt32(dados[12]));
                }
                conn.cmd.Parameters.AddWithValue("cd_tipoContribuicao", Convert.ToInt32(dados[13]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar pagamento contribuição\n" + ex.Message);
            }
        }

        #endregion

        #region Atualiza Dados Requerimento

        private bool verificaEstadoRequerimento(String codigo)
        {
            bool retorno = false;

            try
            {
                String sql = "select COUNT(*) from tbRequerimentoBeneficio "+
                             "where no_reqBeneficio  = " + codigo;

                if (conn.retornoInteiro(sql) > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar requerimento " + ex.Message);
            }

            return retorno;
        }

        private void carregaRequerimento(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " SET IDENTITY_INSERT tbRequerimentoBeneficio ON \n ";
                sql = sql + "  \n ";
                sql = sql + " INSERT INTO tbRequerimentoBeneficio \n ";
                sql = sql + "            (no_reqBeneficio \n ";
                sql = sql + "            ,cd_cadastro \n ";
                sql = sql + "            ,cd_tipoBeneficio \n ";
                sql = sql + "            ,dt_Requerimento \n ";
                sql = sql + "            ,cd_sitRequerimento) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@no_reqBeneficio \n ";
                sql = sql + "            ,@cd_cadastro \n ";
                sql = sql + "            ,@cd_tipoBeneficio \n ";
                sql = sql + "            ,@dt_Requerimento \n ";
                sql = sql + "            ,@cd_sitRequerimento) \n ";
                sql = sql + "  \n ";
                sql = sql + " SET IDENTITY_INSERT tbRequerimentoBeneficio OFF \n ";


                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("no_reqBeneficio", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("cd_tipoBeneficio", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("dt_Requerimento", Convert.ToDateTime(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_sitRequerimento", Convert.ToInt32(dados[5]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar requerimento\n" + ex.Message);
            }
        }

        private void alteraRequerimento(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " UPDATE tbRequerimentoBeneficio \n ";
                sql = sql + "    SET cd_cadastro = @cd_cadastro \n ";
                sql = sql + "       ,cd_tipoBeneficio = @cd_tipoBeneficio \n ";
                sql = sql + "       ,dt_Requerimento = @dt_Requerimento \n ";
                sql = sql + "       ,cd_sitRequerimento = @cd_sitRequerimento \n ";
                sql = sql + "  WHERE no_reqBeneficio = @no_reqBeneficio \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("no_reqBeneficio", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("cd_tipoBeneficio", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("dt_Requerimento", Convert.ToDateTime(dados[4]));
                conn.cmd.Parameters.AddWithValue("cd_sitRequerimento", Convert.ToInt32(dados[5]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar requerimento \n" + ex.Message);
            }
        }

        #endregion

        #region Atualiza Dados Valor

        private bool verificaEstadoValor(String codigo)
        {
            bool retorno = false;

            try
            {
                String sql = "select COUNT(*) from tbValBeneficio " +
                             "where no_seqBeneficio  = " + codigo;

                if (conn.retornoInteiro(sql) > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar valor " + ex.Message);
            }

            return retorno;
        }

        private void carregaValor(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " SET IDENTITY_INSERT tbValBeneficio ON \n ";
                sql = sql + "  \n ";
                sql = sql + " INSERT INTO tbValBeneficio \n ";
                sql = sql + "            (no_reqBeneficio \n ";
                sql = sql + "            ,no_seqBeneficio \n ";
                sql = sql + "            ,dt_Valor \n ";
                sql = sql + "            ,vl_Beneficio) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@no_reqBeneficio \n ";
                sql = sql + "            ,@no_seqBeneficio \n ";
                sql = sql + "            ,@dt_Valor \n ";
                sql = sql + "            ,@vl_Beneficio) \n ";
                sql = sql + " SET IDENTITY_INSERT tbValBeneficio OFF \n ";


                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("no_reqBeneficio", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("no_seqBeneficio", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_Valor", Convert.ToDateTime(dados[3]));
                conn.cmd.Parameters.AddWithValue("vl_Beneficio", Convert.ToDouble(dados[4]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar valor\n" + ex.Message);
            }
        }

        private void alteraValor(String[] dados)
        {
            try
            {
                String sql = "";

                sql = sql + " UPDATE tbValBeneficio \n ";
                sql = sql + "    SET no_reqBeneficio = @no_reqBeneficio \n ";
                sql = sql + "       ,dt_Valor = @dt_Valor \n ";
                sql = sql + "       ,vl_Beneficio = @vl_Beneficio \n ";
                sql = sql + "  WHERE no_seqBeneficio = @no_seqBeneficio \n ";


                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("no_reqBeneficio", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("no_seqBeneficio", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_Valor", Convert.ToDateTime(dados[3]));
                conn.cmd.Parameters.AddWithValue("vl_Beneficio", Convert.ToDouble(dados[4]));

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar valor \n" + ex.Message);
            }
        }

        #endregion

        #region Atualiza Dados Doações

        private bool verificaEstadoDoacoes(String codigo, String ano, String mes)
        {
            bool retorno = false;

            try
            {
                String sql = "select COUNT(*) from tbPagamentoMensal " +
                             "where cd_cadastro  = " + codigo +
                             " and dt_anoPagamento = "+ ano +
                             " and dt_mesPagamento = "+mes;

                if (conn.retornoInteiro(sql) > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao consultar doação " + ex.Message);
            }

            return retorno;
        }

        private void carregaDoacao(String[] dados)
        {
            try
            {
                String sql = "";
                sql = sql + " INSERT INTO tbPagamentoMensal \n ";
                sql = sql + "            (cd_cadastro \n ";
                sql = sql + "            ,dt_anoPagamento \n ";
                sql = sql + "            ,dt_mesPagamento \n ";
                sql = sql + "            ,cd_tipRubrica \n ";
                sql = sql + "            ,no_Rubrica \n ";
                sql = sql + "            ,no_freqRubrica \n ";
                sql = sql + "            ,vl_Rubrica \n ";
                sql = sql + "            ,st_pagamentoMensal) \n ";
                sql = sql + "      VALUES \n ";
                sql = sql + "            (@cd_cadastro \n ";
                sql = sql + "            ,@dt_anoPagamento \n ";
                sql = sql + "            ,@dt_mesPagamento \n ";
                sql = sql + "            ,@cd_tipRubrica \n ";
                sql = sql + "            ,@no_Rubrica \n ";
                sql = sql + "            ,@no_freqRubrica \n ";
                sql = sql + "            ,@vl_Rubrica \n ";
                sql = sql + "            ,@st_pagamentoMensal) \n ";

                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("dt_anoPagamento", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_mesPagamento", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("cd_tipRubrica", Convert.ToDouble(dados[4]));
                conn.cmd.Parameters.AddWithValue("no_Rubrica", Convert.ToInt32(dados[5]));
                conn.cmd.Parameters.AddWithValue("no_freqRubrica", Convert.ToInt32(dados[6]));
                conn.cmd.Parameters.AddWithValue("vl_Rubrica", Convert.ToDouble(dados[7]));
                conn.cmd.Parameters.AddWithValue("st_pagamentoMensal", dados[8]);

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar doação\n" + ex.Message);
            }
        }

        private void alteraDoacao(String[] dados)
        {
            try
            {
                String sql = "";

                sql = sql + " UPDATE tbPagamentoMensal \n ";
                sql = sql + "    SET no_freqRubrica = @no_freqRubrica \n ";
                sql = sql + "       ,vl_Rubrica = @vl_Rubrica \n ";
                sql = sql + "       ,st_pagamentoMensal = @st_pagamentoMensal \n ";
                sql = sql + "  WHERE cd_cadastro = @cd_cadastro \n ";
                sql = sql + "       and dt_anoPagamento = @dt_anoPagamento \n ";
                sql = sql + "       and dt_mesPagamento = @dt_mesPagamento \n ";
                sql = sql + "       and cd_tipRubrica = @cd_tipRubrica \n ";
                sql = sql + "       and no_Rubrica = @no_Rubrica \n ";



                conn.cmd.CommandText = sql;

                conn.cmd.Parameters.AddWithValue("cd_cadastro", Convert.ToInt32(dados[1]));
                conn.cmd.Parameters.AddWithValue("dt_anoPagamento", Convert.ToInt32(dados[2]));
                conn.cmd.Parameters.AddWithValue("dt_mesPagamento", Convert.ToInt32(dados[3]));
                conn.cmd.Parameters.AddWithValue("cd_tipRubrica", Convert.ToDouble(dados[4]));
                conn.cmd.Parameters.AddWithValue("no_Rubrica", Convert.ToInt32(dados[5]));
                conn.cmd.Parameters.AddWithValue("no_freqRubrica", Convert.ToInt32(dados[6]));
                conn.cmd.Parameters.AddWithValue("vl_Rubrica", Convert.ToDouble(dados[7]));
                conn.cmd.Parameters.AddWithValue("st_pagamentoMensal", dados[8]);

                conn.cmd.ExecuteNonQuery();
                conn.cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar doação \n" + ex.Message);
            }
        }

        #endregion


        #region apoio

        private void gerarLinhasTabela(ref StreamWriter arquivo, DataTable dados, char tipo, ref int count) {
            String linha = "";
            foreach (DataRow row in dados.Rows)
            {
                linha = tipo+Preencher(count.ToString(),"0",0,5)+"|";
                foreach (var item in row.ItemArray) {
                    linha+=item + "|";
                }
                
                progressBar1.Value = count;
                count++;
                arquivo.WriteLine(linha);

            }
        }

        public string Preencher(string PVal, string PIns, int PLado, int PNum)
        //{PVal: string que será completada com o caracter PIns
        //PIns: o caracter que será inserido a PVal
        //PLado: O Lado em que o caracter PIns será inserido. Podendo ser o lado direiro ou esquerdo da string PVal
        // PNum: O número de vezes que o caracter PIns será inserido
        //var //i : Integer;
        {
            string Aux;
            Aux = PVal;
            if (Aux.Length < PNum)

            //       i:= 0;
            {
                while (Aux.Length < PNum)
                {
                    if (PLado == 1)
                    {
                        Aux = Aux + PIns;
                    }
                    else
                    {
                        if (PLado == 0)
                        {
                            Aux = PIns + Aux;
                        }
                    }
                }
              
            }
            else
            {
                if (Aux.Length > PNum)
                {
                    Aux = Aux;
                }
            }
            return Aux;
        }
        #endregion
        
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            int count = 1;
            String linha ="";
            try
            {

                StreamReader arquivo = new StreamReader(txNomeArquivo.Text);
                linha = arquivo.ReadLine();

                int maximo = Convert.ToInt32(lblNumCadastroRestore.Text) +
                             Convert.ToInt32(lblNumContribuicoesRestore.Text) +
                             Convert.ToInt32(lblContrbEfetivasRestores.Text) +
                             Convert.ToInt32(lblDoacaoSolicitRest.Text) +
                             Convert.ToInt32(lblDoacaoConsedidaRest.Text) +
                             Convert.ToInt32(lblNumDoacaoRest.Text);
                    
                progressBar2.Minimum = 1;
                progressBar2.Maximum = maximo;
                
                conn.abreBanco();
                while (arquivo.Peek() != -1)
                {
                    linha = arquivo.ReadLine();
                    string s = linha.Substring(0, 1);
                    if (linha.Length > 1)
                    {
                        if (linha.Contains("|"))
                        {
                            String[] dados = linha.Split('|');

                            
                            if (dados[0].Substring(0, 1) == "1")
                            {
                                if (verificaEstadoContribuinte(dados[1]))
                                {
                                    alteraContribuinte(dados);
                                }
                                else
                                {
                                    carregaContribuinte(dados);
                                }
                            }
                            
                            if (dados[0].Substring(0, 1) == "2")
                            {
                                if (verificaEstadoContribuicao(dados[1], dados[2], dados[3], dados[5]))
                                {
                                    alteraContribuicao(dados);
                                }
                                else
                                {
                                    carregaContribuicao(dados);
                                }
                            }
                            
                            if (dados[0].Substring(0, 1) == "3")
                            {
                                if (verificaEstadoPagtoContribuicao(dados[1]))
                                {
                                    alteraPagtoContribuicao(dados);
                                }
                                else
                                {
                                    carregaPagtoContribuicao(dados);
                                }
                            }
                            
                            if (dados[0].Substring(0, 1) == "4")
                            {
                                if (verificaEstadoRequerimento(dados[1]))
                                {
                                    alteraRequerimento(dados);
                                }
                                else
                                {
                                    carregaRequerimento(dados);
                                }
                            }
                            
                            if (dados[0].Substring(0, 1) == "5")
                            {
                                if (verificaEstadoValor(dados[2]))
                                {
                                    alteraValor(dados);
                                }
                                else
                                {
                                    carregaValor(dados);
                                }
                            }
                            
                            if (dados[0].Substring(0, 1) == "6")
                            {
                                if (verificaEstadoDoacoes(dados[1], dados[2], dados[3]))
                                {
                                    alteraDoacao(dados);
                                }
                                else
                                {
                                    carregaDoacao(dados);
                                }
                            }



                            progressBar2.Value = count;
                            count++;
                        }
                    }
                }   
                btnRestaurar.Enabled = false;
                MessageBox.Show("Arquivo carregado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar linha "+count.ToString()+" arquivo.\n"+linha+".\n" + ex.Message);
            }
            finally {
                conn.fechaBanco();

            }
        }

       

        
        
    }
}
