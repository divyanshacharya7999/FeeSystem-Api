using System;

namespace FeeSystem.Students.Dto;

public class UpdateStudentDto
{
    public string StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
}