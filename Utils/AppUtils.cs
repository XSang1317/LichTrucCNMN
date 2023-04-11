using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LichTruc.Utils
{
    public class AppUtils
    {
        public static string GetUniqueName(string name, string folderPath)
        {
            string validatedName = name;
            int tries = 1;
            string filePath = Path.Combine(folderPath, validatedName);
            while (File.Exists(filePath))
            {
                string fileName = Path.GetFileNameWithoutExtension(name);
                string extendsion = Path.GetExtension(name);
                validatedName = string.Format("{0}_{1}{2}", fileName, tries++, extendsion);
                filePath = Path.Combine(folderPath, validatedName);
            }
            return validatedName;
        }


        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static int Tinhngayphepnam(DateTime ngayvaonganh, int year = 0)
        {
            if (year == 0) year = DateTime.Now.Year;
            int sothanglamviec = 0;
            int ngayphep = 0;
            //DateTime ngaycuoinam = new DateTime(DateTime.Now.Year, 12, 31);
            DateTime ngaycuoinam = new DateTime(year, 12, 31);
            if ((ngaycuoinam.Day - ngayvaonganh.Day) >= 15) sothanglamviec++;
            sothanglamviec += ((ngaycuoinam.Year - ngayvaonganh.Year) * 12) + ngaycuoinam.Month - ngayvaonganh.Month;
            if (sothanglamviec > 12)
            {
                ngayphep = 12 + (sothanglamviec / 60);
            }
            else
            {
                ngayphep = sothanglamviec;
            }

            //Console.WriteLine("So ngay phep: " + ngayphep);
            return ngayphep;
        }

        public static int Tinhtongngayphep(DateTime ngayvaonganh)
        {
            int year = DateTime.Now.Year;
            int sothanglamviec = 0;
            int ngayphep = 0;
            DateTime ngaycuoinam = new DateTime(year, 12, 31);
            if ((ngaycuoinam.Day - ngayvaonganh.Day) >= 15) sothanglamviec++;
            sothanglamviec += ((ngaycuoinam.Year - ngayvaonganh.Year) * 12) + ngaycuoinam.Month - ngayvaonganh.Month;
            int sothanglamviectmp = sothanglamviec;

            while (sothanglamviectmp >= 12)
            {
                ngayphep += 12 + (sothanglamviectmp / 60);
                sothanglamviectmp -= 12;
            }
            ngayphep += sothanglamviectmp;

            Console.WriteLine("Tong so ngay phep: " + ngayphep);
            return ngayphep;
        }
    }
}
