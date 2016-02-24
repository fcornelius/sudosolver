using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sudoku
{

    
    public partial class SudokuForm : Form
    {
        public SudokuForm()
        {
            InitializeComponent();

            
        }

        char[] save = new char[81];
        string savefile;
        List<string> logDates = new List<string>();
        List<string> logData = new List<string>();
        string date;
        Label[] labels = new Label[81];
        Cell[] Cells = new Cell[81];
        List<int>[] blocks;
        List<int>[] Column;
        List<int>[] Row; 

        public int hoverLbl;

        private void Form1_Load(object sender, EventArgs e)
        {

            savefile = Directory.GetCurrentDirectory() + "\\" + "sudokusolvr.log";
            if (!File.Exists(savefile)) File.CreateText(savefile);

            //logDates = File.ReadAllLines(savefile).ToList<string>();
            //logData = File.ReadAllLines(savefile).ToList<string>();
            
            int l = 0;
            string[] log = File.ReadAllLines(savefile);
            foreach (string line in log)
            {
                logDates.Add(line.Substring(0, 12));
                logData.Add(line.Substring(13, 81));

                comboBox1.Items.Add(" " + DateTime.ParseExact(logDates[l], "ddMMyyHHmmss", null).ToString("dd.MM.yy, HH:mm"));

                l++;
            }
            comboBox1.Items.Add("  Clear History");

           
            
           // DateTime date2 = DateTime.ParseExact(date,"ddMMyymmHHss",null);
          // textBox1.AppendText(logDates[0] + "-" + logData[0] + date + "    ");

            label1.Visible = false;
            this.Height = 837;
                           
            //  Label.Name = lblB1X1Y1    //1-basiert
            //                   ^ ^ ^
            //         ElementAt 4 6 8

            for (int i = 0; i < 81; i++)
            {
                labels[i] = (Label)panel1.Controls[i];
                labels[i].TabIndex = i;
                
                Cells[i] = new Cell();
                Cells[i].Index = i;
                Cells[i].block = Convert.ToInt32(labels[i].Name.ElementAt(4).ToString())-1; //Null-basierung
                Cells[i].X     = Convert.ToInt32(labels[i].Name.ElementAt(6).ToString())-1;
                Cells[i].Y     = Convert.ToInt32(labels[i].Name.ElementAt(8).ToString())-1;
                Cells[i].Possible = new List<int>();
                Cells[i].Status = "unset";  // unset, set, possible

                save[i] = '.';
                
   
            }
            

            blocks = new List<int>[9];
            Column = new List<int>[9];
            Row = new List<int>[9];

            for (int i = 0; i < 9; i++)
            {
                blocks[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Column[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Row[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
 
            

            /* Label namen generieren
            for (int r = 0; r < 3; r++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    for (int x = 1; x <= 9; x++)
                    {
                        int b = (x - 1) / 3 + 1 + (3*r);
                        textBox1.AppendText("this.panel1.Controls.Add(this.lblB"+b+"X"+x+"Y"+(y + (3*r))+");" + Environment.NewLine);
                    }
                }
            } */
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue >= 49 && e.KeyValue <= 57) 
            {
                labels[hoverLbl].Text = (e.KeyValue - 48).ToString();
                save[hoverLbl] = (char)(e.KeyValue);
                labels[hoverLbl].BackColor = Color.SeaShell;
                labels[hoverLbl].ForeColor = Color.Black;
                labels[hoverLbl].Font = new Font(labels[hoverLbl].Font.FontFamily, 21);
                Cells[hoverLbl].Status = "set";
                textBox1.AppendText(new string(save) + Environment.NewLine);
            }
            
        }

        private void lblB1X1Y1_Click(object sender, EventArgs e)
        {

            Label lbl = sender as Label;
            if (lbl.Text != "")
            {
                lbl.BackColor = Color.White;
                lbl.Text = "";
                save[lbl.TabIndex] = '.';
                textBox1.AppendText(new string(save) + Environment.NewLine);
                Cells[lbl.TabIndex].Status = "unset";
            }
        }

        private void lblB1X1Y1_MouseEnter(object sender, EventArgs e)
        {

            if (Cells[hoverLbl].Status != "set") labels[hoverLbl].BackColor = Color.White;
            


           
            Label lbl = sender as Label;
            if (Cells[lbl.TabIndex].Status != "set") lbl.BackColor = Color.WhiteSmoke;
            hoverLbl = lbl.TabIndex;

            label1.Text = "Cell ID: " + lbl.TabIndex + " | Cell.Status: " + Cells[lbl.TabIndex].Status + " | Cell.X: " + Cells[lbl.TabIndex].X + " | Cell.Y: " + Cells[lbl.TabIndex].Y + " | Cell.block: " + Cells[lbl.TabIndex].block + " | Value: " + labels[lbl.TabIndex].Text;
        }

        private void lblB1X1Y1_MouseLeave(object sender, EventArgs e)
        {
          

        } 

        private void btnGenerate_Click(object sender, EventArgs e)
        {
           // Generieren
        }
        
        private void btnSolve_Click(object sender, EventArgs e)
        {

            date = DateTime.Now.ToString("ddMMyyHHmmss");

            using (StreamWriter stream = File.AppendText(savefile))
            {
                stream.WriteLine(date + " " + (new string(save)));
            }

            comboBox1.Items.Insert(comboBox1.Items.Count - 1, " " + DateTime.ParseExact(date, "ddMMyyHHmmss", null).ToString("dd.MM.yy, HH:mm"));
            logData.Add(new string(save));

            // Soft-Reset (Listen neu erstellen)
            blocks = new List<int>[9];
            Column = new List<int>[9];
            Row = new List<int>[9];

            for (int i = 0; i < 9; i++)
            {
                blocks[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Column[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Row[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }

            foreach (Cell thisCell in Cells)
            {
                thisCell.Possible = new List<int>();
            }

            //------------------------------------

           // textBox1.AppendText("Set:" + Environment.NewLine);


            // Zellen-schleife um gesetzte aus block column und row Listen zu löschen


            int changesPerRun;
            int run = 0;
            
            do

            {

                run++;
                changesPerRun = 0;

                foreach (Cell thisCell in Cells)
                {
                    if (Cells[thisCell.Index].Status != "set") continue;
                    else
                    {
                        int setNumber = Convert.ToInt32(labels[thisCell.Index].Text);

                        blocks[thisCell.block].Remove(setNumber);
                        Column[thisCell.X].Remove(setNumber);
                        Row[thisCell.Y].Remove(setNumber);

                        if (blocks[thisCell.block].Contains(setNumber) ||
                            Column[thisCell.X].Contains(setNumber) ||
                            Row[thisCell.Y].Contains(setNumber)) changesPerRun++;
                    }

                }



                foreach (Cell thisCell in Cells)
                {
                    if (Cells[thisCell.Index].Status == "set") thisCell.Possible.Add(Convert.ToInt32(labels[thisCell.Index].Text));
                    else
                    {


                        List<int> Min;

                        //int minEntries = Math.Min(blocks[thisCell.block].Count, Math.Min(Column[thisCell.X].Count, Row[thisCell.Y].Count));

                        if (blocks[thisCell.block].Count <= Column[thisCell.X].Count && blocks[thisCell.block].Count <= Row[thisCell.Y].Count)
                            Min = blocks[thisCell.block];
                        else if (Column[thisCell.X].Count <= blocks[thisCell.block].Count && Column[thisCell.X].Count <= Row[thisCell.Y].Count)
                            Min = Column[thisCell.X];
                        else
                            Min = Row[thisCell.Y];

                        thisCell.Possible.Clear();
                        foreach (int n in Min)
                        {
                            if (blocks[thisCell.block].Contains(n) && Column[thisCell.X].Contains(n) && Row[thisCell.Y].Contains(n))
                            {
                                thisCell.Possible.Add(n);
                            }
                        }

                        if (thisCell.Possible.Count == 1)
                        {
                            labels[thisCell.Index].BackColor = Color.MistyRose;
                            labels[thisCell.Index].ForeColor = Color.Black;
                            labels[thisCell.Index].Font = new Font(labels[thisCell.Index].Font.FontFamily, 21);
                            labels[thisCell.Index].Padding = new Padding(0);
                            labels[thisCell.Index].Text = thisCell.Possible[0].ToString();
                            Cells[thisCell.Index].Status = "set";

                            blocks[thisCell.block].Remove(thisCell.Possible[0]);
                            Column[thisCell.X].Remove(thisCell.Possible[0]);
                            Row[thisCell.Y].Remove(thisCell.Possible[0]);

                            changesPerRun++;


                        }
                        else
                        {
                            labels[thisCell.Index].ForeColor = Color.Silver;
                            labels[thisCell.Index].Font = new Font(labels[thisCell.Index].Font.FontFamily, 11);
                            labels[thisCell.Index].Padding = new Padding(2, 0, 0, 0);

                            labels[thisCell.Index].Text = "";

                            for (int i = 1; i <= 9; i++)
                            {
                                if (thisCell.Possible.Contains(i)) labels[thisCell.Index].Text += i + "  ";
                                else labels[thisCell.Index].Text += "    ";

                            }
                            Cells[thisCell.Index].Status = "possible";
                        }
                    }


                }

            } while (changesPerRun != 0);
        
        }

        private void btnToggleText_Click(object sender, EventArgs e)
        {
            if (this.Height == 837)
            {
                label4.Visible = false;
                label1.Visible = true;
                //while (this.Height < 1016) this.Height++;
                this.Height = 1016;
            }
            else if (this.Height == 1016)
            {
                label4.Visible = true;
                label1.Visible = false;
                // while (this.Height > 837) this.Height--;
                this.Height = 837;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            blocks = new List<int>[9];
            Column = new List<int>[9];
            Row = new List<int>[9];

            for (int i = 0; i < 9; i++)
            {
                blocks[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Column[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Row[i] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }

            foreach (Cell thisCell in Cells) 
            {
                thisCell.Possible = new List<int>();
                thisCell.Status = "unset";
                labels[thisCell.Index].Text = "";
                labels[thisCell.Index].BackColor = Color.White;
                labels[thisCell.Index].ForeColor = Color.Black;
                labels[thisCell.Index].Font = new Font(labels[thisCell.Index].Font.FontFamily, 21);
                labels[thisCell.Index].Padding = new Padding(0);

            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                DialogResult result = MessageBox.Show("Proceed to clear the history?", "Clear History", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    comboBox1.Items.Clear();
                    logData.Clear();
                    logDates.Clear();
                    File.Create(savefile).Close();
                    comboBox1.Items.Add("  Clear History");
                }
            }
            else
            {
                char[] load = logData[comboBox1.SelectedIndex].ToCharArray();
                //textBox1.AppendText(Environment.NewLine + load[0] + load[1] + load[2] + load[3]);

                foreach (Cell thisCell in Cells)
                {
                    if (load[thisCell.Index] == '.')
                    {
                        labels[thisCell.Index].Text = "";
                        labels[thisCell.Index].BackColor = Color.White;
                        thisCell.Status = "unset";
                    }
                    else
                    {
                        labels[thisCell.Index].Text = load[thisCell.Index].ToString();
                        labels[thisCell.Index].BackColor = Color.SeaShell;
                        labels[thisCell.Index].ForeColor = Color.Black;
                        labels[thisCell.Index].Font = new Font(labels[thisCell.Index].Font.FontFamily, 21);
                        thisCell.Status = "set";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            comboBox1.DroppedDown = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox2.DroppedDown = true;
        }

        private Import _Import = new Import();

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string importsudoku;

            if (comboBox2.SelectedIndex == 0)
            {
                DialogResult result = _Import.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    importsudoku = _Import.SudokuString;

                    save = importsudoku.ToCharArray();
                   

                    foreach (Cell thisCell in Cells)
                    {
                        if (save[thisCell.Index] == '.')
                        {
                            labels[thisCell.Index].Text = "";
                            labels[thisCell.Index].BackColor = Color.White;
                            thisCell.Status = "unset";
                        }
                        else
                        {
                            labels[thisCell.Index].Text = save[thisCell.Index].ToString();
                            labels[thisCell.Index].BackColor = Color.SeaShell;
                            labels[thisCell.Index].ForeColor = Color.Black;
                            labels[thisCell.Index].Font = new Font(labels[thisCell.Index].Font.FontFamily, 21);
                            thisCell.Status = "set";
                        }
                    }
                }

            }
        }

       
    }
    
    public class Cell
    {
        
        
        private int thisBlock;
        public int block // 0-basiert
        {
            get
            {
               
                return thisBlock;
            }
            set
            {
                thisBlock = value;
            }
        }

        private int thisX;
        public int X // 0-basiert
        {
            get
            {
                return thisX;
            }
            set
            {
                thisX = value;
            }
        }

        private int thisY;
        public int Y // 0-basiert
        {
            get
            {
                return thisY;
            }
            set
            {
                thisY = value;
            }
        }
        
        private int thisIndex;
        public int Index //0-basiert
        {
            get
            {
                return thisIndex;
            }
            set
            {
                thisIndex = value;
            }
        }

        private List<int> possible;
        public List<int> Possible 
        {
            get
            {
                return possible;
            }
            set
            {
                possible = value;
            }
        }

        private String thisStatus;
        public String Status //0-basiert
        {
            get
            {
                return thisStatus;
            }
            set
            {
                thisStatus = value;
            }
        }
        

    } 
}
