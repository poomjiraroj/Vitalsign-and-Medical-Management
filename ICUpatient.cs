﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TricuslabSoftware
{
    public partial class ICUpatient : Form
    {
        SqlConnection Conn;
        SqlCommand cmd;
        SqlDataAdapter Da;
        DataSet Ds;
        DataTable Dt;

        public ICUpatient()
        {
            InitializeComponent();
            String strConn = "Data Source=JICOMPUTER;Initial Catalog=Tricus_Data;Integrated Security=True";
            Conn = new SqlConnection(strConn);
            Query();
        }

        public void Query()
        {
            Conn.Open();
            String SQL = "Select Accept.Time, Accept.SID, Bed.BID, Accept.HN, PatientData.PatientFirstname + '  ' + dbo.PatientData.PatientLastname AS Patient, ";
            SQL += "PhysicianData.PhyFirstname + ' ' + dbo.PhysicianData.PhyLastname As Physician ";
            SQL += "From Accept Inner Join Bed On Accept.BID = dbo.Bed.BID ";
            SQL += "Inner Join PhysicianData On Accept.PID = PhysicianData.PID ";
            SQL += "Inner Join PatientData On Accept.HN = PatientData.HN ";
            SQL += "Inner Join sentPhysician on Accept.SID = sentPhysician.SID ";
            SQL += "Where WID = 'W1113' And Bed.Status = 1 and sentPhysician.Status = 'I' ";
            cmd = new SqlCommand(SQL, Conn);
            Da = new SqlDataAdapter(cmd);
            Ds = new DataSet();
            Dt = new DataTable();
            Da.Fill(Ds, "Patient");
            Dt = Ds.Tables["Patient"];
            gvICU.DataSource = Dt;
            gvICU.ReadOnly = true;
            gvICU.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gvICU.Columns[0].FillWeight = 90;
            gvICU.Columns[1].FillWeight = 40;
            gvICU.Columns[2].FillWeight = 35;
            gvICU.Columns[3].FillWeight = 40;
            Conn.Close();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvICU_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int curRow = gvICU.CurrentRow.Index;
            DashboardPatient dashPatient = new DashboardPatient(Dt.Rows[curRow]["HN"].ToString(), Dt.Rows[curRow]["SID"].ToString(), Dt.Rows[curRow]["BID"].ToString());
            try
            {
                dashPatient.Show();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
