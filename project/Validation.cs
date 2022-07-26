using System.Text.RegularExpressions;

namespace HyperMarket
{
    internal class Validation
    {
        // validate cashier Username if null and not exist in our list
        public bool IsValidUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;
            foreach (Cashier ca in Market.market.Cashiers)
            {
                if (ca.Username == userName)
                    return false;
            }
            return true;
        }
        // check is not null and 11 digit
        public bool IsValidPhone(string Phone)
        {
            if (string.IsNullOrEmpty(Phone))
                return false;
            Regex r = new Regex(@"^01[0-9]{9}$");
            return r.IsMatch(Phone);
        }
        //check if salary > 1000 and not null
        public bool IsValidSalary(string Salary)
        {
            if (string.IsNullOrEmpty(Salary))
                return false;
            Regex r = new Regex(@"^([1-9])[0-9]{3}$");
            return r.IsMatch(Salary);
        }
        public bool IsName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            Regex r = new Regex(@"^[A-Z][a-z]{4,} [A-Z][a-z]{4,}$");
            return r.IsMatch(Name);
        }
        public bool IsValidPrice(string Salary)
        {
            if (string.IsNullOrEmpty(Salary))
                return false;
            Regex r = new Regex(@"^([1-9])[0-9]+$");
            return r.IsMatch(Salary);
        }
        //phone
        // text
        // money
        // number
        //userName

    }
}