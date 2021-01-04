using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                CakesResult[i] = this.getCakeForBinding(CakesResult[i]);
            }

            result.CakesResult = CakesResult;
            result.totalCake = totalCake;

            return result;
        }

        public Cake getCakeById(int ID)
        {
            Cake result = _databaseCakeShop
                .Database
                .SqlQuery<GetInfCakeByID_Result>($"Select * from GetInfCakeByID({ID})")
                .FirstOrDefault();

            List<Cake_Image> cake_Images = _databaseCakeShop
                .Database
                .SqlQuery<Cake_Image>($"Select * From Cake_Image where ID_Cake = {ID} and Is_Active = 1")
                .ToList();

            result.CAKE_IMAGE_FOR_BINDING = cake_Images;
            result = getCakeForBinding(result);

            return result;
        }

        private Cake getCakeForBinding(Cake cake)
        {
            Cake result = cake;

            result.NAME_FOR_BINDING = cake.Name_Cake;
            result.SELLING_PRICE_INT_FOR_BINDING = decimal.ToInt32(cake.Selling_Price ?? 0);
            result.SELLING_PRICE_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(result.SELLING_PRICE_INT_FOR_BINDING);
            result.ORIGINAL_PRICE_INT_FOR_BINDING = decimal.ToInt32(cake.Original_Price ?? 0);
            result.ORIGINAL_PRICE_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(result.ORIGINAL_PRICE_INT_FOR_BINDING);
            result.Link_Avt = $"Images/{cake.ID_Cake}/avatar.{result.Link_Avt}";

            for (int i = 0; i < result.CAKE_IMAGE_FOR_BINDING.Count; ++i)
            {
                Cake_Image cake_Image = result.CAKE_IMAGE_FOR_BINDING[i];
                result.CAKE_IMAGE_FOR_BINDING[i].Link_Image = $"Images/{cake.ID_Cake}/{cake_Image.Ordinal_Number}.{cake_Image.Link_Image}";
                result.CAKE_IMAGE_FOR_BINDING[i].ImageIndex = i + 1;
            }

            return result;
        }

        public (List<Cake>, int) GetCakesSearchResult(string search_text, string condition, (string column, string type) sortedBy, int currentPage, int totalCakePerPage)
        {
            (List<Cake> CakesSearchResultList, int totalCakeSearch) result;
            List<Cake> CakesSearchResultList = new List<Cake>();
            int totalCakeSearch = 0;

            //Nhìn ở trên app thì cũng ngon đấy. Chứ đâu ai biết ở dưới app gồng mình catch exception
            try
            {
                string[] OPERATOR = { "and", "or", "and not" };

                //Chuẩn hóa hết mấy cái khoảng trắng thừa.
                //đưa hết mấy cái operator về and, or, and not. không để AND....
                search_text = GetStandardString(search_text);

                //Lấy hết mấy cái "abcd" vô cái stack để hồi pop ra.
                Stack<string> keywords = GetListKeyWords(search_text);

                //:V lấy hết and, or, and not đẩy vô queue.
                Queue<int> operators = GetListOperator(search_text);

                //Nếu số ngoặc kép " là lẻ thì để khỏi crash. thay " thành # :). Best sửa.
                //Tại sao lại là keywords.Count. Vì lúc lấy cái keywords ra thì chỉ có kết quả khi số " là chẵn. còn nếu " lẻ thì keywords sẽ k có phần tử nào.
                if (keywords.Count == 0)
                {
                    search_text = search_text.Replace("\"", "#");

                    string query = "";

                    if (condition.Length > 0)
                    {
                        query = $"SELECT COUNT(ID_Cake) FROM SearchByName(N'{search_text}') WHERE {condition}";
                    }
                    else
                    {
                        query = $"SELECT COUNT(ID_Cake) FROM SearchByName(N'{search_text}')";
                    }

                    totalCakeSearch = _databaseCakeShop
                        .Database
                        .SqlQuery<int>(query)
                        .Single();

                    if (totalCakeSearch > 0)
                    {
                        query = query.Replace("COUNT(ID_Cake)", "*");
                        query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalCakePerPage} ROWS FETCH NEXT {totalCakePerPage} ROWS ONLY";

                        CakesSearchResultList = _databaseCakeShop
                            .Database
                            .SqlQuery<Cake>(query)
                            .ToList();
                    }

                    //Thay xong rồi thì tìm bình thường thôi
                    //var CakesSearchResult = SearchByName(search_text).OrderByDescending(r => r.RANK);

                    //foreach (var CakeSearchResult in CakesSearchResult)
                    //{
                    //    var Cake = from r in Cakes
                    //                 where r.ID_Cake == CakeSearchResult.ID_Cake
                    //                 select r;
                    //    CakesSearchResultList.Add(Cake.FirstOrDefault());
                    //}

                }
                //Điều kiện này tại có nhiều khi nguòi ta chỉ nhập "ab" mà không có toán tử and, or, and not á. thì cũng tìm bình thường á.
                else if (operators.Count > 0)
                {
                    /*
                        Những cái mà dùng HashSet là để khỏi loại những kết quả trùng nhau thui. như kiểu để select distinct
                    */

                    //Cái này để hồi lấy kết quả sau khi thực hiện hết các phép toán tìm kiếm
                    HashSet<int> tempIDsResult = new HashSet<int>();

                    //Cái deathID này là lúc dùng and not á.
                    //Ví dụ "a" and not "b" là coi như thằng nào có "a b" là lấy id bỏ vô cái deadthID này
                    //Đến hồi xét mà có thằng nào nằm trong này là loại luôn
                    HashSet<int> deathID = new HashSet<int>();

                    //Bắt đầu với toán tử đầu tiên
                    int count = 1;

                    //Thực hiện đến khi nào hết toán tử
                    while (operators.Count > 0)
                    {
                        //Toán tử tìm kiếm đẩy vô queue từ trái sang phải. kiểu thực hiện từ trái sáng phải á.
                        var operatorStr = OPERATOR[operators.Dequeue() - 1];

                        //params1 là list các param1 :V 
                        //Lợi hại khi bắt đầu kết hợp nếu có từ 2 toán tử tìm kiếm trong search_text
                        List<string> params1 = new List<string>();

                        //Cái chỗ này là á. 
                        //Khi thực hiện "abc" opr "def" nó sẽ ra 1 list kết quả hoặc thậm chí là k có kết quả nào.
                        //Cần đếm số kết quả đó khi push vô stack để hồi pop ra cho đủ nên mới tồn tại cái count này.
                        //pop ra đủ thì mới thực hiện tiếp mấy cái toán tử sau cho nó chuẩn được
                        while (count > 0)
                        {
                            params1.Add(keywords.Pop());
                            --count;
                        }

                        //param2 thì chỉ có 1 thâu.
                        string param2 = keywords.Pop();

                        //Cái này để tránh bị trùng á
                        HashSet<string> tempKeyWords = new HashSet<string>();

                        //Bắt đầu quá trình thực hiện phép toán tìm kiếm
                        foreach (var param1 in params1)
                        {
                            string query = "";
                            //Tìm DeathID
                            if (operatorStr == "and not")
                            {
                                string deathSearchText = param1 + " " + "and" + " " + param2;

                                //var tempCakesSearchResultDeath = SearchByName(deathSearchText);
                                query = $"SELECT * FROM SearchByName(N'{deathSearchText}')";

                                var tempCakesSearchResultDeath = _databaseCakeShop
                                                                .Database
                                                                .SqlQuery<Cake>(query)
                                                                .ToList();

                                foreach (var tempCakeSearchResultDeath in tempCakesSearchResultDeath)
                                {
                                    deathID.Add(tempCakeSearchResultDeath.ID_Cake);
                                }
                            }

                            //Thực hiện tìm lần lượt nào
                            string tempSearchText = param1 + " " + operatorStr + " " + param2;

                            query = $"SELECT * FROM SearchByName(N'{tempSearchText}')";

                            var tempCakesSearchResult = _databaseCakeShop
                                                        .Database
                                                        .SqlQuery<Cake>(query)
                                                        .ToList();

                            count += tempCakesSearchResult.Count();

                            foreach (var tempCakeSearchResult in tempCakesSearchResult)
                            {
                                if (operators.Count == 0)
                                {
                                    tempIDsResult.Add(tempCakeSearchResult.ID_Cake);
                                }
                                else
                                {
                                    //Add cái tên mới tìm ra để tí kết vào tìm theo operator sau tiếp
                                    tempKeyWords.Add("\"" + tempCakeSearchResult.Name_Cake + "\"");
                                }

                            }

                            while (tempKeyWords.Count > 0)
                            {
                                keywords.Push(tempKeyWords.First());
                                tempKeyWords.Remove(tempKeyWords.First());
                            }
                        }
                    }

                    //Lấy kểt quả cuối
                    bool hasConditionBefore = (condition.Length > 0 ? true : false);
                    string resultQuery = "";

                    if (tempIDsResult.Count > 0)
                    {
                        if (condition.Length > 0)
                        {
                            condition += " AND (";
                        }
                        else
                        {
                            //Do Nothing
                        }

                        foreach (var tempID in tempIDsResult)
                        {
                            if (!deathID.Contains(tempID))
                            {
                                condition += $" ID_Cake = {tempID} OR";
                            }
                            else
                            {
                                //Do Nothing
                            }
                        }

                        if (condition.Length > 0)
                        {
                            //Select * from [dbo].[Cake] where FAVORITE_FLAG = 1 AND (FOOD_GROUP = N'Ăn sáng' OR FOOD_GROUP = N'Món chính')
                            //Select * from [dbo].[Cake] where FOOD_GROUP = N'Ăn sáng'
                            condition = condition.Substring(0, condition.Length - 3);

                            if (hasConditionBefore)
                            {
                                condition += ")";
                            }
                            else
                            {
                                //Do Nothing
                            }

                            if (condition.Length > 0)
                            {
                                resultQuery = $"SELECT COUNT(ID_Cake) FROM Cake WHERE {condition}";
                            }
                        }
                        else
                        {
                            //Do Nothing
                        }

                        totalCakeSearch = _databaseCakeShop
                            .Database
                            .SqlQuery<int>(resultQuery)
                            .Single();

                        if (totalCakeSearch > 0)
                        {
                            resultQuery = resultQuery.Replace("COUNT(ID_Cake)", "*");
                            resultQuery += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalCakePerPage} ROWS FETCH NEXT {totalCakePerPage} ROWS ONLY";

                            CakesSearchResultList = _databaseCakeShop
                            .Database
                            .SqlQuery<Cake>(resultQuery)
                            .ToList();
                        }
                    }
                    else
                    {
                        //Do Nothing
                    }



                }
                else
                {
                    string query = "";

                    if (condition.Length > 0)
                    {
                        query = $"SELECT COUNT(ID_Cake) FROM SearchByName(N'{search_text}') WHERE {condition}";
                    }
                    else
                    {
                        query = $"SELECT COUNT(ID_Cake) FROM SearchByName(N'{search_text}')";
                    }

                    totalCakeSearch = _databaseCakeShop
                        .Database
                        .SqlQuery<int>(query)
                        .Single();

                    if (totalCakeSearch > 0)
                    {
                        query = query.Replace("COUNT(ID_Cake)", "*");
                        query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalCakePerPage} ROWS FETCH NEXT {totalCakePerPage} ROWS ONLY";

                        CakesSearchResultList = _databaseCakeShop
                        .Database
                        .SqlQuery<Cake>(query)
                        .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }

            for (int i = 0; i < CakesSearchResultList.Count; ++i)
            {
                CakesSearchResultList[i] = this.getCakeForBinding(CakesSearchResultList[i]);
            }

            result.CakesSearchResultList = CakesSearchResultList;
            result.totalCakeSearch = totalCakeSearch;

            return result;
        }

        public string GetStandardString(string srcString)
        {
            string result = srcString;

            result = result.Trim();

            while (result.IndexOf("  ") != -1)
            {
                result = result.Replace("  ", " ");
            }

            result = result.ToLower();

            return result;
        }

        //Lấy cái list KeyWord để search nè.
        public Stack<string> GetListKeyWords(string search_text)
        {
            Stack<string> result = new Stack<string>();
            Stack<string> temp = new Stack<string>();

            List<int> indexQuotes = new List<int>();

            for (int i = 0; i < search_text.Length; ++i)
            {
                if (search_text[i] == '"')
                {
                    indexQuotes.Add(i);
                }
                else
                {
                    //Do Nothing
                }
            }

            if (indexQuotes.Count % 2 == 0)
            {
                for (int i = 0; i < indexQuotes.Count - 1; i += 2)
                {
                    string tempString = "";

                    for (int j = indexQuotes[i]; j <= indexQuotes[i + 1]; ++j)
                    {
                        tempString += search_text[j];
                    }

                    //"abc" and "123"
                    if (tempString.Length > 2)
                    {
                        temp.Push(tempString);
                    }
                }
            }

            while (temp.Count > 0)
            {
                result.Push(temp.Pop());
            }

            return result;
        }

        //Lấy cái list toán tử nè 
        public Queue<int> GetListOperator(string search_text)
        {
            Queue<int> result = new Queue<int>();

            var tokens = search_text.Split(' ');

            for (int i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i] == "and")
                {
                    if (i + 1 < tokens.Length && tokens[i + 1] == "not")
                    {
                        result.Enqueue(3);
                    }
                    else
                    {
                        result.Enqueue(1);
                    }
                }
                else if (tokens[i] == "or")
                {
                    result.Enqueue(2);
                }
            }
            return result;
        }

        public int GetMaxOrdinalNumberImage(int ID_Cake)
        {
            int result = _databaseCakeShop
                .Database
                .SqlQuery<int>($"Select Max(Ordinal_Number) From Cake_Image Where ID_Cake = {ID_Cake}")
                .FirstOrDefault();

            return result;
        }

        public int GetMaxIDCake()
        {
            int result = _databaseCakeShop
                .Database
                .SqlQuery<int>($"Select Max(ID_Cake) From Cake")
                .FirstOrDefault();

            return result;
        }
        public int GetMaxIDStock()
        {
            int result = _databaseCakeShop
                .Database
                .SqlQuery<int>($"Select Max(ID_Stock) From StockReceiving")
                .FirstOrDefault();

            return result;
        }

        public int AddCake(Nullable<int> id, string name, string des, string type, Nullable<decimal> orPrice, Nullable<decimal> sellPrice, Nullable<int> quantity, string link)
        {
            try
            {
                _databaseCakeShop.AddCake(id, name, des, type, orPrice, sellPrice, quantity, link);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return 0;
        }

        public int UpdateCake(int ID_Cake, string name, string des, string type, Nullable<decimal> orPrice, Nullable<decimal> sellPrice, Nullable<int> quantity, string link)
        {
            try
            {
                _databaseCakeShop
                .Database
                .ExecuteSqlCommand($"UPDATE Cake SET Name_Cake = N'{name}', Description = N'{des}', Type_Cake = N'{type}', Original_Price = {orPrice}, Selling_Price = {sellPrice}, Current_Quantity = {quantity}, Link_Avt = N'{link}' WHERE ID_CAKE = {ID_Cake}");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return 0;
        }
        
        public void UpdateImage(int ID_Cake, int ordinal_number, string link_image, int is_active)
        {
            var checkExist = _databaseCakeShop
                .Database
                .SqlQuery<Cake_Image>($"Select * from Cake_Image where Ordinal_Number = {ordinal_number} and ID_Cake = {ID_Cake}")
                .FirstOrDefault();

            if (checkExist != null)
            {
                _databaseCakeShop
               .Database
               .ExecuteSqlCommand($"Update Cake_Image Set Link_Image = N'{link_image}', Is_Active = {is_active} Where Ordinal_Number = {ordinal_number} And ID_Cake = {ID_Cake}");
            }
            else
            {
                AddCakeImage(ID_Cake, ordinal_number, link_image, is_active);
            }
        }

        public void AddCakeImage(int ID_Cake, int ordinal_number, string link_image, int is_active)
        {
            _databaseCakeShop
               .Database
               .ExecuteSqlCommand($"INSERT [dbo].[Cake_Image]([ID_Cake], [Ordinal_Number], [Link_Image], [Is_Active]) VALUES({ID_Cake}, {ordinal_number}, N'{link_image}', {is_active})");

        }

        public int AddStock(int ID_Stock, int ID_Cake, int quantity, DateTime date)
        {
            _databaseCakeShop
                .Database
                .ExecuteSqlCommand($"INSERT [dbo].[StockReceiving]([ID_Stock], [ID_Cake], [Quantity], [Date]) VALUES({ID_Stock}, {ID_Cake}, {quantity}, '{date}')");

            return 1;
        }

        public List<Invoice> GetAllInvoice()
        {
            List<Invoice> result = _databaseCakeShop
                .Database
                .SqlQuery<Invoice>("Select * From Invoice")
                .ToList();

            for (int i = 0; i < result.Count; ++i)
            {
                result[i].ID_FOR_BINDING = $"CS-{result[i].ID_Invoice}";
                result[i].TOTAL_COST_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(decimal.ToInt32(result[i].Total_Money ?? 0));
            }

            return result;
        }

        public List<Cake> GetAllCake()
        {
            List<Cake> result = _databaseCakeShop
                .Database
                .SqlQuery<Cake>("Select * from Cake where Current_Quantity > 0")
                .ToList();

            for (int i = 0; i < result.Count; ++i)
            {
                result[i].SELLING_PRICE_INT_FOR_BINDING = decimal.ToInt32(result[i].Selling_Price ?? 0) ;
                result[i].SELLING_PRICE_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(result[i].SELLING_PRICE_INT_FOR_BINDING);
                result[i].ORIGINAL_PRICE_INT_FOR_BINDING = decimal.ToInt32(result[i].Original_Price ?? 0);
                result[i].ORIGINAL_PRICE_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(result[i].ORIGINAL_PRICE_INT_FOR_BINDING);
            }

            return result;
        }

        public void AddInvoice(Nullable<int> id, Nullable<System.DateTime> date, string name, string add, string phone, Nullable<decimal> ship, Nullable<decimal> total)
        {
   
            _databaseCakeShop.AddInvoice(id, date, name, add, phone, ship, total);

        }

        public void AddInvoiceDetail(Nullable<int> id, Nullable<int> orNum, Nullable<int> idCake, Nullable<int> quantity)
        {
            _databaseCakeShop.AddInvoiceDetail(id, orNum, idCake, quantity);
        }

        public int GetMaxIDInvoice()
        {
            int result = _databaseCakeShop
                .Database
                .SqlQuery<int>($"Select Max(ID_Invoice) From Invoice")
                .FirstOrDefault();

            return result;
        }

        public int CalcTotalCurrentCake()
        {
            int result = _databaseCakeShop
                .Database
                .SqlQuery<int>("Select Sum(Current_Quantity) From Cake")
                .FirstOrDefault();

            return result;
        }

        public StatisticByYear_Result StatisticByYear(Nullable<int> year)
        {
            var result = _databaseCakeShop
                .StatisticByYear(year)
                .FirstOrDefault();

            result.SumStockReveivingMoney_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(decimal.ToInt32(result.SumStockReveivingMoney ?? 0));
            result.SumRevenue_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(decimal.ToInt32(result.SumRevenue ?? 0));
            result.SumProfit_FOR_BINDING = _applicationUtilities.GetMoneyForBinding(decimal.ToInt32(result.SumProfit ?? 0));


            return result;
        }

        public double GetRevenueByMonthInYear(int month, int year)
        {
            var result = _databaseCakeShop
                .StatisticRevenueByMonthInYear(month, year)
                .FirstOrDefault();

            return decimal.ToDouble(result.SumRevenue ?? 0);
        }

        public List<StatisticRevenueByTypeOfCakeInYear_Result> statisticRevenueByTypeOfCakeInYear_Results(int year)
        {
            return _databaseCakeShop.StatisticRevenueByTypeOfCakeInYear(year).ToList();
        }

        public List<int> GetAllYear()
        {
            return _databaseCakeShop
                .Database
                .SqlQuery<int>("SELECT DISTINCT(YEAR(Date)) FROM dbo.[StockReceiving]")
                .ToList();
        }
    }
}
