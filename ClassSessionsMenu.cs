using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManagment.Model;
using GymManagment.Controller;

namespace GymManagment
{
    public partial class ClassSessionsMenu : Form
    {
        ClassSessionsModel session;
        Dictionary<string, string> empty = [];
        TrainersController TrainerController;
        ClassSessionsController Sessioncontroller;
        MembersController MemberController;

        public ClassSessionsMenu()
        {
            InitializeComponent();
            session = new ClassSessionsModel("", "", null, "", "", "");
            Sessioncontroller = new ClassSessionsController();
            TrainerController = new TrainersController();
            MemberController = new MembersController();





        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)//for Add button
        {
            empty.Clear();
            session = new ClassSessionsModel(IdBox.Text, NameBox.Text, null, DateTimeBox.Value.ToString(), "20", null);
            empty.Add("Id", IdBox.Text);
            empty.Add("Name", NameBox.Text);
            empty.Add("ScheduleTime", DateTimeBox.Value.ToString());
            foreach (KeyValuePair<string, string> kvp in empty)
            {
                if (kvp.Value == "")
                {
                    MessageBox.Show($"{kvp.Key} cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    empty.Clear();
                    return;
                }
            }

            string ClassSessionPath = Sessioncontroller.GetFilePath();
            if (!File.Exists(ClassSessionPath))
                File.Create(ClassSessionPath).Close();
            if ((session.SessionId != "" && session.Name != "") && session.ScheduleTime != "")
            {
                using (StreamWriter SESSIONS = new StreamWriter(ClassSessionPath, true))
                {
                    SESSIONS.WriteLine(session.ToString());
                    MessageBox.Show("Session Succesfully Added", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    string path = Path.Combine(@"C:\Users\USER\source\repos\GymManagment", "Session.txt");
                    using (StreamWriter sessiontrainer = new StreamWriter(path, true))
                    {
                        sessiontrainer.WriteLine($"{session.Name}");
                    }
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main main = new Main();
            main.Show();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            
            if (DataGridViewMembers.Rows.Count == 0 || DataGridViewTrainers.Rows.Count==0 )
            {
                DataGridViewMembers.Rows.Clear();
                DataGridViewTrainers.Rows.Clear();
                //Displaying the value on the screen for accepted members once its empty
                string AcceptedMembersFilePath = @"C:\Users\USER\source\repos\GymManagment";
                string AcceptedMembersPath = Path.Combine(AcceptedMembersFilePath, "MembersAccepted.txt");

                if(File.Exists(AcceptedMembersPath))
                { 
                List<string> lines = File.ReadAllLines(AcceptedMembersPath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                  
                        string[] line = lines[i].Split(",");
                        DataGridViewMembers.Rows.Add(
                                   line[0], //0 for name 1 for session
                                  line[1]);
                }
                }


                string SESSIONS = Path.Combine(@"C:\Users\USER\source\repos\GymManagment", "Session.txt");
                string TRAINERS = Path.Combine(@"C:\Users\USER\source\repos\GymManagment", "SessionTrainer.txt");
                
                    
                    int maxslots = 20;
                if (File.Exists(SESSIONS))
                {
                    
                    List<string> Sess = File.ReadAllLines(SESSIONS).ToList();
                    List<string> trai = new List<string>();
                    if (File.Exists(TRAINERS))
                    {
                        trai = File.ReadAllLines(TRAINERS).ToList();
                    }
                    

                    while (Sess.Count() != trai.Count())
                        {
                            trai.Add("");
                        }
                    for (int k = 0; k < Sess.Count; k++)
                    {
                        
                        if (File.Exists(AcceptedMembersPath))
                        {
                            List<string> acceptedmembers = File.ReadAllLines(AcceptedMembersPath).ToList();

                            for (int j = 0; j < acceptedmembers.Count; j++)
                            {
                                string[] members = acceptedmembers[j].Split(",");
                                if (members[1].ToLower() == Sess[k].ToLower())
                                    maxslots--;

                            }
                        }

                        // Add a new row with values in the CORRECT ORDER
                        DataGridViewTrainers.Rows.Add(
                             trai[k],
                            Sess[k],
                            DateTimeBox.Value.ToString(),
                           maxslots
                        );
                        maxslots = 20;
                    }
                    

                }

            }

        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddOldMember_Click(object sender, EventArgs e)
        {
            
            ClassSessionsController SessCon = new ClassSessionsController();
            if (addMembers.Text != "")
            {
                string[] OldMem = SessCon.IsUserLegal(addMembers.Text);
                if (OldMem != null)
                {

                    if (!OldMem[1].Equals("active", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("This member is Inactive", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }

                    else
                    {
                        bool isaccepted = false;
                        List<string> lines = File.ReadAllLines(Sessioncontroller.GetFilePath()).ToList();
                        string[] MemberSessions = OldMem[2].Split(",");
                        for (int i = 0; i < lines.Count; i++)
                        {
                            string[] line = lines[i].Split(':', ',');
                            for (int j = 0; j < MemberSessions.Length; j++)
                            {
                                if (line[1].Trim().ToLower() == MemberSessions[j].Trim().ToLower())
                                {
                                    string AcceptedMembersFilePath = @"C:\Users\USER\source\repos\GymManagment";
                                    string AcceptedMembersPath = Path.Combine(AcceptedMembersFilePath, "MembersAccepted.txt");
                                    if (!File.Exists(AcceptedMembersPath))
                                        File.Create(AcceptedMembersPath).Close();
                                    using(StreamWriter accepted=new StreamWriter(AcceptedMembersPath,true))
                                    {
                                        accepted.WriteLine($"{addMembers.Text},{line[1].ToLower()}");
                                    }
                                    isaccepted = true;
                                    MessageBox.Show("Member succesfully Added   ", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    DataGridViewMembers.Rows.Add(
                                          addMembers.Text, MemberSessions[j]
                                         );
                                    

                                }
                            }
                            
                        }
                        if(!isaccepted)
                        MessageBox.Show("Member is not enrolled in in any classe", "error", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    
                }
                else
                    MessageBox.Show("Member not Found", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("AddMember cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);




        }

        private void AddMembers_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
