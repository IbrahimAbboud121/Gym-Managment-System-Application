using GymManagment.Controller;
using GymManagment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;


namespace GymManagment
{


    public partial class TrainersMenu : Form
    {
        TrainersModel trainer = new TrainersModel("", "", "", "");
        TrainersController controller;
        Dictionary<string, string> empty = [];



        public TrainersMenu()
        {
            InitializeComponent();
            controller = new TrainersController();
            trainer = new TrainersModel("", "", "", "");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)//for add button
        {

            trainer = new TrainersModel(IdBox.Text, NameBox.Text, SpecializationBox.Text, AssignedClassesBox.Text);
            empty.Add("Trainer's Id", trainer.TrainerId);
            empty.Add("Name", trainer.Name);
            empty.Add("Specialization", trainer.Specialization);
            empty.Add("Assigned Classes", trainer.AssignedClasses);
            foreach (KeyValuePair<string, string> kvp in empty)
            {
                if (kvp.Value == "")
                {
                    MessageBox.Show($"{kvp.Key} cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    empty = [];
                    break;
                }
            }


            string path = controller.GetFilePath();
            if (!File.Exists(path))
                File.Create(path).Close();
            string[] lines = File.ReadAllLines(path);


            if ((trainer.TrainerId != "" && trainer.Name != "") && (trainer.Specialization != "" && trainer.AssignedClasses != ""))
            {
                using (StreamWriter Trainers = new StreamWriter(path, true))
                {
                    bool idExists = lines.Any(line => line.StartsWith($"Id:{trainer.TrainerId}")); //  Check if any line starts with the member ID followed by a comma
                    if (idExists)
                    {
                        MessageBox.Show("trainer already exists!", "Failed",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        empty = [];
                        Trainers.WriteLine(trainer.ToString());
                        MessageBox.Show("Trainer is added successfully!", "Success",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                        controller.Reset(IdBox, NameBox, SpecializationBox, AssignedClassesBox);
                        
                    }
                }
                //adding the trainer to the sessiontrainer file
                string STpath = Path.Combine(@"C:\Users\USER\source\repos\GymManagment", "SessionTrainer.txt");
                using (StreamWriter sessiontrainer = new StreamWriter(STpath, true))
                {
                    sessiontrainer.WriteLine($"{trainer.Name}");
     
                }


            }
        }





        private void button1_Click(object sender, EventArgs e)//Reset button
        {
            controller.Reset(IdBox, NameBox, SpecializationBox, AssignedClassesBox);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            controller.RemoveTrainer(controller.GetFilePath(),trainer.TrainerId);
            MessageBox.Show("Trainer removed successfully!",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main = new Main();
            main.Show();
        }
    }
}
