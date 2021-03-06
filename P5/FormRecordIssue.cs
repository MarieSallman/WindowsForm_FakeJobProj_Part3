using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P5
{
    public partial class FormRecordIssue : Form
    {
        AppUser _CurrentAppUser;
        int _SelectedProjectId;
        FakeAppUserRepository userRepository = new FakeAppUserRepository();
        FakeIssueRepository issueRepository = new FakeIssueRepository();
        FakeIssueStatusRepository issueStatusRepository = new FakeIssueStatusRepository();
        public FormRecordIssue(AppUser appUser)
        {
            InitializeComponent();
            _CurrentAppUser = appUser;
        }

        private void FormRecordIssue_Load(object sender, EventArgs e)
        {
            dateTimediscovery.MaxDate = DateTime.Now;
            dateTimediscovery.Value = DateTime.Today;
            dateTimediscovery.Format = DateTimePickerFormat.Custom;
            dateTimediscovery.CustomFormat = "hh:mm:ss tt dd MMM yyyy";

            int newId = 1;

            foreach (AppUser appUser in userRepository.GetAll())
            {
                comboBoxdiscoverer.Items.Add(appUser.LastName + ", " + appUser.FirstName);
            }

            foreach (IssueStatus issueStatus in issueStatusRepository.GetAll())
            {
                comboBoxstatus.Items.Add(issueStatus.Value);
            }

            foreach (Issue issue in issueRepository.GetAll(_SelectedProjectId))
            {
                newId++;
            }
            this.CenterToScreen();

            textBoxid.Text = newId.ToString();
        }

        private void CreateIssueButton_Click(object sender, EventArgs e)
        {
            int newissueId = 1;
            foreach (Issue issueId in issueRepository.GetAll(_SelectedProjectId))
            {
                if(issueId.Id > newissueId)
                {
                    newissueId = issueId.Id;
                }               
            }
            newissueId++;

            Issue issue = new Issue();
            issue.Id = newissueId;
            issue.Title = textBoxtitle.Text.Trim();
            issue.DiscoveryDate = DateTime.Parse(dateTimediscovery.Text);
            issue.Discoverer = comboBoxdiscoverer.Text;
            issue.InitialDescription = textBoxdescription.Text;
            issue.Component = textBoxcomponent.Text;
            issue.IssueStatusId = issueStatusRepository.GetIdByStatus(comboBoxstatus.Text);
            FakePreferenceRepository preferenceRepository = new FakePreferenceRepository();
            _SelectedProjectId = Convert.ToInt32(preferenceRepository.GetPreference(_CurrentAppUser.UserName, FakePreferenceRepository.PREFERENCE_PROJECT_ID));
            issue.ProjectId = _SelectedProjectId;


            string result = issueRepository.Add(issue);
            if (result == FakeIssueRepository.NO_ERROR)
            {
                MessageBox.Show("Issue added successfully.");
            }
            else
            {
                MessageBox.Show("Issue not created. " + result, "Attention.");
            }
            this.Close();
        }
    }
}
