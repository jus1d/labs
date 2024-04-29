using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Lab07
{
    public partial class Stream : Form
    {
        public Stream()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 owner = (Form1)this.Owner;
            string path = "./vectors.bin";

            List<IVectorable> vectors = owner.vectors;
            
            if (File.Exists(path)) File.Delete(path);

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                Vectors.WriteVectors(fs, vectors);
            }
        
            MessageBox.Show($"Запись векторов в файл `{path}` выполнена", "Успех!");

            owner.vectors = new List<IVectorable>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 owner = (Form1)this.Owner;
            string path = "./vectors.bin";

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    owner.vectors = Vectors.ReadVectors(fs);
                }
            }
            catch
            {
                MessageBox.Show($"Файл пуст!", "Ошибка!");
                return;
            }

            MessageBox.Show($"Чтение векторов из файла `{path}` выполнено", "Успех!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // write
            Form1 owner = (Form1)this.Owner;
            string path = "./vectors.txt";
            
            if (File.Exists(path)) File.Delete(path);
        
            using (TextWriter w = File.AppendText(path))
            {
                for (int i = 0; i < owner.vectors.Count; i++)
                {
                    Vectors.WriteVector(w, owner.vectors[i]);
                }

                MessageBox.Show($"Запись векторов в файл `{path}` выполнена", "Успех!");
            }

            owner.vectors = new List<IVectorable>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // read
            Form1 owner = (Form1)this.Owner;
            string path = "./vectors.txt";
            
            TextReader r = File.OpenText(path);
            int n = r.ReadToEnd().Split('\n').Length;
            r = File.OpenText(path);
            List<IVectorable> vectorsRead = new List<IVectorable>();
            for (int i = 0; i < n - 1; i++)
            {
                vectorsRead.Add(Vectors.ReadVector(r));
            }
            r.Close();
            
            owner.vectors = vectorsRead;
            MessageBox.Show($"Чтение векторов из файла `{path}` выполнено", "Учпех!");
        }
    }
}