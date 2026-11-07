
using MySqlConnector;
using StudentManagement.Models;

namespace StudentManagement.Repository;

public class StudentRepository
{
    private readonly string _connectionString;

    public StudentRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("StudentDb")
            ?? throw new Exception("Chưa cấu hình database");
    }

    // Lấy danh sách sinh viên
    public List<Student> GetAll()
    {
        var students = new List<Student>();

        using var connection =
            new MySqlConnection(_connectionString);

        using var command = new MySqlCommand(
            @"SELECT StudentId, StudentCode, FullName,
                     DateOfBirth, Email, Phone
              FROM Student
              ORDER BY StudentId DESC",
            connection);

        connection.Open();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            students.Add(ReadStudent(reader));
        }

        return students;
    }

    // Tìm sinh viên theo ID
    public Student? GetById(int id)
    {
        using var connection =
            new MySqlConnection(_connectionString);

        using var command = new MySqlCommand(
            @"SELECT StudentId, StudentCode, FullName,
                     DateOfBirth, Email, Phone
              FROM Student
              WHERE StudentId = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return ReadStudent(reader);
        }

        return null;
    }

    // Thêm sinh viên
    public void Add(Student student)
    {
        using var connection =
            new MySqlConnection(_connectionString);

        using var command = new MySqlCommand(
            @"INSERT INTO Student
              (StudentCode, FullName, DateOfBirth, Email, Phone)
              VALUES
              (@code, @name, @dob, @email, @phone)",
            connection);

        AddParameters(command, student);

        connection.Open();
        command.ExecuteNonQuery();
    }

    // Sửa sinh viên
    public bool Update(Student student)
    {
        using var connection =
            new MySqlConnection(_connectionString);

        using var command = new MySqlCommand(
            @"UPDATE Student
              SET StudentCode = @code,
                  FullName = @name,
                  DateOfBirth = @dob,
                  Email = @email,
                  Phone = @phone
              WHERE StudentId = @id",
            connection);

        AddParameters(command, student);

        command.Parameters.AddWithValue(
            "@id", student.StudentId);

        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    // Xóa sinh viên
    public bool Delete(int id)
    {
        using var connection =
            new MySqlConnection(_connectionString);

        using var command = new MySqlCommand(
            "DELETE FROM Student WHERE StudentId = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    // Đọc một sinh viên từ database
    private static Student ReadStudent(MySqlDataReader reader)
    {
        return new Student
        {
            StudentId = reader.GetInt32(0),
            StudentCode = reader.GetString(1),
            FullName = reader.GetString(2),
            DateOfBirth = reader.GetDateTime(3),
            Email = reader.GetString(4),
            Phone = reader.GetString(5)
        };
    }

    // Gán tham số cho câu lệnh SQL
    private static void AddParameters(
        MySqlCommand command, Student student)
    {
        command.Parameters.AddWithValue(
            "@code", student.StudentCode);

        command.Parameters.AddWithValue(
            "@name", student.FullName);

        command.Parameters.AddWithValue(
            "@dob", student.DateOfBirth!.Value);

        command.Parameters.AddWithValue(
            "@email", student.Email);

        command.Parameters.AddWithValue(
            "@phone", student.Phone);
    }
}
