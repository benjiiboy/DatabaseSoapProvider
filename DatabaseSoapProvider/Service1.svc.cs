using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DatabaseSoapProvider
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string ConnectionString = "Server=tcp:server1233.database.windows.net,1433;Initial Catalog=StudentDatabase;Persist Security Info=False;User ID=benjamin;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int AddStudent(string name, int id)
        {
            const string insertStudent = "insert into students (name, id) values (@name, @id)";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@name", name);
                    insertCommand.Parameters.AddWithValue("@id", id);
                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        //Testcommit

        public IList<Student> GetAllStudents()
        {
            const string selectAllStudents = "select * from students order by name";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectAllStudents, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        List<Student> studentList = new List<Student>();
                        while (reader.Read())
                        {
                            Student student = ReadStudent(reader);
                            studentList.Add(student);
                        }
                        return studentList;
                    }
                }
            }
        }


        private static Student ReadStudent(IDataRecord reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            Student student = new Student
            {
                Id = id,
                Name = name,
            };
            return student;
        }



        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Student GetStudentById(int id)
        {
            const string selectStudent = "select * from students where id=@id";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStudent, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        reader.Read(); // Advance cursor to first row
                        Student student = ReadStudent(reader);
                        return student;
                    }
                }
            }
        }

        public IList<Student> GetStudentsByName(string name)
        {
            string selectStr = "select * from students where name LIKE @name";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                databaseConnection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStr, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@name", "%" + name + "%");
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        IList<Student> studentList = new List<Student>();
                        while (reader.Read())
                        {
                            Student st = ReadStudent(reader);
                            studentList.Add(st);
                        }
                        return studentList;
                    }
                }
            }
        }
    }
}
