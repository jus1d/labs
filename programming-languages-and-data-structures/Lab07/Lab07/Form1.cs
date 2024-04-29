using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab07
{
    public partial class Form1 : Form
    {
        public List<IVectorable> vectors = new List<IVectorable>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IVectorable vec = new LinkedListVector(Convert.ToInt32(textBox1.Text));
                vectors.Add(vec);
                listBox1.Items.Add(vectors.Count + "\tLinkedListVector\t" + vec.ToString());
            }
            catch
            {
                MessageBox.Show("Длина вектора введена неправильно", "Ошибка!");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                IVectorable vec = new ArrayVector(Convert.ToInt32(textBox1.Text));
                vectors.Add(vec);
                listBox1.Items.Add(vectors.Count + "\tArrayVector\t" + vec.ToString());
            }
            catch
            {
                MessageBox.Show("Длина вектора введена неправильно", "Ошибка!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }
            
            try
            {
                int idx = listBox1.SelectedIndex;
                vectors.RemoveAt(idx - 1);
                listBox1.Items.RemoveAt(idx);
                for (int i = idx; i <= vectors.Count; i++)
                {
                    string str = (string)listBox1.Items[i];
                    int k = Convert.ToInt32((str.Split())[0]) - 1;
                    str = Convert.ToString(k) + str.Substring(1);
                    listBox1.Items.RemoveAt(i);
                    listBox1.Items.Insert(i, str);
                }
            }
            catch
            {
                MessageBox.Show("Выберете вектор для удаления", "Ошибка!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }
            
            try
            {
                int n = listBox1.SelectedIndex;
                if (vectors[n - 1] is ArrayVector)
                {
                    vectors.Add((IVectorable)(((ArrayVector)vectors[n - 1]).Clone()));
                    listBox1.Items.Add(vectors.Count + "\tArrayVector\t" + vectors[n - 1].ToString());
                }
                else
                {
                    vectors.Add((IVectorable)(((LinkedListVector)vectors[n - 1]).Clone()));
                    listBox1.Items.Add(vectors.Count + "\tLinkedListVector\t" + vectors[n - 1].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Выберете вектор для удаления", "Ошибка!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }
                    
                    
            for (int i = 0; i < vectors.Count - 1; i++)
            {
                for (int j = i + 1; j < vectors.Count; j++)
                {
                    int compareResult;
                    if (vectors[i] is ArrayVector)
                    {
                        compareResult = (vectors[i] as ArrayVector).CompareTo(vectors[j]);
                    }
                    else
                    {
                        compareResult = (vectors[i] as LinkedListVector).CompareTo(vectors[j]);
                    }
                            
                    if (compareResult > 0)
                    {
                        IVectorable tmp = vectors[i];
                        vectors[i] = vectors[j];
                        vectors[j] = tmp;
                    }
                }
            }

            ClearVectorsBox();
            ShowVectorsBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }
        
            IVectorable tmp;
            VectorsComparer comparer = new VectorsComparer();

            for (int i = 0; i < vectors.Count - 1; i++)
            {
                for (int j = i + 1; j < vectors.Count; j++)
                {
                    if (comparer.Compare(vectors[i], vectors[j]) > 0)
                    {
                        tmp = vectors[j];
                        vectors[j] = vectors[i];
                        vectors[i] = tmp;
                    }
                }
            }
            
            ClearVectorsBox();
            ShowVectorsBox();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }

            int idx = listBox1.SelectedIndex;

            if (idx == 0)
            {
                MessageBox.Show("Выберете вектор для расчета модуля", "Ошибка!");
                return;
            }

            try
            {
                MessageBox.Show($"Вектор: {vectors[idx - 1]}, модуль: {vectors[idx - 1].GetNorm()}", "Ответ!");
            }
            catch
            {
                MessageBox.Show("Выберете вектор для удаления", "Ошибка!");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (vectors.Count == 0)
            {
                MessageBox.Show("Список векторов пуст!", "Внимание!");
                return;
            }

            int idx = listBox1.SelectedIndex;

            if (idx == 0)
            {
                MessageBox.Show("Выберете вектор для расчета хэш-кода", "Ошибка!");
                return;
            }

            try
            {
                int hashCode = vectors[idx - 1].GetHashCode();
                MessageBox.Show($"Вектор: {vectors[idx - 1]}, хэш-код (hex): 0x{Convert.ToString(hashCode, 16)}", "Ответ!");
            }
            catch
            {
                MessageBox.Show("Выберете вектор для удаления", "Ошибка!");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Stream form = new Stream();
            form.Owner = this;
            form.ShowDialog();
            
            ClearVectorsBox();
            ShowVectorsBox();
        }

        private void ClearVectorsBox()
        {
            while (listBox1.Items.Count > 1)
            {
                listBox1.Items.RemoveAt(1);
            }
        }

        private void ShowVectorsBox()
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                if (vectors[i] is ArrayVector)
                {
                    listBox1.Items.Add((i + 1).ToString() + "\tArrayVector\t" + vectors[i].ToString());
                }
                else
                {
                    listBox1.Items.Add((i + 1).ToString() + "\tLinkedListVector\t" + vectors[i].ToString());
                }
            }
        }
    }
}
