namespace UdemyConcurrencia
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.loadingGIF = new System.Windows.Forms.PictureBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pgProcesamiento = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.loadingGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // loadingGIF
            // 
            this.loadingGIF.Image = ((System.Drawing.Image)(resources.GetObject("loadingGIF.Image")));
            this.loadingGIF.Location = new System.Drawing.Point(61, 151);
            this.loadingGIF.Name = "loadingGIF";
            this.loadingGIF.Size = new System.Drawing.Size(120, 85);
            this.loadingGIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingGIF.TabIndex = 1;
            this.loadingGIF.TabStop = false;
            this.loadingGIF.Visible = false;
            this.loadingGIF.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(61, 85);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(94, 29);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(160, 27);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(125, 27);
            this.txtInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nombre";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pgProcesamiento
            // 
            this.pgProcesamiento.Location = new System.Drawing.Point(61, 254);
            this.pgProcesamiento.Name = "pgProcesamiento";
            this.pgProcesamiento.Size = new System.Drawing.Size(125, 29);
            this.pgProcesamiento.TabIndex = 5;
            this.pgProcesamiento.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pgProcesamiento);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.loadingGIF);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loadingGIF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox loadingGIF;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pgProcesamiento;
    }
}

