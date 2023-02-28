using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CarsDatabase
{
    public partial class frmCars : Form
    {
        public static List<tblCar> carList = new List<tblCar>();

        public static int counterCar = 1;
        public static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:/Users/Anthony/source/repos/CarsDatabase/Resources/Hire.mdb;Persist Security Info=False;";

        static OleDbConnection oleDbConnection;
        static OleDbCommand cmd;



        public frmCars()
        {
            InitializeComponent();
            ToolTip();
            DbConnection();
            if (carList.Count != 0)
            {
                txtCount.Text = $"1 of {carList.Count}";
                tblCar tblCar = carList[0];
                Mapper(tblCar);
            }
            else
            {
                Clear();
            }
        }


       
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (carList.Count != 0)
            {
                txtCount.Text = $"1 of {carList.Count}";
                tblCar tblCar = carList[0];
                Mapper(tblCar);
            }
            else
            {
                Clear();
            }
            counterCar = 1;

            if (carList.Count != 0)
            {
                txtCount.Text = $"1 of {carList.Count}";
                tblCar tblCar = carList[0];
                Mapper(tblCar);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (carList.Count != 0)
            {
                txtCount.Text = $"{carList.Count} of {carList.Count}";
                tblCar tblCar = carList[carList.Count - 1];
                counterCar = carList.Count;
                Mapper(tblCar);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (carList.Count != 0 && counterCar <= carList.Count - 1)
            {
                counterCar++;
                txtCount.Text = $"{counterCar} of {carList.Count}";
                tblCar tblCar = carList[counterCar - 1];
                Mapper(tblCar);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (carList.Count != 0 && counterCar >= 2)
            {
                counterCar--;
                txtCount.Text = $"{counterCar} of {carList.Count}";
                tblCar tblCar = carList[counterCar - 1];
                Mapper(tblCar);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (carList.Count == 0)
                return;
            oleDbConnection = new OleDbConnection(connectionString);
            oleDbConnection.Open();
            cmd = new OleDbCommand("delete from tblCar where VehicleRegNo = @VehicleRegNo", oleDbConnection);
            try
            {
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted...");
                oleDbConnection.Close();
                DbConnection();
                btnFirst.PerformClick();

            }
            catch (Exception x)
            {
                MessageBox.Show(" Not Deleted " + x.Message);
                oleDbConnection.Close();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            oleDbConnection = new OleDbConnection(connectionString);
            oleDbConnection.Open();
            cmd = new OleDbCommand("Insert into tblCar (VehicleRegNo,Make,EngineSize,DateRegistered,RentalPerDay,Available) Values(@VehicleRegNo,@Make,@EngineSize,@DateRegistered,@RentalPerDay,@Available)", oleDbConnection);
            try
            {
                string rentalPerDay = txtRentalPerDay.Text;
                rentalPerDay = rentalPerDay.Length == 0 ? "0.00" : rentalPerDay;
                rentalPerDay = rentalPerDay[0] == '£' ? rentalPerDay.Substring(1) : rentalPerDay;
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);
                cmd.Parameters.AddWithValue("@Make", txtMake.Text);
                cmd.Parameters.AddWithValue("@EngineSize", txtEngineSize.Text);
                cmd.Parameters.AddWithValue("@DateRegistered", txtDateRegistered.Text);
                cmd.Parameters.AddWithValue("@RentalPerDay", rentalPerDay);
                cmd.Parameters.AddWithValue("@Available", checkBoxAvailable.Checked);

                cmd.ExecuteNonQuery();
                MessageBox.Show("  Added ");
                oleDbConnection.Close();
                DbConnection();
                btnFirst.PerformClick();
            }
            catch (Exception x)
            {
                MessageBox.Show(" Not Added " + x.Message);
                oleDbConnection.Close();
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form searchForm = new frmSearch();
            searchForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            oleDbConnection = new OleDbConnection(connectionString);
            oleDbConnection.Open();
            cmd = new OleDbCommand("Update  tblCar Set Make = @Make,EngineSize = @EngineSize,DateRegistered = @DateRegistered,RentalPerDay = @RentalPerDay,Available =@Available Where VehicleRegNo = @VehicleRegNo", oleDbConnection);
            try
            {
                string rentalPerDay = txtRentalPerDay.Text;
                rentalPerDay = rentalPerDay.Length == 0 ? "0.00" : rentalPerDay;
                rentalPerDay = rentalPerDay[0] == '£' ? rentalPerDay.Substring(1) : rentalPerDay;


                cmd.Parameters.AddWithValue("@Make", txtMake.Text);
                cmd.Parameters.AddWithValue("@EngineSize", txtEngineSize.Text);
                cmd.Parameters.AddWithValue("@DateRegistered", txtDateRegistered.Text);
                cmd.Parameters.AddWithValue("@RentalPerDay", rentalPerDay);
                cmd.Parameters.AddWithValue("@Available", checkBoxAvailable.Checked);
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtVehicleRegNo.Text);
                var result = cmd.ExecuteNonQuery();
                MessageBox.Show("Updated...");
                oleDbConnection.Close();
                DbConnection();

            }
            catch (Exception x)
            {
                MessageBox.Show(" Not Updated " + x.Message);
                oleDbConnection.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (carList.Count == 0)
                return;
            else
            {
                tblCar tblCar = carList[counterCar - 1];
                Mapper(tblCar);
            }
        }


        private void ToolTip()
        {
            new ToolTip().SetToolTip(lblVehicleRegNo, "Enter the Registration Number of Vehicle");
            new ToolTip().SetToolTip(lblMake, "Enter the Make of Vehicle");
            new ToolTip().SetToolTip(lblEngineSize, "Enter the Engine Size of Vehicle");
            new ToolTip().SetToolTip(lblDateRegistered, "Enter the Registration Date of Vehicle in format dd/mm/yyyy");
            new ToolTip().SetToolTip(lblRentalPerDay, "Enter the Rent of Vehicle per Day");
            new ToolTip().SetToolTip(lblAvailable, "Check whether the Vehicle is Available or Not");
        }

        private void Mapper(tblCar tblCar)
        {
            txtVehicleRegNo.Text = tblCar.VehicleRegNo;
            txtMake.Text = tblCar.Make;
            txtEngineSize.Text = tblCar.EngineSize;
            txtRentalPerDay.Text = $"£{tblCar.RentalPerDay:F2}";
            txtDateRegistered.Text = tblCar.DateRegistered.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            checkBoxAvailable.Checked = tblCar.Available;
        }

        private void Clear()
        {
            txtVehicleRegNo.Text = "";
            txtMake.Text = "";
            txtEngineSize.Text = "";
            txtRentalPerDay.Text = $"£0.00";
            txtDateRegistered.Text = "";
            checkBoxAvailable.Checked = false;
            txtCount.Text = "";

        }

        private static void DbConnection()
        {
            oleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand cmd = new OleDbCommand("Select * from tblCar", oleDbConnection);
            try
            {
                oleDbConnection.Open();

                carList = DataReaderMapToList<tblCar>(cmd.ExecuteReader());


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

        private void txtRentalPerDay_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmCars_Load(object sender, EventArgs e)
        {

        }
    }
}
