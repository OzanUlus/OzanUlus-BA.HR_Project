﻿namespace BA.HR_Project.WEB.Models
{
    public class AppUserUpdateForAdminVM
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public string SecondSurname { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsTurkishCitizen { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public int Salary { get; set; }
    }
}
