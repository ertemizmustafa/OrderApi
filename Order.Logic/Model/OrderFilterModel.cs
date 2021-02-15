using System;
using System.Collections.Generic;

namespace Order.Logic.Model
{
    public class OrderFilterModel
    {
        public int PageSize { get; set; }
        //Kaçıncı sayfanın döndüğü (Örneğin PageNumber=2, PageSize=25 iken 26-50 arasındaki sonuçları dönmeli)
        public int PageNumber { get; set; }
        //Arama metni, StoreName ve CustomerName değerleri üzerinde arama yapılabiliyor olmalı
        public string SearchText { get; set; }
        //Order CreatedOn değerinin minimum olması gereken değer
        public DateTime? StartDate { get; set; }
        //Order CreatedOn değerinin maximum olması gereken değer
        public DateTime? EndDate { get; set; }
        //Filtrelemek için alınan liste, örneğin [10,20] aldığı zaman sadece Created ve InProgress olan Order'lar dönmeli
        public List<int> Statuses { get; set; }
        //Gelen sonuçlar hangi Order alanına göre sıralanacak (ascending olarak sıralanmalı)
        //"Id", "BrandId", "Price", "StoreName", "CustomerName", "CreatedOn", "Status" değerlerini alabilir
        public string SortBy { get; set; }
    }
}
