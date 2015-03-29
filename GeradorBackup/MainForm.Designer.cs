namespace GeradorBackup
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnVerificar = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEfetivadas = new System.Windows.Forms.Label();
            this.lblConcecoes = new System.Windows.Forms.Label();
            this.lblSolicitacoes = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblNumDoacoes = new System.Windows.Forms.Label();
            this.lblNumContribuicoes = new System.Windows.Forms.Label();
            this.lblNumCadastros = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtCopia = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnBuscarArquivo = new System.Windows.Forms.Button();
            this.txNomeArquivo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblContrbEfetivasRestores = new System.Windows.Forms.Label();
            this.lblNumContribuicoesRestore = new System.Windows.Forms.Label();
            this.lblNumCadastroRestore = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDoacaoConsedidaRest = new System.Windows.Forms.Label();
            this.lblDoacaoSolicitRest = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblNumDoacaoRest = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(437, 223);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnVerificar);
            this.tabPage1.Controls.Add(this.btnBackup);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.dtCopia);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(429, 197);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fazer Cópia";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnVerificar
            // 
            this.btnVerificar.Location = new System.Drawing.Point(183, 9);
            this.btnVerificar.Name = "btnVerificar";
            this.btnVerificar.Size = new System.Drawing.Size(75, 23);
            this.btnVerificar.TabIndex = 7;
            this.btnVerificar.Text = "Verificar";
            this.btnVerificar.UseVisualStyleBackColor = true;
            this.btnVerificar.Click += new System.EventHandler(this.btnVerificar_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Enabled = false;
            this.btnBackup.Location = new System.Drawing.Point(346, 159);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 6;
            this.btnBackup.Text = "Executar Cópia";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 159);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(329, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblEfetivadas);
            this.groupBox1.Controls.Add(this.lblConcecoes);
            this.groupBox1.Controls.Add(this.lblSolicitacoes);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.lblNumDoacoes);
            this.groupBox1.Controls.Add(this.lblNumContribuicoes);
            this.groupBox1.Controls.Add(this.lblNumCadastros);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(11, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 106);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detalhe da Cópia";
            // 
            // lblEfetivadas
            // 
            this.lblEfetivadas.AutoSize = true;
            this.lblEfetivadas.Location = new System.Drawing.Point(164, 73);
            this.lblEfetivadas.Name = "lblEfetivadas";
            this.lblEfetivadas.Size = new System.Drawing.Size(13, 13);
            this.lblEfetivadas.TabIndex = 11;
            this.lblEfetivadas.Text = "0";
            // 
            // lblConcecoes
            // 
            this.lblConcecoes.AutoSize = true;
            this.lblConcecoes.Location = new System.Drawing.Point(351, 45);
            this.lblConcecoes.Name = "lblConcecoes";
            this.lblConcecoes.Size = new System.Drawing.Size(13, 13);
            this.lblConcecoes.TabIndex = 10;
            this.lblConcecoes.Text = "0";
            // 
            // lblSolicitacoes
            // 
            this.lblSolicitacoes.AutoSize = true;
            this.lblSolicitacoes.Location = new System.Drawing.Point(351, 20);
            this.lblSolicitacoes.Name = "lblSolicitacoes";
            this.lblSolicitacoes.Size = new System.Drawing.Size(13, 13);
            this.lblSolicitacoes.TabIndex = 9;
            this.lblSolicitacoes.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(24, 73);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(126, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "Contribuições efetivadas:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(239, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Doação Consedidas:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(248, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(97, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "Doação Solicitada:";
            // 
            // lblNumDoacoes
            // 
            this.lblNumDoacoes.AutoSize = true;
            this.lblNumDoacoes.Location = new System.Drawing.Point(351, 73);
            this.lblNumDoacoes.Name = "lblNumDoacoes";
            this.lblNumDoacoes.Size = new System.Drawing.Size(13, 13);
            this.lblNumDoacoes.TabIndex = 5;
            this.lblNumDoacoes.Text = "0";
            // 
            // lblNumContribuicoes
            // 
            this.lblNumContribuicoes.AutoSize = true;
            this.lblNumContribuicoes.Location = new System.Drawing.Point(164, 45);
            this.lblNumContribuicoes.Name = "lblNumContribuicoes";
            this.lblNumContribuicoes.Size = new System.Drawing.Size(13, 13);
            this.lblNumContribuicoes.TabIndex = 4;
            this.lblNumContribuicoes.Text = "0";
            // 
            // lblNumCadastros
            // 
            this.lblNumCadastros.AutoSize = true;
            this.lblNumCadastros.Location = new System.Drawing.Point(164, 20);
            this.lblNumCadastros.Name = "lblNumCadastros";
            this.lblNumCadastros.Size = new System.Drawing.Size(13, 13);
            this.lblNumCadastros.TabIndex = 3;
            this.lblNumCadastros.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Número de Doações:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Número de Contribuições: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Número de Cadastros: ";
            // 
            // dtCopia
            // 
            this.dtCopia.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtCopia.Location = new System.Drawing.Point(55, 9);
            this.dtCopia.Name = "dtCopia";
            this.dtCopia.Size = new System.Drawing.Size(121, 20);
            this.dtCopia.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Data:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnBuscarArquivo);
            this.tabPage2.Controls.Add(this.txNomeArquivo);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.btnRestaurar);
            this.tabPage2.Controls.Add(this.progressBar2);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(429, 197);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Restaurar Cópia";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnBuscarArquivo
            // 
            this.btnBuscarArquivo.Location = new System.Drawing.Point(382, 8);
            this.btnBuscarArquivo.Name = "btnBuscarArquivo";
            this.btnBuscarArquivo.Size = new System.Drawing.Size(34, 23);
            this.btnBuscarArquivo.TabIndex = 14;
            this.btnBuscarArquivo.Text = "OK";
            this.btnBuscarArquivo.UseVisualStyleBackColor = true;
            this.btnBuscarArquivo.Click += new System.EventHandler(this.btnBuscarArquivo_Click);
            // 
            // txNomeArquivo
            // 
            this.txNomeArquivo.Location = new System.Drawing.Point(66, 10);
            this.txNomeArquivo.Name = "txNomeArquivo";
            this.txNomeArquivo.Size = new System.Drawing.Size(310, 20);
            this.txNomeArquivo.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Arquivo:";
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Enabled = false;
            this.btnRestaurar.Location = new System.Drawing.Point(346, 159);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(75, 23);
            this.btnRestaurar.TabIndex = 11;
            this.btnRestaurar.Text = "Executar Cópia";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(11, 159);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(329, 23);
            this.progressBar2.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblDoacaoConsedidaRest);
            this.groupBox2.Controls.Add(this.lblDoacaoSolicitRest);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.lblNumDoacaoRest);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.lblContrbEfetivasRestores);
            this.groupBox2.Controls.Add(this.lblNumContribuicoesRestore);
            this.groupBox2.Controls.Add(this.lblNumCadastroRestore);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(11, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 106);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detalhe da Cópia";
            // 
            // lblContrbEfetivasRestores
            // 
            this.lblContrbEfetivasRestores.AutoSize = true;
            this.lblContrbEfetivasRestores.Location = new System.Drawing.Point(164, 73);
            this.lblContrbEfetivasRestores.Name = "lblContrbEfetivasRestores";
            this.lblContrbEfetivasRestores.Size = new System.Drawing.Size(13, 13);
            this.lblContrbEfetivasRestores.TabIndex = 5;
            this.lblContrbEfetivasRestores.Text = "0";
            // 
            // lblNumContribuicoesRestore
            // 
            this.lblNumContribuicoesRestore.AutoSize = true;
            this.lblNumContribuicoesRestore.Location = new System.Drawing.Point(164, 45);
            this.lblNumContribuicoesRestore.Name = "lblNumContribuicoesRestore";
            this.lblNumContribuicoesRestore.Size = new System.Drawing.Size(13, 13);
            this.lblNumContribuicoesRestore.TabIndex = 4;
            this.lblNumContribuicoesRestore.Text = "0";
            // 
            // lblNumCadastroRestore
            // 
            this.lblNumCadastroRestore.AutoSize = true;
            this.lblNumCadastroRestore.Location = new System.Drawing.Point(164, 20);
            this.lblNumCadastroRestore.Name = "lblNumCadastroRestore";
            this.lblNumCadastroRestore.Size = new System.Drawing.Size(13, 13);
            this.lblNumCadastroRestore.TabIndex = 3;
            this.lblNumCadastroRestore.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Número de Contribuições: ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(41, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Número de Cadastros: ";
            // 
            // lblDoacaoConsedidaRest
            // 
            this.lblDoacaoConsedidaRest.AutoSize = true;
            this.lblDoacaoConsedidaRest.Location = new System.Drawing.Point(351, 45);
            this.lblDoacaoConsedidaRest.Name = "lblDoacaoConsedidaRest";
            this.lblDoacaoConsedidaRest.Size = new System.Drawing.Size(13, 13);
            this.lblDoacaoConsedidaRest.TabIndex = 16;
            this.lblDoacaoConsedidaRest.Text = "0";
            // 
            // lblDoacaoSolicitRest
            // 
            this.lblDoacaoSolicitRest.AutoSize = true;
            this.lblDoacaoSolicitRest.Location = new System.Drawing.Point(351, 20);
            this.lblDoacaoSolicitRest.Name = "lblDoacaoSolicitRest";
            this.lblDoacaoSolicitRest.Size = new System.Drawing.Size(13, 13);
            this.lblDoacaoSolicitRest.TabIndex = 15;
            this.lblDoacaoSolicitRest.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(239, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(106, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "Doação Consedidas:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(248, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(97, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Doação Solicitada:";
            // 
            // lblNumDoacaoRest
            // 
            this.lblNumDoacaoRest.AutoSize = true;
            this.lblNumDoacaoRest.Location = new System.Drawing.Point(351, 73);
            this.lblNumDoacaoRest.Name = "lblNumDoacaoRest";
            this.lblNumDoacaoRest.Size = new System.Drawing.Size(13, 13);
            this.lblNumDoacaoRest.TabIndex = 12;
            this.lblNumDoacaoRest.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(237, 73);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "Número de Doações:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(126, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Contribuições efetivadas:";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 223);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmMain";
            this.Text = "Gerador de BackUp";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNumDoacoes;
        private System.Windows.Forms.Label lblNumContribuicoes;
        private System.Windows.Forms.Label lblNumCadastros;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtCopia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnBuscarArquivo;
        private System.Windows.Forms.TextBox txNomeArquivo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblContrbEfetivasRestores;
        private System.Windows.Forms.Label lblNumContribuicoesRestore;
        private System.Windows.Forms.Label lblNumCadastroRestore;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnVerificar;
        private System.Windows.Forms.Label lblEfetivadas;
        private System.Windows.Forms.Label lblConcecoes;
        private System.Windows.Forms.Label lblSolicitacoes;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDoacaoConsedidaRest;
        private System.Windows.Forms.Label lblDoacaoSolicitRest;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblNumDoacaoRest;
        private System.Windows.Forms.Label label20;
    }
}

