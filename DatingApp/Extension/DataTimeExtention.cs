namespace WebAPIDatingAPP.Extension
{
    public static class DataTimeExtension
    {
        public  static int CalcuateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(age))age-- ;
            return age;

        }
    }
}