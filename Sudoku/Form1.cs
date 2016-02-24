using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnGenerate.Location = new Point(505, 51);
            btnSolve.Location = new Point(599, 51);
            comboBox2.Location = new Point(599, 52);
            comboBox2.SelectedIndex = 0;
            
            
        }

        Label[] labels = new Label[81];
        int activeLbl = 81;
        int hoverLbl;

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 81; i++)
            {
                labels[i] = (Label)panel1.Controls[i];
                labels[i].TabIndex = i;

            }
            
    
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            label1.Text = e.KeyData.ToString() + " " + e.KeyValue + " " + e.KeyCode + "  " + activeLbl;
            if (e.KeyValue >= 49 && e.KeyValue <= 57) 
            {
                labels[hoverLbl].Text = (e.KeyValue - 48).ToString();
            }
            
        }

        private void lblB1X1Y1_Click(object sender, EventArgs e)
        {

            Label lbl = sender as Label;
            if (lbl.Text != "")
            {
                lbl.BackColor = Color.White;
                lbl.Text = "";
            }
        }

        private void lblB1X1Y1_MouseEnter(object sender, EventArgs e)
        {
            if (labels[hoverLbl].Text == "") labels[hoverLbl].BackColor = Color.White;
            else labels[hoverLbl].BackColor = Color.SeaShell;
            labels[hoverLbl].BorderStyle = BorderStyle.None;

            Label lbl = sender as Label;
            lbl.BackColor = Color.WhiteSmoke;
            lbl.BorderStyle = BorderStyle.FixedSingle;
            hoverLbl = lbl.TabIndex;

            label1.Text = activeLbl + "  " + hoverLbl + "  " + labels[hoverLbl].Text;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (btnGenerate.Location.X == 505)
            {
                while (btnGenerate.Location.X >= 409)
                {
                    btnGenerate.Location = new Point(btnGenerate.Location.X - 7, btnGenerate.Location.Y);
                    comboBox2.Location = new Point(comboBox2.Location.X - 7, comboBox2.Location.Y);
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
    }
}
