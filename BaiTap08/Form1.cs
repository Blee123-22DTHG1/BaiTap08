using BaiTap08.Modell1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap08
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (var context = new SchoolDBEntities())
            {
                dgvStudents.DataSource = context.Students.ToList();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var context = new SchoolDBEntities())
            {
                var student = new Student
                {
                    FullName = txtFullName.Text,
                    Age = Convert.ToInt32(txtAge.Text),
                    Major = cboMajor.Text                           
                };
                context.Students.Add(student);
                context.SaveChanges();
                LoadData();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null)
            {
                int studentId = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentId"].Value);
                using (var context = new SchoolDBEntities())
                {
                    var student = context.Students.Find(studentId);
                    if (student != null)
                    {
                        student.FullName = txtFullName.Text;
                        student.Age = Convert.ToInt32(txtAge.Text);
                        student.Major = cboMajor.Text;
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure the StudentId column exists
                if (dgvStudents.Columns.Contains("StudentId"))
                {
                    // Ensure a row is selected
                    if (dgvStudents.SelectedRows.Count > 0)
                    {
                        // Confirm deletion with the user
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this student?",
                                                              "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            // Get the StudentId of the selected row
                            int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["StudentId"].Value);

                            using (var context = new SchoolDBEntities())
                            {
                                var student = context.Students.Find(studentId);
                                if (student != null)
                                {
                                    // Remove the student and save changes
                                    context.Students.Remove(student);
                                    context.SaveChanges();

                                    // Reload the data to reflect changes
                                    LoadData();

                                    MessageBox.Show("Student deleted successfully.");
                                }
                                else
                                {
                                    MessageBox.Show("Student not found in the database.");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a student to delete.");
                    }
                }
                else
                {
                    MessageBox.Show("Column 'StudentId' does not exist in the DataGridView.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid StudentId format.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStudents.CurrentRow != null)
            {
                txtFullName.Text = dgvStudents.CurrentRow.Cells["FullName"].Value.ToString();
                txtAge.Text = dgvStudents.CurrentRow.Cells["Age"].Value.ToString();
                cboMajor.Text = dgvStudents.CurrentRow.Cells["Major"].Value.ToString();
            }
        }
    }
}
