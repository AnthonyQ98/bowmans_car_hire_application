using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
namespace CarsDatabase
{
    public partial class frmSearch : Form
    {
        static OleDbConnection oleDbConnection;
        static OleDbCommand cmd;
        public static List<tblCar> carData = new List<tblCar>();
        public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/Users/Anthony/source/repos/CarsDatabase/Resources/Hire.mdb;Persist Security Info=False;";
        public static Dictionary<string, string> searchValues = new Dictionary<string, string>();
        public frmSearch()
        {
            InitializeComponent();
           // DbConnection();
           // dataGridView1.DataSource = carData;
        }

      
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form  frmCar = new frmCars();
            frmCar.ShowDialog();

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string item = $"";
            if(cboField.SelectedItem.ToString() == "Available" )
            {
                item += " =";
                item += txtValue.Text.ToLower() == "yes" ? " true" : " false";

            }
            else if(cboField.SelectedItem.ToString() == "RentalPerDay")
            {
                item += $" {cboOperator.SelectedItem} {txtValue.Text}";
              
            }
            else
            {
                item += $" {cboOperator.SelectedItem} '{txtValue.Text}'";
            }
            if(searchValues.Count == 0)
            {
                searchValues.Add(cboField.SelectedItem.ToString(), $"{item}");
                MessageBox.Show("Select two More Values");
                return;
            }
            if(searchValues.ContainsKey(cboField.SelectedItem.ToString()))
            {
                MessageBox.Show($"{cboField.SelectedItem} Value is Already Selected ");
                return;
            }
             if(searchValues.Count < 2)
            {

                searchValues.Add(cboField.SelectedItem.ToString(), $"{item}");
                MessageBox.Show("Select One More Value");
            }
            else
            {
                searchValues.Add(cboField.SelectedItem.ToString(), $"{item}");

                oleDbConnection = new OleDbConnection(connectionString);

                StringBuilder sb = new StringBuilder();
                int counter = 0;
                foreach(var key in searchValues)
                {
                    sb.Append(key.Key);
                    sb.Append(key.Value);
                    if (counter != 2)
                        sb.Append(" AND ");
                        counter++;
                }
                searchValues.Clear();
                cmd = new OleDbCommand($"Select * from tblCar where {sb}", oleDbConnection);
                try
                {
                    oleDbConnection.Open();
                    carData = DataReaderMapToList<tblCar>(cmd.ExecuteReader());
                    dataGridView1.DataSource = carData;
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error is " + x.Message);
                }
                finally
                {
                    oleDbConnection.Close();
                }

            }
        }

        private static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            cboField.Focus();
            cboField.DataSource = new List<string> {
                "Make", "EngineSize", "RentalPerDay","Available"
            };
            cboField.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOperator.DataSource = new List<string>
            {
                 "=", "<", ">", "<=", ">="
            };
            cboOperator.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
