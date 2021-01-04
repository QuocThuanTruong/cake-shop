using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeShop.Converter;

namespace CakeShop.Utilities
{
    class ApplicationUtilities
    {
        private ApplicationUtilities() { }

        private static ApplicationUtilities _applicationInstance;
        private AbsolutePathConverter _absolutePathConverter = new AbsolutePathConverter();

       
        public static ApplicationUtilities GetAppInstance()
        {
            if (_applicationInstance == null)
            {
                _applicationInstance = new ApplicationUtilities();
            }
            else
            {
                //Do Nothing
            }

            return _applicationInstance;
        }

        public string getStandardName(string name, int maxLength)
        {
            var result = name;

            if (result.Length > maxLength)
            {
                result = result.Substring(0, maxLength - 3);
                result += "...";
            }

            return result;
        }


        public void createIDDirectory(int ID)
        {
            string path = (string)(_absolutePathConverter.Convert($"Images/{ID}", null, null, null));

            if (Directory.Exists(path))
            {
                //Do Nothing
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        public void copyImageToIDDirectory(int ID, string srcPath, string nameFile)
        {
            var destPath = (string)_absolutePathConverter.Convert($"Images\\{ID}\\{nameFile}.{getTypeOfImage(srcPath)}", null, null, null);
            
            try
            {
                File.Copy(srcPath, destPath, true);
            }
            catch (Exception e)
            {

            }
        }

        public string getTypeOfImage(string uriImage)
        {
            string result = "";

            int index = uriImage.Length - 1;

            while (uriImage[index] != '.')
            {
                result = uriImage[index--] + result;
            }

            return result;
        }

        public string GetMoneyForBinding(int money)
        {
            string result = string.Format("{0:n0}", money);

            result += " VNĐ";

            return result;
        }
    }
}
