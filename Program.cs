using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;
using System.Windows.Forms;

namespace Mason
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            MessageBox.Show("Please select the CSV file containing the brick orders. Click OK to show file browser.", "Mason");
            string csvFilePath = Program.GetFilePathOfCSV();

            int brickEditionNumber = Program.ShowDialog(
                "Please enter a starting index for the brick edition numbers, e.g. 106",
                "Brick starting number"
              );

            List<BrickInscription> inscriptions = BrickInscription.ParseBrickInscriptionsFromCSV(csvFilePath, brickEditionNumber);
            BrickInscription.PrintInscriptionsToDXFFile(inscriptions);

            MessageBox.Show("Brick inscription generation complete. You can find the brick inscriptions in the following folder: " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\generatedDXF", "Mason");

            return 0;
        }

        public static string GetFilePathOfCSV()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select CSV file containing brick orders";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            else
            {
                throw new System.ArgumentException("File path to the CSV containing brick orders must be selected", "CSV");
            }
        }

        public static int ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 400 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                return Int32.Parse(textBox.Text);
            }
            else
            {
                throw new System.ArgumentException("Argument must be specified and must be a number", caption);
            }
        }
    }
}