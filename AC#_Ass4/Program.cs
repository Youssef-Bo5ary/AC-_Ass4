namespace AC__Ass4
{

    public enum LayOffCause
    {
        NegativeVacationStock,
        AgeGreaterThanSixty
    }

    public class EmployeeLayOffEventArgs : EventArgs
    {
        public LayOffCause Cause { get; set; }
    }

    public class Employee
    {
        public event EventHandler<EmployeeLayOffEventArgs> EmployeeLayOff;

        public int EmployeeID { get; set; }
        public DateTime BirthDate { get; set; }
        public int VacationStock { get; set; }

        public bool RequestVacation(DateTime From, DateTime To)
        {
            throw new NotImplementedException();
        }

        public void EndOfYearOperation()
        {
            // Check if the VacationStock is negative or the age is greater than 60
            if (VacationStock < 0 || CalculateAge() > 60)
            {
                OnEmployeeLayOff(new EmployeeLayOffEventArgs { Cause = VacationStock < 0 ? LayOffCause.NegativeVacationStock : LayOffCause.AgeGreaterThanSixty });
            }
        }

        protected virtual void OnEmployeeLayOff(EmployeeLayOffEventArgs e)
        {
            EmployeeLayOff?.Invoke(this, e);
        }

        private int CalculateAge()
        {
            // Calculate age based on the BirthDate
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
    public class Department
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        List<Employee> Staff;

        public Department()
        {
            Staff = new List<Employee>();
        }

        public void AddStaff(Employee E)
        {
            Staff.Add(E);
            E.EmployeeLayOff += RemoveStaff;
        }

        public void RemoveStaff(object sender, EmployeeLayOffEventArgs e)
        {
            Employee employeeToRemove = (Employee)sender;
            Staff.Remove(employeeToRemove);
            Console.WriteLine($"Employee {employeeToRemove.EmployeeID} removed from {DeptName} department due to {e.Cause}.");
        }
    }

    public class Club
    {
        public int ClubID { get; set; }
        public string ClubName { get; set; }
        List<Employee> Members;

        public Club()
        {
            Members = new List<Employee>();
        }

        public void AddMember(Employee E)
        {
            Members.Add(E);
            E.EmployeeLayOff += RemoveMember;
        }

        public void RemoveMember(object sender, EmployeeLayOffEventArgs e)
        {
            Employee employeeToRemove = (Employee)sender;

            if (e.Cause == LayOffCause.NegativeVacationStock)
            {
                Members.Remove(employeeToRemove);
                Console.WriteLine($"Employee {employeeToRemove.EmployeeID} removed from {ClubName} club due to negative vacation stock.");
            }
        }
    }
    public class SalesPerson : Employee
    {
        public int AchievedTarget { get; set; }

        public bool CheckTarget(int Quota)
        {
            if (AchievedTarget >= Quota)
            {
                return true;
            }
            else
            {
                OnEmployeeLayOff(new EmployeeLayOffEventArgs { Cause = LayOffCause.FailedSalesTarget });
                return false;
            }
        }
    }

    public class BoardMember : Employee
    {
        public void Resign()
        {
            OnEmployeeLayOff(new EmployeeLayOffEventArgs { Cause = LayOffCause.Resigned });
        }
    }


    internal class Program
        {
            static void Main(string[] args)
            {
            Club companyClub = new Club
            {
                ClubID = 1,
                ClubName = "Company Social Club"
            };

            Employee employee1 = new Employee
            {
                EmployeeID = 1,
                BirthDate = new DateTime(1950, 5, 10),
                VacationStock = -2
            };

            Employee employee2 = new Employee
            {
                EmployeeID = 2,
                BirthDate = new DateTime(1965, 4, 20),
                VacationStock = 5
            };

            companyClub.AddMember(employee1);
            companyClub.AddMember(employee2);

            employee1.EndOfYearOperation(); // This will trigger removal of employee1 from the club
            employee2.EndOfYearOperation(); // This will not trigger removal since conditions are not met

            Console.WriteLine($"Remaining members in {companyClub.ClubName}:");
            foreach (var member in companyClub.Members)
            {
                Console.WriteLine($"Employee ID: {member.EmployeeID}");
            }
        }
    }
        }
    

