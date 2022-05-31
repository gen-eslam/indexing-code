using System.IO;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FileStream fileStream;
        StreamReader streamReader;
        StreamWriter streamWriter;
        SortedDictionary<int,int> dictionary = new SortedDictionary<int,int>();
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)// open file
        {
            fileStream = new FileStream("test.txt",FileMode.OpenOrCreate,FileAccess.ReadWrite);
            streamReader = new StreamReader(fileStream);
            streamWriter = new StreamWriter(fileStream);
            MessageBox.Show("opend");


        }

        private void button2_Click(object sender, EventArgs e)// load index file to dictionary
        {
            dictionary.Clear();//clear doctinary
            textBox4.Text = "KEY\tLOC\r\n";
            if (File.Exists("index.txt"))
            {
                StreamReader reader = new StreamReader("index.txt");
                String? line; // ?=> use for null safety ===>>> dont care about it
                while ((line = reader.ReadLine()) != null)
                {
                    String[] arr = line.Split('|');  // key | value
                    dictionary.Add(int.Parse(arr[0]), int.Parse(arr[1])); // convert to int and add to dictionary
                    textBox4.Text += arr[0] +"\t" + arr[1]+"\r\n";
                }
                reader.Close();
                MessageBox.Show("Index file is loaded");
            }
            

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)//insert
        {
            //insert to test file
            fileStream.Seek(0, SeekOrigin.End);
            int position = Convert.ToInt32(fileStream.Position);
            streamWriter.WriteLine(textBox1.Text + '|' + textBox2.Text + '|' + textBox3.Text);
            streamWriter.Flush();
            dictionary.Add(Convert.ToInt32(textBox1.Text), position);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            MessageBox.Show("Inserted");

        }

        private void button6_Click(object sender, EventArgs e)//rewite ... save dictionary to index file
        {
            StreamWriter writer = new StreamWriter("index.txt");
            foreach(KeyValuePair<int,int> item in dictionary) 
            {
                writer.WriteLine(item.Key + "|" + item.Value);
                writer.Flush();

            }
            writer.Close();
            MessageBox.Show("Done");

        }
        public bool binarySearch(int[] arr, int item) // take array and the item you need to found it 
        {
            int f =0 ,l = arr.Length-1, mid;
            while (f <= l)
            {
                mid = (f + l) / 2;
                if (item < arr[mid])
                {
                    l = mid - 1;
                }
                else if (item > arr[mid]) 
                {
                    f = mid + 1;
                }
                else { return true; }



            }
           return false;
        }
        

        private void button4_Click(object sender, EventArgs e)//search
        {
            streamReader.DiscardBufferedData();
            int[] arr = dictionary.Keys.ToArray();
            if (binarySearch(arr, Convert.ToInt32(textBox1.Text))) 
            {
                int location = dictionary[Convert.ToInt32(textBox1.Text)];
                fileStream.Seek(location, SeekOrigin.Begin);
                String? line = streamReader .ReadLine();
                String[] arr2 = line.Split("|");
                textBox2.Text = arr2[1];
                textBox3.Text = arr2[2];
                return;

            }
            textBox2.Clear();
            textBox3.Clear();
            MessageBox.Show("not found");

        }

        private void button5_Click(object sender, EventArgs e)//delete
        {
            streamReader.DiscardBufferedData();
            int[] arr = dictionary.Keys.ToArray();
            if (binarySearch(arr, Convert.ToInt32(textBox1.Text)))
            {
                int location = dictionary[Convert.ToInt32(textBox1.Text)];
                fileStream.Seek(location, SeekOrigin.Begin);
                streamWriter.Write("*");
                streamWriter.Flush();
                dictionary.Remove(Convert.ToInt32(textBox1.Text));
                MessageBox.Show("done");
                return;


            }
            
            MessageBox.Show("not found");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            streamWriter.Close();
            streamReader.Close();
            fileStream.Close();
            Dispose();
        }
    }
}