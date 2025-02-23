namespace FeeSystem.Students.Dto;

public class UpdateStudentFeeDto
{
    public string StudentId { get; set; }
    public string FeeHeading { get; set; }
    public double AdditionalFee { get; set; }
    public double Discount { get; set; }
}