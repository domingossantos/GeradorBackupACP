using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace GeradorBackup
{
    class ConexaoDB
    {
        public ConexaoDB(String strConexao)
        {
            this._conexao = new SqlConnection();
            this._conexao.ConnectionString = strConexao;
            this._cmd = new SqlCommand();
            this._cmd.Connection = _conexao;
            this._dataAdapter = new SqlDataAdapter();
            this._dataSet = new DataSet();
        }

        private SqlTransaction _trans;
        public SqlTransaction trans {
            set { this._trans = value;  }
            get { return this.trans; }
        }

        private SqlConnection _conexao;
        public SqlConnection conexao
        {
            set { this._conexao = value; }
            get { return this._conexao; }
        }

        private SqlCommand _cmd;
        public SqlCommand cmd
        {
            set { this._cmd = value; }
            get { return this._cmd; }
        }

        private SqlDataAdapter _dataAdapter;
        public SqlDataAdapter dataAdapter
        {
            set { this._dataAdapter = value; }
            get { return this._dataAdapter; }
        }

        private DataSet _dataSet;
        public DataSet dataSet
        {
            set { this._dataSet = value; }
            get { return this._dataSet; }
        }



        public void abreBanco() { if (_conexao.State == ConnectionState.Closed) _conexao.Open(); }

        public void fechaBanco() { if (_conexao.State == ConnectionState.Open) _conexao.Close(); }


        public DataTable retornarDataSet(String sql)
        {
            _dataAdapter = new SqlDataAdapter(sql, _conexao);
            
            DataTable tabela = new DataTable();
            _dataAdapter.Fill(tabela);
            _dataAdapter.Dispose();
            return tabela;
        }

        public int retornoInteiro(String sql)
        {
            int num = 0;

            _cmd.CommandText = sql;

            if (_trans != null) {
                _cmd.Transaction = _trans;
            }

            SqlDataReader dataReader = _cmd.ExecuteReader();

            if (dataReader.HasRows)
            {
                dataReader.Read();
                num = dataReader.GetInt32(0);
            }
            dataReader.Close();
            return num;
        }

        public SqlDataReader retornoDataReader(String sql) {
            _cmd.CommandText = sql;
            
            if (_trans != null)
            {
                _cmd.Transaction = _trans;
            }

            SqlDataReader dr = _cmd.ExecuteReader();

            return dr;
        }


        public void executaSemRetorno(String sql)
        {
            if (_trans != null)
            {
                _cmd.Transaction = _trans;
            }
            _cmd.CommandText = sql;
            _cmd.ExecuteNonQuery();
        }

    }
}
