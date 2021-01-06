using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.Global
{
    class Global
    {

        public static List<Cake> cakesOrder = new List<Cake>();
        public static int totalCost = 0;
        public static void calcTotalCost()
        {
            totalCost = 0;
            foreach (var cake in cakesOrder)
            {
                if (cake.isActive == 1)
                {
                    totalCost += cake.SELLING_PRICE_INT_FOR_BINDING * cake.Order_Quantity;
                }
            }
        }

        public static void clearGlobal()
        {
            cakesOrder.Clear();
            totalCost = 0;
        }
    }
}
