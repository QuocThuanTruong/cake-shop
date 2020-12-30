using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.Utilities
{
    class DatabaseUtilities
    {
        private DatabaseUtilities() { }

        private static DatabaseUtilities _databaseInstance;
        private static CakeShopEntities _databaseCakeShop;
        private ApplicationUtilities _applicationUtilities = ApplicationUtilities.GetAppInstance();

        public static DatabaseUtilities GetDatabaseInstance()
        {
            if (_databaseInstance == null)
            {
                _databaseInstance = new DatabaseUtilities();
                _databaseCakeShop = new CakeShopEntities();
            }
            else
            {
                //Do Nothing
            }

            return _databaseInstance;
        }

        public (List<Cake>, int) ExecQureyToGetCakes(string condition, (string column, string type) sortedBy, int currentPage, int totalCakePerPage)
        {
            (List<Cake> CakesResult, int totalCake) result;
            List<Cake> CakesResult = new List<Cake>();
            int totalCake = 0;

            string query = "";

            if (condition.Length > 0)
            {
                query = $"SELECT COUNT(ID_Cake) FROM [dbo].[Cake] WHERE {condition}";
            }
            else
            {
                query = $"SELECT COUNT(ID_Cake) FROM [dbo].[Cake]";
            }

            totalCake = _databaseCakeShop
                .Database
                .SqlQuery<int>(query)
                .Single();

            if (totalCake > 0)
            {
                query = query.Replace("COUNT(ID_Cake)", "*");
                query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalCakePerPage} ROWS FETCH NEXT {totalCakePerPage} ROWS ONLY";

                CakesResult = _databaseCakeShop
                .Database
                .SqlQuery<Cake>(query)
                .ToList();
            }

            for (int i = 0; i < CakesResult.Count; ++i)
            {
                CakesResult[i] = this.getCakeForBindingInListView(CakesResult[i]);
            }

            result.CakesResult = CakesResult;
            result.totalCake = totalCake;

            return result;
        }

        private Cake getCakeForBindingInListView(Cake cake)
        {
            Cake result = cake;

            result.NAME_FOR_BINDING = cake.Name_Cake;
            result.SELLING_PRICE_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(decimal.ToInt32(cake.Selling_Price ?? 0));

            return result;
        }
    }

   
}
