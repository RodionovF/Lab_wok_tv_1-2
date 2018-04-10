using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Lab_2_TV
{
    public partial class Form1 : Form
    {
        public int n;
        public int size;
        double min;
        double alfa;
        double st_sv;
        double max;
        double[] ksi = null;
        double[] zi = null;
        double[] ni = null;
        double[] board = null;
        double lambda;
        public Form1()
        {
            InitializeComponent();
        }
        public void Computing()
            {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Rows.Clear();
            var random = new Random(DateTime.Now.Millisecond);
            double tmp;
            lambda = Convert.ToDouble(textBox1.Text); // параметр распределения
            

            n = Convert.ToInt32(textBox2.Text);
           ksi = new double[n];

            for (int j = 0; j < n; ++j)
            {
                ksi[j] = 0;
            }
            for (int j = 0; j < n; ++j)
            {
                tmp = random.NextDouble();
                ksi[j] = -((Math.Log(tmp)) / lambda);
            }
            Array.Sort(ksi);
            min = ksi[0];
            max = ksi[n - 1];
           for (int j = 0; j < n; ++j)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j);
                dataGridView1.Rows[j].Cells[1].Value = Convert.ToString(ksi[j]);

            }
        }

        public void Counted()
        {
        }
        public void Painted()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Computing();
            Counted();
            Painted();
        }

        public void Form_cotrol(TextBox textBox1, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (e.KeyChar == '.')
            {
                // точку заменим запятой
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {

                    // запятая уже есть в поле редактирования
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }

        public void mod_form_conrtol(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form_cotrol(textBox1, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            alfa = Convert.ToDouble(textBox4.Text);
            st_sv = size - 1;
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }

            dataGridView3.Rows.Clear();
            dataGridView3.ColumnCount = size+2;
            dataGridView3.RowCount = 4;

            dataGridView2.Rows[0].Cells[0].Value = Convert.ToString(Int64.MinValue);
            for (int j = 0; j < size; ++j)
            {
                board[j] = Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value);
            }
            dataGridView2.Rows[size].Cells[0].Value = Convert.ToString(Int64.MaxValue);

            for (int j = 0; j < size+1; ++j)
            {

                zi[j] = (Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value));
                dataGridView3.Rows[0].Cells[j+1].Value = Convert.ToString(zi[j]);
            }

            Refresh();

            for (int i = 0; i < n; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    if ((ksi[i] <= zi[j]) && (ksi[i] >= zi[j - 1]))
                    {
                        ni[j - 1]++;
                        break;
                    }
                }

            }
            double R0 = 0;
            double tt = 0;
            double pp = 0;

            dataGridView3.Rows[1].Cells[0].Value = Convert.ToString("nj");
            dataGridView3.Rows[2].Cells[0].Value = Convert.ToString("qj");
            dataGridView3.Rows[3].Cells[0].Value = Convert.ToString("R0");

            for (int j = 0; j < size; j++)
            {
                dataGridView3.Rows[1].Cells[j+2].Value = Convert.ToString(ni[j]);
                if (zi[j] < 0)
                    tt = 1 - Math.Pow(Math.E, 0);
                else
                    tt = 1 - Math.Pow(Math.E, -lambda * (zi[j]));
                if (zi[j + 1] < 0)
                    pp = 1 - Math.Pow(Math.E, 0);
                else
                    pp = 1 - Math.Pow(Math.E, -lambda * (zi[j + 1]));
                dataGridView3.Rows[2].Cells[j + 2].Value = (pp - tt);
                R0 += Math.Pow(ni[j] - n * (pp - tt), 2) / (n * (pp - tt));
                dataGridView3.Rows[3].Cells[j + 2].Value = R0;
            }
            label5.Text = Convert.ToString(R0);


            double FR0 = 0;
            double xi2 = 0;
            for (int i = 1; i < n; i++)
                xi2 += Math.Pow(2, -st_sv / 2) * Math.Pow(SpecialFunction.gamma(st_sv / 2), -1) * (Math.Pow(Math.E, -(R0 * (i - 1) / n) / 2) * Math.Pow((R0 * (i - 1) / n), st_sv / 2 - 1) + Math.Pow(Math.E, -(R0 * i / n) / 2) * Math.Pow((R0 * i / n), st_sv / 2 - 1));

            xi2 = xi2 * R0 / (2 * n);
            FR0 = 1 - xi2;


            label8.Text = Convert.ToString(FR0);

            if(FR0<alfa)
                label9.Text = Convert.ToString("Гипотеза принята");
            else
                label9.Text = Convert.ToString("Гипотеза отвержена");
            R0 = 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            alfa = Convert.ToDouble(textBox4.Text);
            st_sv = size-1;
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView2.RowCount = size;
            dataGridView2.ColumnCount = 1;
            dataGridView3.ColumnCount = size+2;
            dataGridView3.RowCount = 4;

            dataGridView2.Rows[0].Cells[0].Value = Convert.ToString(Int64.MinValue);
            for (int j = 1; j < size; ++j)
            {
                dataGridView2.Rows.Add();
                board[j] = (min + ((max - min) / size) * j);
                dataGridView2.Rows[j].Cells[0].Value = Convert.ToString(board[j]);

            }
            dataGridView2.Rows[size].Cells[0].Value = Convert.ToString(Int64.MaxValue);

            for (int j = 0; j < size+1; ++j)
            {

                zi[j] = (Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value));
                dataGridView3.Rows[0].Cells[j+1].Value = Convert.ToString(zi[j]);
            }

            Refresh();

            for (int i = 0; i < n; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    if ((ksi[i] <=zi[j]) && (ksi[i] >= zi[j - 1]))
                    {
                        ni[j - 1]++;


                        break;
                    }
                }

            }
            double R0 = 0;
            double tt = 0;
            double pp = 0;

            dataGridView3.Rows[1].Cells[0].Value = Convert.ToString("nj");
            dataGridView3.Rows[2].Cells[0].Value = Convert.ToString("qj");
            dataGridView3.Rows[3].Cells[0].Value = Convert.ToString("R0");

            for (int j = 0; j < size; j++)
            {
                dataGridView3.Rows[1].Cells[j+2].Value = Convert.ToString(ni[j]);
                if (zi[j]<0)
                     tt = 1 - Math.Pow(Math.E, 0);
                else
                     tt = 1 - Math.Pow(Math.E, -lambda * (zi[j]));
                if (zi[j+1] < 0)
                    pp = 1 - Math.Pow(Math.E, 0);
                else
                    pp = 1 - Math.Pow(Math.E, -lambda * (zi[j + 1]));
                dataGridView3.Rows[2].Cells[j+2].Value = (pp-tt);
                R0 += Math.Pow(ni[j] - n * (pp - tt), 2) / (n * (pp - tt));
                dataGridView3.Rows[3].Cells[j + 2].Value = R0;
            }
            label5.Text = Convert.ToString(R0);
            double FR0 = 0;
            double xi2 = 0;
            for (int i = 1; i < n; i++)
                xi2 += Math.Pow(2, -st_sv / 2) * Math.Pow(SpecialFunction.gamma(st_sv / 2), -1) * (Math.Pow(Math.E, -(R0 * (i-1) / n) / 2) * Math.Pow((R0 * (i-1) / n), st_sv / 2 - 1) + Math.Pow(Math.E, -(R0 * i / n) / 2) * Math.Pow((R0 * i / n), st_sv / 2 - 1));

            xi2 = xi2 * R0 / (2 * n);
            FR0 = 1 - xi2;


            label8.Text = Convert.ToString(FR0);


            if (FR0 < alfa)
                label9.Text = Convert.ToString("Гипотеза принята");
            else
                label9.Text = Convert.ToString("Гипотеза отвержена");

            R0 = 0;


        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            size = Convert.ToInt32(textBox3.Text);

            if (size > 2)
            {
                dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView2.Rows.Clear();
                dataGridView2.RowCount = size + 1;
                dataGridView2.ColumnCount = 1;
                dataGridView2.Rows[0].Cells[0].Value = Convert.ToString(Int64.MinValue);
               // dataGridView2.Rows[1].Cells[0].Value = Convert.ToString(min+0.5);
                dataGridView2.Rows[size].Cells[0].Value = Convert.ToString(Int64.MaxValue);
               // dataGridView2.Rows[size-1].Cells[0].Value = Convert.ToString(max-0.5);

                dataGridView3.Rows.Clear();
                dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView3.ColumnCount = size;
                dataGridView3.RowCount = 3;
                ni = new double[size + 1];
                zi = new double[size + 1];
                board = new double[size + 1];
                for (int j = 0; j < size; ++j)
                {
                    ni[j] = 0;
                }
                Refresh();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }
    }
}
