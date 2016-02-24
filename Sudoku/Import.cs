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
    public partial class Import : Form
    {
        
        public Import()
        {
            InitializeComponent();
          
        }

        public string SudokuString
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        private void Import_Load(object sender, EventArgs e)
        {

            this.Location = new Point(SudokuForm.ActiveForm.Location.X + 752 / 2 - this.Width / 2, SudokuForm.ActiveForm.Location.Y + 38);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
